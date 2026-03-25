using UnityEngine;

public class activar_anim : MonoBehaviour
{
    public GameObject Puerta;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Activador"))
        {
        animator.SetBool("Abrir", true);            
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Activador"))
        {
        animator.SetBool("Abrir", false);
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
