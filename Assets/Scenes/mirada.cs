using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections;

public class mirada : MonoBehaviour
{
    public GameObject Robert;
    public float speed = 100f;
    private float girox = 0f;

    public GameObject PortalA;
    public GameObject PortalB;

    private Rigidbody rb;
    private FixedJoint fj;
    private Rigidbody rbExt;

    public float rayDistance;
    public float DistanciaPortal;

    public bool Ocupado;
    private CambioGravedad Gravedad;

    public LayerMask Objeto;
    public LayerMask MuroPortal;

    public float SepararX;
    public Animator Mira;

    [Header("Sonidos de FMOD")]
    public FMODUnity.EventReference sonidoPortalAzul;   // Clic Izquierdo
    public FMODUnity.EventReference sonidoPortalNaranja; // Clic Derecho
    public FMODUnity.EventReference sonidoAgarre;        // Tecla E

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
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance) && Input.GetKeyDown(KeyCode.Z))
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
        }

        // --- PORTAL NARANJA (CLIC DERECHO) ---
        RaycastHit hitPared;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(1))
        {
            PortalA.transform.position = hitPared.point + (hitPared.normal * SepararX);
            PortalA.transform.rotation = Quaternion.LookRotation(hitPared.normal);

            // REPRODUCIR SONIDO
            FMODUnity.RuntimeManager.PlayOneShot(sonidoPortalNaranja, transform.position);
            Debug.Log("Portal Naranja");
        }

        // --- PORTAL AZUL (CLIC IZQUIERDO) ---
        RaycastHit hitPared2;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared2, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(0))
        {
            PortalB.transform.position = hitPared2.point + (hitPared2.normal * SepararX);
            PortalB.transform.rotation = Quaternion.LookRotation(hitPared2.normal);

            // REPRODUCIR SONIDO
            FMODUnity.RuntimeManager.PlayOneShot(sonidoPortalAzul, transform.position);
            Debug.Log("Portal Azul");
        }

        Debug.DrawRay(transform.position, transform.forward * DistanciaPortal, Color.blue);
    }
}