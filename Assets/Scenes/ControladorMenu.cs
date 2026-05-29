using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public class ControladorMenu : MonoBehaviour
{
    [Header("UI General")]
    public GameObject canvasMenu;

    [Header("Configuracion de Teletransporte")]
    public GameObject robert;
    public Transform puntoInicio;
    public float alturaCamara = 0f;

    [Header("UI Cinematica Nueva")]
    public GameObject fondoNegroCinematica;
    public GameObject textoNarrador;

    [Header("Scripts a Desactivar de Robert")]
    public MonoBehaviour movimientoJugador;
    public mirada scriptMirada;
    public MonoBehaviour armaJugador;

    [Header("NUEVO: Control del Timeline")]
    public PlayableDirector directorTimeline;
    public GameObject camaraCinematica;

    void Start()
    {
        // 1. Aseguramos el menú activo
        if (canvasMenu != null) canvasMenu.SetActive(true);

        // 2. TRUCO ANTSECUESTRO: Apagamos el componente del director para que no evalúe el fotograma 0
        if (directorTimeline != null) directorTimeline.enabled = false;
        if (camaraCinematica != null) camaraCinematica.SetActive(false);

        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);
        if (textoNarrador != null) textoNarrador.SetActive(false);

        // 3. Apagamos los sistemas de Robert para que no se mueva en el menú
        if (movimientoJugador != null) movimientoJugador.enabled = false;
        if (scriptMirada != null) scriptMirada.enabled = false;
        if (armaJugador != null) armaJugador.enabled = false;

        // Forzamos al ratón a estar libre para el menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (canvasMenu != null && canvasMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void PulsarPlay()
    {
        StartCoroutine(SecuenciaCinematica());
    }

    IEnumerator SecuenciaCinematica()
    {
        // 1. Apagamos el menú de la televisión
        if (canvasMenu != null) canvasMenu.SetActive(false);

        // 2. Encendemos el fondo negro y la primera frase
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(true);
        if (textoNarrador != null) textoNarrador.SetActive(true);

        // 3. ESPERA EN NEGRO (6 segundos)
        yield return new WaitForSeconds(6.0f);

        // 4. Termina la intro en negro
        if (textoNarrador != null) textoNarrador.SetActive(false);
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);

        // ====================================================================
        // REPRODUCIR LA CINEMÁTICA
        // ====================================================================

        // Activamos la cámara de la animación
        if (camaraCinematica != null) camaraCinematica.SetActive(true);

        if (directorTimeline != null)
        {
            directorTimeline.enabled = true; // <--- ¡NUEVO! Despertamos al director aquí
            directorTimeline.Play();

            // CAMBIO AQUÍ: En vez de WaitForSeconds, usamos este bucle inteligente
            while (directorTimeline.state == UnityEngine.Playables.PlayState.Playing)
            {
                yield return null; // Espera al siguiente fotograma y vuelve a comprobar
            }
        }

        // Al terminar la animación, apagamos la cámara cinemática
        if (camaraCinematica != null) camaraCinematica.SetActive(false);

        // ====================================================================

        // 5. Teletransportamos a Robert a su sitio de inicio de JUEGO
        if (robert != null && puntoInicio != null)
        {
            CharacterController cc = robert.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            robert.transform.position = puntoInicio.position;
            robert.transform.rotation = puntoInicio.rotation;

            // Como 'scriptMirada' está pegado a la Main_Camara, reseteamos su posición local
            if (scriptMirada != null)
            {
                scriptMirada.transform.localPosition = new Vector3(0f, alturaCamara, 0f);
                scriptMirada.transform.localRotation = Quaternion.identity;
            }

            if (cc != null) cc.enabled = true;
        }

        // 6. Reactivamos los controles de Robert para que empiece a jugar
        if (movimientoJugador != null) movimientoJugador.enabled = true;
        if (scriptMirada != null) scriptMirada.enabled = true;
        if (armaJugador != null) armaJugador.enabled = true;

        // 7. Escondemos el ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}