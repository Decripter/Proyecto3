using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections;

public class mirada : MonoBehaviour
{
    public GameObject Robert;
    public float speed = 100f;
    private float girox = 0f;

    private Rigidbody rb;
    private FixedJoint fj;
    private Rigidbody rbExt;

    public bool Ocupado;
    private CambioGravedad Gravedad;

    public LayerMask Objeto;
    public LayerMask MuroPortal;
    public float rayDistance;

    public Animator Mira;

    [Header("Sonidos de FMOD")]
    public FMODUnity.EventReference sonidoAgarre; 
    public FMODUnity.EventReference sonidoSoltar;

    public float DistanciaPortal;
    public float SepararX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        fj = GetComponent<FixedJoint>();
        rb = GetComponent<Rigidbody>();
        Ocupado = false;
    }

    void Update()
    {
        // --- MOVIMIENTO DE CÁMARA ---
        float x = Input.GetAxis("Mouse X") * speed;
        float y = Input.GetAxis("Mouse Y") * speed;

        girox -= y;
        girox = Mathf.Clamp(girox, -90f, 90f);
        transform.localRotation = Quaternion.Euler(girox, 0, 0);
        Robert.transform.Rotate(Vector3.up * x);

        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

        // --- SISTEMA DE INTERACCIÓN POR INTERFAZ ---
        RaycastHit hit;
        IInteractable interactableActual = null;

        // Lanzamos el rayo (ya no filtramos por Layer, choca con todo)
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {
            // Intentamos obtener la interfaz. Si la tiene, se guarda en interactableActual
            hit.collider.TryGetComponent<IInteractable>(out interactableActual);
        }

        // Animación de la mira (se expande si interactableActual no es nulo)
        Mira.SetBool("Est", interactableActual != null && !Ocupado);

        // --- LÓGICA DE INPUT (TECLA E) ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Ocupado)
            {
                SoltarObjeto();
            }
            else if (interactableActual != null)
            {
                // 1. ¿Es un objeto físico/agarrable? 
                // Comprobamos si el componente real es de tipo 'ObjetoFisico'
                if (interactableActual is Objeto)
                {
                    interactableActual.Interactuar(this); // Le pasamos la mirada para el FixedJoint
                }
                else
                {
                    // 2. Si no es un objeto físico, asumimos que es un botón, nota o palanca
                    interactableActual.Interactuar(); // Ejecuta la versión limpia sin parámetros
                }
            }
        }
    }

    // --- MÉTODOS PÚBLICOS PARA LOS OBJETOS ---
    public void AgarrarObjeto(Rigidbody rbObjeto)
    {
        rbExt = rbObjeto;
        fj.connectedBody = rbExt;
        Ocupado = true;
        FMODUnity.RuntimeManager.PlayOneShot(sonidoAgarre, transform.position);
    }

    public void SoltarObjeto()
    {
        fj.connectedBody = null;

        if (rbExt != null)
        {
            rbExt.linearVelocity = Vector3.zero;
            rbExt.angularVelocity = Vector3.zero;
            rbExt = null;
        }

        Ocupado = false;

        if (!sonidoSoltar.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sonidoSoltar, transform.position);
        }
    }

    public void ResetearMirada()
    {
        girox = 0f;
        transform.localRotation = Quaternion.identity;
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

}