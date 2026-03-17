using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _Controller;
    public float AirSpeed = 5f;
    public float GroundSpeed = 15f;

    public float jumpspeed = 6.9f;

    public float aceleracion = 5f;
    public float speed;
    public float TargetSpeed;
    public Vector3 currentSpeed;
    public float friccionAire = 2f; //(más alto = frena antes)

    private Vector3 fuerza;
    public float gravedad = -9.8f;

    private GroundChecker _GroundChecker;
    public PortalChecker PortalChecker;
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

        if(_GroundChecker.Tocando && fuerza.y < 0)
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

    public void AplicarVelocidad(Vector3 Velocidad)
    {
        fuerza = Velocidad;
    }

}
