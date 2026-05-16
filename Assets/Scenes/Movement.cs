using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Intento de RB
    /*
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

    }*/

    private CharacterController _Controller;
    public float AirSpeed;
    public float GroundSpeed;

    public float jumpspeed;

    public float aceleracion;
    public float speed;
    public float TargetSpeed;
    public Vector3 currentSpeed;
    public float friccionAire; //(más alto = frena antes)

    private Vector3 fuerza;
    public float gravedad;



    private GroundChecker _GroundChecker;
    public PortalChecker PortalChecker;

    public float empuje;

    //private FMODUnity.StudioEventEmitter pasosEmitter;
    void Start()
    {
        _Controller = GetComponent<CharacterController>();
        _GroundChecker = GetComponentInChildren<GroundChecker>();
    }


    void Update()
    {
        mover();
        AplicarGravedad();
        if (Input.GetKeyDown(KeyCode.Space) && _GroundChecker.Tocando)
        {
            salto();
        }
    }

    private void salto()
    {
        fuerza.y = jumpspeed;
    }

    private void mover()
    {

        if (_GroundChecker.Tocando && !PortalChecker.EnZonaDePortal)
        {
            speed = GroundSpeed;
        }
        else
        {
            speed = AirSpeed;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movertarget = (transform.right * x + transform.forward * z) * speed; //El move original

        currentSpeed = Vector3.Lerp(currentSpeed, movertarget, aceleracion * Time.deltaTime);


        _Controller.Move(currentSpeed * Time.deltaTime);
    }

    private void AplicarGravedad()
    {

        if (_GroundChecker.Tocando && fuerza.y < 0)
        {
            fuerza.y = -2f;
        }

        float frenado;
        if (_GroundChecker.Tocando)
        {
            frenado = 10f;
        }
        else
        {
            frenado = friccionAire;
        } // Más fricción en el suelo
        fuerza.x = Mathf.Lerp(fuerza.x, 0, frenado * Time.deltaTime);
        fuerza.z = Mathf.Lerp(fuerza.z, 0, frenado * Time.deltaTime);

        fuerza.y += gravedad * Time.deltaTime;
        _Controller.Move(fuerza * Time.deltaTime);
    }

    //// --- NUEVA FUNCIÓN PARA CONTROLAR EL AUDIO LARGO ---
    //private void ControlarAudioPasos()
    //{
    //    if (pasosEmitter == null) return;

    //    // Comprobamos si el jugador se está moviendo de verdad en los ejes X o Z
    //    bool seEstaMoviendo = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

    //    // Queremos que suene si: Toca el suelo AND Se está moviendo AND No está en la zona del portal
    //    if (_GroundChecker.Tocando && seEstaMoviendo && !PortalChecker.EnZonaDePortal)
    //    {
    //        // Si el audio no está reproduciéndose ya, lo encendemos
    //        if (!pasosEmitter.IsPlaying())
    //        {
    //            pasosEmitter.Play();
    //        }
    //    }
    //    else
    //    {
    //        // Si se para, salta o va por el aire, apagamos el audio largo
    //        if (pasosEmitter.IsPlaying())
    //        {
    //            pasosEmitter.Stop();
    //        }
    //    }
    //}

    public void AplicarVelocidad(Vector3 Velocidad)
    {
        fuerza = Velocidad * empuje;
    }
}
