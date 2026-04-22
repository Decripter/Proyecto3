using System.Collections;
using UnityEngine;

public class CambioGravedad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool Invertir;
    private Rigidbody rb;

    [SerializeField] private float Valor;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Valor = -9.8f;
        Invertir = true;
    }

    // Update is called once per frame
    void Update()
    {
        Alterar(Valor);
    }

    public float GetValor()
    {
        return Valor;
    }

    public void Alterar(float NuevoValor)
    {
        Valor = NuevoValor;
    }
    void FixedUpdate()
    {
        // Desactivamos la gravedad normal y aplicamos una hacia arriba de manera constante
        if (!Invertir)
        {
            rb.useGravity = false;
            rb.AddForce(Vector3.up * Valor, ForceMode.Acceleration);
        }

        else
        {
            rb.useGravity = true;
        }
    }


    public IEnumerator EfectoInversion()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) yield break;

        Material mat = rend.material; // Crea una instancia única para este objeto
        Color colorOriginal = mat.GetColor("_BaseColor"); // O "_Color" según tu shader
        Color colorPulso = Color.cyan; // Color del pulso de gravedad

        float duracion = 0.25f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            // Curva de "pico": sube y baja rápido (PingPong o Sinusoidal)
            float intensidad = Mathf.Sin(t * Mathf.PI);

            // Mezclamos el color original con el de pulso y subimos la emisión
            mat.SetColor("_BaseColor", Color.Lerp(colorOriginal, colorPulso, intensidad));
            mat.SetColor("_EmissionColor", colorPulso * intensidad * 0.5f); // Multiplicador de brillo
            mat.EnableKeyword("_EMISSION");

            yield return null;
        }

        // Resetear al estado original
        mat.SetColor("_BaseColor", colorOriginal);
        mat.SetColor("_EmissionColor", Color.black);
    }
}
