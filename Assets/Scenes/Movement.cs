using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
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

    private Vector3 velocidadPlataforma;
    private Rigidbody plataformaActual;
    private Vector3 ultimaVelocidadPlataforma;

    //private FMODUnity.StudioEventEmitter pasosEmitter;
    void Start()
    {
        _Controller = GetComponent<CharacterController>();
        _GroundChecker = GetComponentInChildren<GroundChecker>();
    }


    void Update()
    {
        ActualizarPlataforma(); // Leemos la mesa antes de movernos

        mover();
        AplicarGravedad();
        if (Input.GetKeyDown(KeyCode.Space) && _GroundChecker.Tocando)
        {
            salto();
        }
    }

    // --- NUEVA FUNCIÓN: EL EFECTO POLIZÓN ---
    private void ActualizarPlataforma()
    {
        velocidadPlataforma = Vector3.zero;

        if (_GroundChecker.Tocando)
        {
            if (Physics.Raycast(_GroundChecker.transform.position, Vector3.down, out RaycastHit hit, 0.5f))
            {
                Rigidbody rbSuelo = hit.collider.GetComponent<Rigidbody>();

                if (rbSuelo != null)
                {
                    velocidadPlataforma = rbSuelo.linearVelocity;

                    // --- 1. EL FRENAZO BRUSCO (Mantenimiento de Inercia en suelo) ---
                    // Restamos la velocidad pasada con la actual para ver si frenó de golpe
                    Vector3 decelaracion = ultimaVelocidadPlataforma - velocidadPlataforma;

                    if (decelaracion.magnitude > 3f) // Umbral de frenazo brusco
                    {
                        // Le metemos esa fuerza fantasma al jugador para que resbale hacia adelante
                        fuerza += decelaracion;
                    }

                    // Guardamos los datos para el frame que viene
                    ultimaVelocidadPlataforma = velocidadPlataforma;
                    plataformaActual = rbSuelo;
                }
                else
                {
                    // Pisamos suelo estático normal, borramos la memoria
                    ResetearInerciaPlataforma();
                }
            }
        }
        else
        {
            // --- 2. EL SALTO DEL POLIZÓN (Mantenimiento de Inercia en el aire) ---
            // Acabamos de dejar de tocar el suelo, pero en el frame anterior estábamos en una plataforma
            if (plataformaActual != null)
            {
                // Te sumamos la inercia a tu cuerpo para que sigas volando en esa dirección
                fuerza.x += ultimaVelocidadPlataforma.x;
                fuerza.z += ultimaVelocidadPlataforma.z;

                // Si la plataforma estaba subiendo cuando saltaste, te da un empujón extra hacia arriba
                if (ultimaVelocidadPlataforma.y > 0)
                {
                    fuerza.y += ultimaVelocidadPlataforma.y;
                }

                ResetearInerciaPlataforma();
            }
        }
    }

    private void ResetearInerciaPlataforma()
    {
        plataformaActual = null;
        ultimaVelocidadPlataforma = Vector3.zero;
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


        //_Controller.Move(currentSpeed * Time.deltaTime);
        _Controller.Move((currentSpeed + velocidadPlataforma) * Time.deltaTime);
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
            frenado = 3f;
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

    public void AplicarVelocidad(Vector3 Velocidad)
    {
        fuerza = Velocidad * empuje;
    }
}
