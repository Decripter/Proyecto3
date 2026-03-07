using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class mirada : MonoBehaviour
{
    public GameObject Robert;
    public float speed = 100f;
    private float girox = 0f;

    private Rigidbody rb;
    private FixedJoint fj;
    private Rigidbody rbExt;

    public float rayDistance;

    public bool Ocupado;
    private CambioGravedad Gravedad;

    public LayerMask Objeto;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        fj = GetComponent<FixedJoint>();
        rb = GetComponent<Rigidbody>();
        Ocupado = false;
    }

    
    void Update()
    {
        float x = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

        girox -= y;
        girox = Mathf.Clamp(girox, -90f, 90f);

        transform.localRotation = Quaternion.Euler(girox, 0 ,0);
        Robert.transform.Rotate(Vector3.up * x);


        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, Objeto) && Input.GetKeyDown(KeyCode.E) )
        {
            if(!Ocupado)
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
        }

    }
}
