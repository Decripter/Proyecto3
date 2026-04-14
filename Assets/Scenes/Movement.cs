using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] public float Speed;   
    [SerializeField] private float Salto;
    [SerializeField] private float gravedad = -9.81f;
    public float verticalVelocity;
    public Vector3 momentumPortal;

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
        if (Input.GetKeyDown(KeyCode.Space) && _GroundChecker.Tocando && Mathf.Abs(rb.linearVelocity.y) <= 0)
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
        Debug.Log("Saltando");
        rb.AddForce(transform.up * Salto);
    }

    private void mover()
    {
        // 2. Input WASD
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direccionTarget = (transform.forward * v + transform.right * h).normalized * Speed;

        Vector3 MovimientoTotal = transform.position;
        MovimientoTotal += direccionTarget * Speed * Time.deltaTime;

        rb.MovePosition(MovimientoTotal);

        
    }

    private void AplicarGravedad()
    {
            rb.AddForce(-transform.up * -9.8f * gravedad);
    }
    public void AplicarVelocidad(Vector3 Velocidad)
    {

    }

}
