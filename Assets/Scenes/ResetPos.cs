using UnityEngine;

public class ResetPos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject TP_R;
    public GameObject TP_T;
    public GameObject TP_Y;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rigidbody rb = transform.GetComponent<Rigidbody>();
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = TP_R.transform.position;
            transform.rotation = TP_R.transform.rotation;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = TP_T.transform.position;
            transform.rotation = TP_T.transform.rotation;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = TP_Y.transform.position;
            transform.rotation = TP_Y.transform.rotation;
        }
    }
}
