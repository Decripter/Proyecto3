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

    public GameObject armaNormal;

    public GameObject armaGravedad;

    public GameObject armaPortales;



    [Header("NUEVO: Control del Timeline")]

    public PlayableDirector directorTimeline;

    public GameObject camaraCinematica;



    // VARIABLES INTERNAS PARA EL CONTROL DEL SKIP

    private Coroutine secuenciaCoroutine;

    private bool enCinematica = false;



    void Start()

    {

        // 1. Aseguramos el menú activo

        if (canvasMenu != null) canvasMenu.SetActive(true);



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



        if (armaNormal != null) armaNormal.SetActive(false);

        if (armaGravedad != null) armaGravedad.SetActive(false);

        if (armaPortales != null) armaPortales.SetActive(false);



        // Forzamos al ratón a estar libre para el menú

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;



        enCinematica = false;

    }



    void Update()

    {

        if (canvasMenu != null && canvasMenu.activeSelf)

        {

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

        }



        // ¡NUEVO! Si estamos en mitad de la cinemática y se presiona la F, saltamos todo

        if (enCinematica && Input.GetKeyDown(KeyCode.F))

        {

            SaltarCinematica();

        }

    }



    public void PulsarPlay()

    {

        // Guardamos la referencia de la corrutina para poder detenerla si se skipea

        secuenciaCoroutine = StartCoroutine(SecuenciaCinematica());

    }



    IEnumerator SecuenciaCinematica()

    {

        enCinematica = true;



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



        // Si la cinemática termina de manera natural, iniciamos el juego normalmente

        FinalizarYActivarJuego();

    }



    // ¡NUEVO MÉTODO! Se encarga de interrumpir todo de forma segura

    void SaltarCinematica()

    {

        // 1. Detenemos la corrutina para que no siga esperando o ejecutando pasos de fondo

        if (secuenciaCoroutine != null)

        {

            StopCoroutine(secuenciaCoroutine);

        }



        // 2. Detenemos el Timeline en seco si se estaba reproduciendo

        if (directorTimeline != null)

        {

            directorTimeline.Stop();

        }



        // 3. Forzamos el encendido del juego de inmediato

        FinalizarYActivarJuego();

    }



    // Aquí agrupamos toda la lógica de encendido del juego para no duplicar código

    void FinalizarYActivarJuego()

    {

        enCinematica = false;



        // Nos aseguramos de apagar de golpe todas las cámaras y elementos visuales de la intro

        if (textoNarrador != null) textoNarrador.SetActive(false);

        if (fondoNegroCinematica != null) fondoNegroCinematica.SetActive(false);

        if (camaraCinematica != null) camaraCinematica.SetActive(false);



        // ====================================================================

        // 5. Teletransportamos a Robert al inicio del JUEGO

        // ====================================================================

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



        // ====================================================================

        // 6. ENCENDEMOS EL JUEGO

        // ====================================================================

        if (movimientoJugador != null) movimientoJugador.enabled = true;

        if (scriptMirada != null) scriptMirada.enabled = true;

        if (armaJugador != null) armaJugador.enabled = true;



        // Encendemos las interfaces y el modelo del arma

        if (objetoMira != null) objetoMira.SetActive(true);

        if (managerUI != null) managerUI.SetActive(true);

        if (armaVisual != null) armaVisual.SetActive(true);



        // ACTIVAMOS SOLO EL ARMA NORMAL PARA EMPEZAR A JUGAR

        if (armaNormal != null) armaNormal.SetActive(true);



        // Nos aseguramos de que las otras dos sigan apagadas

        if (armaGravedad != null) armaGravedad.SetActive(false);

        if (armaPortales != null) armaPortales.SetActive(false);



        // 7. Escondemos el ratón

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

    }

}

