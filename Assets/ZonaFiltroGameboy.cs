using System.Collections;
using UnityEngine;

public class ZonaFiltroGameboy : MonoBehaviour
{
    [Header("Arrastra el Material del Filtro aquí")]
    public Material materialFiltro;

    [Header("Configuración de la Zona")]
    [Range(0f, 1f)]
    public float intensidadMaxima = 1f; // 1 = full Gameboy, 0.5 = mezcla
    public float tiempoTransicion = 1.5f; // Segundos que tarda en aparecer/desaparecer

    private Coroutine transicionActual;

    void Start()
    {
        // SEGURO DE VIDA 1: Apagar el filtro siempre al iniciar la escena
        if (materialFiltro != null)
        {
            materialFiltro.SetFloat("_Transparencia", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que sea el jugador el que entra
        if (other.CompareTag("Player"))
        {
            if (transicionActual != null) StopCoroutine(transicionActual);
            transicionActual = StartCoroutine(AnimarFiltro(intensidadMaxima));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Al salir, lo devolvemos a 0 suavemente
        if (other.CompareTag("Player"))
        {
            if (transicionActual != null) StopCoroutine(transicionActual);
            transicionActual = StartCoroutine(AnimarFiltro(0f));
        }
    }

    private IEnumerator AnimarFiltro(float valorObjetivo)
    {
        // Leemos cómo está el filtro en este frame exacto por si la corrutina se cortó a medias
        float valorInicial = materialFiltro.GetFloat("_Transparencia");
        float tiempo = 0f;

        while (tiempo < tiempoTransicion)
        {
            tiempo += Time.deltaTime;

            // Lerp matemático para suavizar la entrada/salida
            float lerp = Mathf.Lerp(valorInicial, valorObjetivo, tiempo / tiempoTransicion);
            materialFiltro.SetFloat("_Transparencia", lerp);

            yield return null;
        }

        // Aseguramos el valor final matemático para evitar decimales residuales
        materialFiltro.SetFloat("_Transparencia", valorObjetivo);
    }

    void OnDisable()
    {
        // SEGURO DE VIDA 2: Si el objeto se destruye o cambias de escena, el filtro se apaga
        if (materialFiltro != null)
        {
            materialFiltro.SetFloat("_Transparencia", 0f);
        }
    }
}