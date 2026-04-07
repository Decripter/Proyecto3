using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 Vector_Movimiento;
    private Rigidbody rb;
    
    [SerializeField]private float Speed;
    
    [SerializeField]private float Salto;

    private GroundChecker _GroundChecker;
    public PortalChecker PortalChecker;

    public float empuje;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _GroundChecker = GetComponentInChildren<GroundChecker>();
    }

    
    void Update()
    {
        Vector_Movimiento = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space) && _GroundChecker.Tocando)
        {
            salto();
        }
    }

    private void FixedUpdate()
    {
        mover();
        AplicarGravedad();

    }

    private void salto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _GroundChecker.Tocando)
        {
            rb.AddForce(Vector3.up * Salto, ForceMode.Impulse);
        }
    }

    private void mover()
    {
        Vector3 MoverVector = transform.TransformDirection(Vector_Movimiento) * Speed;
        rb.linearVelocity = new Vector3(MoverVector.x, rb.linearVelocity.y, MoverVector.z);

        if (_GroundChecker.Tocando && !PortalChecker.EnZonaDePortal)
        {

        }
        else
        {

        }


    }

    private void AplicarGravedad()
    {

    }

    public void AplicarVelocidad(Vector3 Velocidad)
    {

    }

}
