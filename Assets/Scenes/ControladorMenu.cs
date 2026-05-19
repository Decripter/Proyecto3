using UnityEngine;

public class ControladorMenu : MonoBehaviour
{
    [Header("UI General")]
    public GameObject canvasMenu;

    [Header("Configuracion de Teletransporte")]
    public GameObject robert;          // El objeto de Robert entero
    public Transform puntoInicio;      // El objeto 'PuntoInicioJuego' que creamos en el mapa

    [Header("Scripts a Desactivar de Robert")]
    public MonoBehaviour movimientoJugador;
    public mirada scriptMirada;        // Enlazamos directamente tu script de mirada
    public MonoBehaviour armaJugador;

    void Start()
    {
        canvasMenu.SetActive(true);

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
        // 1. Teletransportamos a Robert a la sala de inicio antes de encender nada
        if (robert != null && puntoInicio != null)
        {
            // Si Robert usa un CharacterController, lo apagamos un milisegundo para evitar conflictos de física
            CharacterController cc = robert.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            robert.transform.position = puntoInicio.position;
            robert.transform.rotation = puntoInicio.rotation;

            if (cc != null) cc.enabled = true;
        }

        // 2. Apagamos de golpe el menú de la tele
        canvasMenu.SetActive(false);

        // 3. Reactivamos todos los controles para que empiece la acción
        if (movimientoJugador != null) movimientoJugador.enabled = true;
        if (scriptMirada != null) scriptMirada.enabled = true;
        if (armaJugador != null) armaJugador.enabled = true;

        // 4. Escondemos el ratón para que el script 'mirada' tome el control del apuntado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}