using UnityEngine;
using System.Collections; // <--- ¡Asegúrate de tener esta línea añadida!

public class ControladorMenu : MonoBehaviour
{
    [Header("UI General")]
    public GameObject canvasMenu;

    [Header("Configuracion de Teletransporte")]
    public GameObject robert;          // El objeto de Robert entero
    public Transform puntoInicio;      // El objeto 'PuntoInicioJuego' que creamos en el mapa

    [Header("UI Cinematica Nueva")]
    public GameObject fondoNegroCinematica; // El fondo negro que creamos
    public GameObject textoNarrador;        // El texto de la primera frase

    [Header("Scripts a Desactivar de Robert")]
    public MonoBehaviour movimientoJugador;
    public mirada scriptMirada;        // Enlazamos directamente tu script de mirada
    public MonoBehaviour armaJugador;

    void Start()
    {
        canvasMenu.SetActive(true);

        // Nos aseguramos de que la cinemática esté apagada al arrancar el juego
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);
        if (textoNarrador != null) textoNarrador.SetActive(false);

        // Al empezar, apagamos por completo los sistemas de Robert
        if (movimientoJugador != null) movimientoJugador.enabled = false;
        if (scriptMirada != null) scriptMirada.enabled = false;
        if (armaJugador != null) armaJugador.enabled = false;

        // Forzamos al ratón a estar libre para el menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Bloqueo de seguridad: Mientras el menú esté activo, el ratón se queda libre
        if (canvasMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void PulsarPlay()
    {
        StartCoroutine(SecuenciaCinematica());
    }

    // Esta funcion controla el orden y el tiempo de la historia paso a paso
    IEnumerator SecuenciaCinematica()
    {
        // 1. Apagamos el menú de la televisión
        canvasMenu.SetActive(false);

        // 2. Encendemos el fondo negro y la primera frase
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(true);
        if (textoNarrador != null) textoNarrador.SetActive(true);

        // 3. ¡ESPERA EN NEGRO! El juego se para aquí durante 6 segundos
        yield return new WaitForSeconds(6.0f);

        // 4. Termina la intro en negro: Apagamos el texto para pasar a la oficina
        if (textoNarrador != null) textoNarrador.SetActive(false);

        // 5. Teletransportamos a Robert a su sitio (el código que ya tenías)
        if (robert != null && puntoInicio != null)
        {
            CharacterController cc = robert.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            robert.transform.position = puntoInicio.position;
            robert.transform.rotation = puntoInicio.rotation;

            if (cc != null) cc.enabled = true;
        }

        // 6. Quitamos el fondo negro para ver el mapa
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);

        // 7. Reactivamos los controles que tenías antes
        if (movimientoJugador != null) movimientoJugador.enabled = true;
        if (scriptMirada != null) scriptMirada.enabled = true;
        if (armaJugador != null) armaJugador.enabled = true;

        // 8. Escondemos el ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}