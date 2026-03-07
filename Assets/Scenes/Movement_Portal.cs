using Unity.VisualScripting;
using UnityEngine;

public class Movement_Portal : MonoBehaviour
{
    private CharacterController _Controller;
    public float GroundSpeed = 15f;

    public float aceleracion = 5f;
    public float speed;
    public float TargetSpeed;
    public Vector3 currentSpeed;

    public bool activo;
    public float gravedad = -9.8f;

    void Start()
    {
        _Controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        if(activo)
        {
            
        }
            mover();
    }
    private void mover()
    {
            speed = GroundSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 movertarget = (transform.right * x + transform.forward * z) * speed; //El move original
        currentSpeed = Vector3.Lerp(currentSpeed, movertarget, aceleracion * Time.deltaTime);
        

       _Controller.Move(currentSpeed * Time.deltaTime);
    }

}
