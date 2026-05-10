using UnityEngine;

public class ArmaPortales : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject PortalA;
    public GameObject PortalB;
    public float rayDistance;
    public float DistanciaPortal;
    public float SepararX;

    public LayerMask Objeto;
    public LayerMask MuroPortal;

    [Header("Sonidos de FMOD")]
    public FMODUnity.EventReference sonidoPortalAzul;   // Clic Izquierdo
    public FMODUnity.EventReference sonidoPortalNaranja; // Clic Derecho
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // --- PORTAL NARANJA (CLIC DERECHO) ---
        RaycastHit hitPared;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(1))
        {
            PortalA.transform.position = hitPared.point + (hitPared.normal * SepararX);
            PortalA.transform.rotation = Quaternion.LookRotation(hitPared.normal);

            // REPRODUCIR SONIDO
            FMODUnity.RuntimeManager.PlayOneShot(sonidoPortalNaranja, transform.position);
            Debug.Log("Portal Naranja");
        }

        // --- PORTAL AZUL (CLIC IZQUIERDO) ---
        RaycastHit hitPared2;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared2, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(0))
        {
            PortalB.transform.position = hitPared2.point + (hitPared2.normal * SepararX);
            PortalB.transform.rotation = Quaternion.LookRotation(hitPared2.normal);

            // REPRODUCIR SONIDO
            FMODUnity.RuntimeManager.PlayOneShot(sonidoPortalAzul, transform.position);
            Debug.Log("Portal Azul");
        }

        Debug.DrawRay(transform.position, transform.forward * DistanciaPortal, Color.blue);
    }
}
