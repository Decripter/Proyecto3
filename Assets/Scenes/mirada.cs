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
            // ¿Tiene Rigidbody O tiene el script de Gravedad?

            if (hit.transform.GetComponent<Rigidbody>() != null || hit.transform.GetComponent<CambioGravedad>() != null)
            {
                detectar = true;
            }

        }
            Mira.SetBool("Est", detectar);
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, Objeto) && Input.GetKeyDown(KeyCode.E))
        {
            if (!Ocupado)
            {
                Debug.Log(hit.transform.name);
                rbExt = hit.transform.GetComponent<Rigidbody>();
                fj.connectedBody = rbExt;
                Ocupado = true;
            }

            else
            {
                Debug.Log("soltando");
                fj.connectedBody = null; //Lo desenganchamos, pero luego hay que resetear las fisicas del objeto
                rbExt.linearVelocity = Vector3.zero;
                rbExt.angularVelocity = Vector3.zero;
                Ocupado = false;
            }

        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance) && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Invirtiendo");
            Gravedad = hit.transform.GetComponent<CambioGravedad>();

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


        RaycastHit hitPared;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(1))
        {
            //Vector3 Separar = new Vector3(SepararX, 0f, 0f);

            PortalA.transform.position = hitPared.point + (hitPared.normal * SepararX);
            //PortalA.transform.rotation = Quaternion.FromToRotation(PortalA.transform.forward, hitPared.normal) * PortalA.transform.rotation;
            PortalA.transform.rotation = Quaternion.LookRotation(hitPared.normal);
            Debug.Log("Portal");
        }

        RaycastHit hitPared2;

        if (Physics.Raycast(transform.position, transform.forward, out hitPared2, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(0))
        {
            //Vector3 Separar = new Vector3(SepararX, 0f, 0f);

            PortalB.transform.position = hitPared2.point + (hitPared2.normal * SepararX);
            //PortalB.transform.rotation = Quaternion.FromToRotation(PortalB.transform.forward, hitPared2.normal) * PortalB.transform.rotation;
            PortalB.transform.rotation = Quaternion.LookRotation(hitPared2.normal);
            Debug.Log("Portal");
        }
        Debug.DrawRay(transform.position, transform.forward * DistanciaPortal, Color.blue);

    }

}