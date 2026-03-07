using UnityEngine;

public class Fisicas : MonoBehaviour
{
    public float Fuerza = 5;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.transform.name);
        var other = hit.transform.GetComponent<Rigidbody>();
        if (other != null)
        {
            Vector3 force = hit.moveDirection * Fuerza;
            other.AddForce(force);
        }

    }
}
