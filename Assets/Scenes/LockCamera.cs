using Unity.Mathematics;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    public float rotacionX = 90;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(rotacionX, 0,0);
    }
}
