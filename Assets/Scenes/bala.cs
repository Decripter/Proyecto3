using UnityEngine;
using System.Collections;

public class bala : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;

    public float tiempoVida = 3f;
    private Renderer rend;
    public void lanzar(float speed)
    {
    }
    
    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        rend = GetComponent<Renderer>();
        StartCoroutine(DesvanecerYDestruir());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DesvanecerYDestruir()
    {
        // Esperamos los 3 segundos de vida útil
        yield return new WaitForSeconds(tiempoVida);

        float duracionBorrado = 0.5f;
        float tiempo = 0f;
        Color colorInicial = rend.material.color;

        // Importante: El material debe tener el Surface Type en "Transparent" 
        // para que el canal Alpha funcione.
        while (tiempo < duracionBorrado)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, tiempo / duracionBorrado);
            rend.material.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
