using UnityEngine;

public class Reset_Cubo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector3 m_Position;
    private Quaternion m_Rotation;
    private Rigidbody Rb;
    private CambioGravedad Gravedad;
    void Start()
    {
        m_Position = transform.position;
        m_Rotation = transform.rotation;
        Rb = GetComponent<Rigidbody>();
        Gravedad = transform.GetComponent<CambioGravedad>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Gravedad.Alterar(-9.8f);
            Rb.linearVelocity = Vector3.zero;
            Rb.angularVelocity = Vector3.zero;

            transform.position = m_Position;
            transform.rotation = m_Rotation;
        }
    }
}
