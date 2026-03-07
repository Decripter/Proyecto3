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


    private Vector3 fuerza;
    public float gravedad = -9.8f;

    private GroundChecker _GroundChecker;
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

        if (_GroundChecker.Tocando)
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

        fuerza.y += gravedad * Time.deltaTime;
        _Controller.Move(fuerza * Time.deltaTime);
    }

}
