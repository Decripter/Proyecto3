using UnityEngine;

public class activar_anim : MonoBehaviour
{
    public GameObject Puerta;
    public Animator animator;

    [Header("Sonidos de FMOD")]
    public FMODUnity.EventReference sonidoAcierto;   // El "chinchín" de nivel completado
    public FMODUnity.EventReference sonidoPuerta;    // El sonido de la puerta moviéndose

    private bool yaHaSonadoAcierto = false; // Evita que el sonido de victoria se repita si quitas y pones el cubo

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Activador"))
        {
        animator.SetBool("Abrir", true);

            // --- NUEVO: Sonidos al Abrir ---

            // 1. Sonido de acierto (solo suena la primera vez que se resuelve)
            if (!yaHaSonadoAcierto && !sonidoAcierto.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(sonidoAcierto, transform.position);
                yaHaSonadoAcierto = true;
            }

            // 2. Sonido de la puerta abriéndose
            if (!sonidoPuerta.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(sonidoPuerta, Puerta.transform.position);
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Activador"))
        {
            animator.SetBool("Abrir", false);
 
            // --- NUEVO: Sonido al Cerrar ---
            // Si el cubo se quita, la puerta se cierra y vuelve a sonar el movimiento de la puerta
            if (!sonidoPuerta.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(sonidoPuerta, Puerta.transform.position);
            }
        }


    }

    /*
    private void OnTriggerStay(Collider other)
    {
        animator.SetTrigger("activar");
        Debug.Log("xxx");
    }*/
    void Awake()
    {
        animator = Puerta.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
