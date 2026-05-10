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
    public FMODUnity.EventReference sonidoAgarre;        // Tecla E

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
        float x = Input.GetAxis("Mouse X") * speed;
        float y = Input.GetAxis("Mouse Y") * speed;

        girox -= y;
        girox = Mathf.Clamp(girox, -90f, 90f);

        transform.localRotation = Quaternion.Euler(girox, 0, 0);
        Robert.transform.Rotate(Vector3.up * x);

        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

        RaycastHit hit;
        bool detectar = false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, Objeto))
        {
            if (hit.transform.GetComponent<Rigidbody>() != null || hit.transform.GetComponent<CambioGravedad>() != null)
            {
                detectar = true;
            }
        }

        Mira.SetBool("Est", detectar);

        // --- AGARRAR OBJETOS (TECLA E) ---
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, Objeto) && Input.GetKeyDown(KeyCode.E))
        {
            if (!Ocupado)
            {
                rbExt = hit.transform.GetComponent<Rigidbody>();
                fj.connectedBody = rbExt;
                Ocupado = true;
                // Sonido al agarrar
                FMODUnity.RuntimeManager.PlayOneShot(sonidoAgarre, transform.position);
            }
            else
            {
                fj.connectedBody = null;
                rbExt.linearVelocity = Vector3.zero;
                rbExt.angularVelocity = Vector3.zero;
                Ocupado = false;
                // Sonido al soltar (puedes usar el mismo o uno diferente)
                FMODUnity.RuntimeManager.PlayOneShot(sonidoAgarre, transform.position);
            }
        }

        // --- INVERTIR GRAVEDAD (TECLA Z) ---
        /*if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance) && Input.GetKeyDown(KeyCode.Z))
        {
            Gravedad = hit.transform.GetComponent<CambioGravedad>();
            if (Gravedad != null)
            {
                if (Gravedad.GetValor() < 0)
                {
                    Gravedad.Alterar(9.8f);
                    Gravedad.Invertir = false;
                }
                else
                {
                    Gravedad.Alterar(-9.8f);
                }
                Gravedad.StartCoroutine(Gravedad.EfectoInversion());
            }
        }*/


    }
}