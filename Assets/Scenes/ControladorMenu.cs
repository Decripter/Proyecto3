using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public class ControladorMenu : MonoBehaviour
{
    [Header("UI General")]
    public GameObject canvasMenu;
    public GameObject canvasCreditos; // ◄ NUEVO: Arrastra aquí el panel de créditos

    [Header("Configuracion de Teletransporte")]
    public GameObject robert;
    public Transform puntoInicio;
    public float alturaCamara = 1f;

    [Header("UI Cinematica Nueva")]
    public GameObject fondoNegroCinematica;
    public GameObject textoNarrador;

    [Header("Scripts a Desactivar de Robert")]
    public MonoBehaviour movimientoJugador;
    public mirada scriptMirada;
    public MonoBehaviour armaJugador;

    [Header("NUEVO: Control Total de HUD, Miras y Armas")]
    public GameObject objetoMira;
    public GameObject managerUI;
    public GameObject armaVisual;
    [Space]

    [Header("NUEVO: Control del Timeline")]
    public PlayableDirector directorTimeline;
    public GameObject camaraCinematica;

    [Header("NUEVO FMOD: Audio del Narrador")]
    public FMODUnity.EventReference eventoNarrador;
    private FMOD.Studio.EventInstance narradorInstance;

    private Coroutine secuenciaCoroutine;
    private bool enCinematica = false;

    void Start()
    {
        // 1. Aseguramos el menú activo y los créditos apagados al empezar
        if (canvasMenu != null) canvasMenu.SetActive(true);
        if (canvasCreditos != null) canvasCreditos.SetActive(false); // ◄ NUEVO

        // 2. Apagamos el director para que no evalúe el fotograma 0
        if (directorTimeline != null) directorTimeline.enabled = false;
        if (camaraCinematica != null) camaraCinematica.SetActive(false);

        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);
        if (textoNarrador != null) textoNarrador.SetActive(false);

        // 3. Apagamos los sistemas de Robert para que no se mueva en el menú
        if (movimientoJugador != null) movimientoJugador.enabled = false;
        if (scriptMirada != null) scriptMirada.enabled = false;
        if (armaJugador != null) armaJugador.enabled = false;

        // 4. APAGAMOS TODO LO VISUAL Y LOGICO DEL JUEGO EN EL MENÚ
        if (objetoMira != null) objetoMira.SetActive(false);
        if (managerUI != null) managerUI.SetActive(false);
        if (armaVisual != null) armaVisual.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        enCinematica = false;
    }

    void Update()
    {
        // Modificado para que el ratón se quede libre si cualquiera de los dos menús está abierto
        bool menuAbierto = (canvasMenu != null && canvasMenu.activeSelf) || (canvasCreditos != null && canvasCreditos.activeSelf);
        if (menuAbierto)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (enCinematica && Input.GetKeyDown(KeyCode.F))
        {
            SaltarCinematica();
        }
    }

    public void PulsarPlay()
    {
        secuenciaCoroutine = StartCoroutine(SecuenciaCinematica());
    }

    // ====================================================================
    // NUEVOS MÉTODOS PARA LOS CRÉDITOS Y SALIDA
    // ====================================================================
    public void PulsarCreditos() // ◄ NUEVO
    {
        if (canvasMenu != null) canvasMenu.SetActive(false);       // Apaga el menú principal
        if (canvasCreditos != null) canvasCreditos.SetActive(true); // Enciende los créditos
    }

    public void PulsarVolverCreditos() // ◄ NUEVO
    {
        if (canvasCreditos != null) canvasCreditos.SetActive(false); // Apaga los créditos
        if (canvasMenu != null) canvasMenu.SetActive(true);          // Vuelve a encender el menú principal
    }

    public void PulsarExit() // ◄ NUEVO: Cierra la aplicación
    {
        // Cierra el ejecutable del juego compilado
        Application.Quit();

        // Detiene el modo de prueba si estás dentro del editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    // ====================================================================

    IEnumerator SecuenciaCinematica()
    {
        enCinematica = true;

        if (!eventoNarrador.IsNull)
        {
            narradorInstance = FMODUnity.RuntimeManager.CreateInstance(eventoNarrador);
            narradorInstance.start();
        }

        if (canvasMenu != null) canvasMenu.SetActive(false);
        if (canvasCreditos != null) canvasCreditos.SetActive(false); // Por seguridad, nos aseguramos de que estén apagados

        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(true);
        if (textoNarrador != null) textoNarrador.SetActive(true);

        yield return new WaitForSeconds(6.0f);

        if (textoNarrador != null) textoNarrador.SetActive(false);
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);

        if (camaraCinematica != null) camaraCinematica.SetActive(true);

        if (directorTimeline != null)
        {
            directorTimeline.enabled = true;
            directorTimeline.Play();

            while (directorTimeline.state == UnityEngine.Playables.PlayState.Playing)
            {
                yield return null;
            }
        }

        FinalizarYActivarJuego();
    }

    void SaltarCinematica()
    {
        if (secuenciaCoroutine != null)
        {
            StopCoroutine(secuenciaCoroutine);
        }

        if (directorTimeline != null)
        {
            directorTimeline.Stop();
        }

        narradorInstance.stop((FMOD.Studio.STOP_MODE)0);
        narradorInstance.release();

        FinalizarYActivarJuego();
    }

    void FinalizarYActivarJuego()
    {
        enCinematica = false;
        narradorInstance.release();

        if (textoNarrador != null) textoNarrador.SetActive(false);
        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);
        if (camaraCinematica != null) camaraCinematica.SetActive(false);

        if (robert != null && puntoInicio != null)
        {
            CharacterController cc = robert.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            robert.transform.position = puntoInicio.position;
            robert.transform.rotation = puntoInicio.rotation;

            if (scriptMirada != null)
            {
                scriptMirada.transform.localPosition = new Vector3(0f, alturaCamara, 0f);
                scriptMirada.transform.localRotation = Quaternion.identity;
            }

            if (cc != null) cc.enabled = true;
        }

        if (movimientoJugador != null) movimientoJugador.enabled = true;
        if (scriptMirada != null) scriptMirada.enabled = true;
        if (armaJugador != null) armaJugador.enabled = true;

        if (objetoMira != null) objetoMira.SetActive(true);
        if (managerUI != null) managerUI.SetActive(true);
        if (armaVisual != null) armaVisual.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}