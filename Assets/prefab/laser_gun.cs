using UnityEngine;

public class laser_gun : MonoBehaviour
{
    public Transform firepoint;
    public float distance = 10; //distacia maxima
    public LayerMask mascara;
    private LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.U))
        {
            fire();
        }
    }

    void fire()
    {
        RaycastHit hit;


        lineRenderer.SetPosition(0, firepoint.position);
        
        if (Physics.Raycast(firepoint.position, firepoint.forward, out hit, distance)) //origen del raycast, direccion del raycast, hit info, distancia maxima
        {
            Debug.Log("Hit: " + hit.transform.name);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            //Destroy(hit.transform.gameObject);
        }
        else
        {
            Vector3 pos = firepoint.position + firepoint.forward * distance;
            lineRenderer.SetPosition(1, pos);
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
        }


    }
}
