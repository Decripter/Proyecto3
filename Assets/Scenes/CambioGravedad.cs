using System.Collections;
using UnityEngine;

public class CambioGravedad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool Invertir;
    private Rigidbody rb;

    [SerializeField] public float Valor;
    
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
        // 1. Buscamos TODOS los renderers, tanto en el padre como en los hijos
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // Si el array está vacío (no hay mallas), cortamos
        if (renderers.Length == 0) yield break;

        // 2. Creamos "cajas" para guardar los materiales y colores de cada pieza
        Material[] materiales = new Material[renderers.Length];
        Color[] coloresOriginales = new Color[renderers.Length];

        // Rellenamos las cajas leyendo el estado inicial de cada pieza
        for (int i = 0; i < renderers.Length; i++)
        {
            materiales[i] = renderers[i].material; // Crea la instancia única
            coloresOriginales[i] = materiales[i].GetColor("_BaseColor");
        }

        Color colorPulso = Color.purple;
        float duracion = 0.25f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            float intensidad = Mathf.Sin(t * Mathf.PI);

            // 3. Aplicamos el color modificado a TODOS los materiales guardados
            for (int i = 0; i < materiales.Length; i++)
            {
                materiales[i].SetColor("_BaseColor", Color.Lerp(coloresOriginales[i], colorPulso, intensidad));
                materiales[i].SetColor("_EmissionColor", colorPulso * intensidad * 0.5f);
                materiales[i].EnableKeyword("_EMISSION");
            }

            yield return null;
        }

        // 4. Restauramos cada pieza a su color original correspondiente
        for (int i = 0; i < materiales.Length; i++)
        {
            materiales[i].SetColor("_BaseColor", coloresOriginales[i]);
            materiales[i].SetColor("_EmissionColor", Color.black);
        }
    }
}
