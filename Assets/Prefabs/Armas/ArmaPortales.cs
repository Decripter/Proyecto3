using System.Collections;
using UnityEngine;

public class ArmaPortales : MonoBehaviour
{
    public GameObject PortalA;
    public GameObject PortalB;
    public float rayDistance;
    public float DistanciaPortal;
    public float SepararX;

    public LayerMask Objeto;
    public LayerMask MuroPortal;

    public Animator _animatorA;
    public Animator _animatorB;
    public Animator _animatorArma;
    public float cadencia = 0.5f;
    float ultimotiro;

    [Header("Sonidos de FMOD")]
    public FMODUnity.EventReference sonidoPortalAzul;
    public FMODUnity.EventReference sonidoPortalNaranja;

    void Update()
    {
        // --- PORTAL NARANJA (CLIC DERECHO) ---
        RaycastHit hitPared;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(1))
        {
            if (Time.time > (cadencia + ultimotiro))
            {
                ultimotiro = Time.time;
                Portal(PortalA, hitPared, _animatorA);
                FMODUnity.RuntimeManager.PlayOneShot(sonidoPortalNaranja, transform.position);
            }
        }

        // --- PORTAL AZUL (CLIC IZQUIERDO) ---
        RaycastHit hitPared2;
        if (Physics.Raycast(transform.position, transform.forward, out hitPared2, DistanciaPortal, MuroPortal) && Input.GetMouseButtonDown(0))
        {
            if (Time.time > (cadencia + ultimotiro))
            {
                ultimotiro = Time.time;
                Portal(PortalB, hitPared2, _animatorB);
                FMODUnity.RuntimeManager.PlayOneShot(sonidoPortalAzul, transform.position);
            }
        }
    }

    // Le pasamos el RaycastHit completo para tener la info de la pared
    void Portal(GameObject portal, RaycastHit hit, Animator animator)
    {
        _animatorArma.SetTrigger("DisparoPortal");
        Vector3 posCalculada = hit.point + (hit.normal * SepararX);
        Quaternion rotCalculada = Quaternion.LookRotation(hit.normal);

        // Desvinculamos el portal de su pared anterior al instante para evitar tirones
        portal.transform.SetParent(null);

        StartCoroutine(SecuenciaTeleportPortal(posCalculada, rotCalculada, hit.transform, portal, animator));
    }

    IEnumerator SecuenciaTeleportPortal(Vector3 nuevaPos, Quaternion nuevaRot, Transform nuevaPared, GameObject portal, Animator _animator)
    {
        _animator.SetBool("Alterar", true);

        yield return new WaitForSeconds(0.25f);

        portal.transform.position = nuevaPos;
        portal.transform.rotation = nuevaRot;

        // LA MAGIA ESTÁ AQUÍ: Hacemos que el portal sea hijo de la pared.
        // El "true" final le dice a Unity que mantenga la posición y rotación que le acabamos de dar.
        portal.transform.SetParent(nuevaPared, true);

        _animator.SetBool("Alterar", false);
    }
}