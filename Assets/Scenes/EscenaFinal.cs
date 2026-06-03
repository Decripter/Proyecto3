using UnityEngine;
using System.Collections;

public class FinalJuego : MonoBehaviour
{
    [Header("UI Final")]
    public GameObject canvasFinal; // Arrastra aquí tu Canvas de "Juego Terminado"

    [Header("Sonido de FMOD")]
    public FMODUnity.EventReference sonidoFinal; // Tu evento de sonido de FMOD

    [Header("Configuración")]
    public float retrasoCanvas = 7.0f;  // ⏳ ¡NUEVO! Segundos que tarda en aparecer el Canvas
    public float tiempoDeEspera = 4.0f; // Segundos que se muestra el mensaje antes de cerrar
    public MonoBehaviour movimientoJugador; // Para congelar a Robert al ganar

    private bool juegoTerminado = false;

    private void OnTriggerEnter(Collider other)
    {
        // Detectamos si lo que ha cruzado la zona es Robert
        if ((other.CompareTag("Player") || other.name == "Robert") && !juegoTerminado)
        {
            juegoTerminado = true;
            StartCoroutine(SecuenciaFinDelJuego());
        }
    }

    IEnumerator SecuenciaFinDelJuego()
    {
        // 1. Paramos los pies a Robert inmediatamente para que no se salga del mapa en la espera
        if (movimientoJugador != null) movimientoJugador.enabled = false;

        // 2. ⏳ ¡EL TRUCO! El script se duerme aquí durante 7 segundos
        yield return new WaitForSeconds(retrasoCanvas);

        // 3. Pasados los 7 segundos, se reproduce el sonido...
        if (!sonidoFinal.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sonidoFinal, transform.position);
        }

        // 4. ...y aparece el Canvas de golpe cubriendo la pantalla
        if (canvasFinal != null) canvasFinal.SetActive(true);

        // 5. Dejamos el texto en pantalla durante unos segundos
        yield return new WaitForSeconds(tiempoDeEspera);

        // 6. Cerramos el juego
        Debug.Log("¡Juego Cerrado!");
        Application.Quit();

        // Para que funcione también dentro del editor de Unity:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}