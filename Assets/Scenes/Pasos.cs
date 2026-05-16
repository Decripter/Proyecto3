using UnityEngine;

public class Pasos : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public FMODUnity.EventReference sonidoPasos;

    private FMOD.Studio.EventInstance instanciaPasos;
    private GroundChecker groundChecker;

    void Start()
    {
        // Buscamos el GroundChecker automáticamente en los hijos de Robert
        groundChecker = GetComponentInChildren<GroundChecker>();

        // Preparamos la instancia de FMOD
        if (!sonidoPasos.IsNull)
        {
            instanciaPasos = FMODUnity.RuntimeManager.CreateInstance(sonidoPasos);
        }
    }

    void Update()
    {
        if (sonidoPasos.IsNull) return;

        // COMPROBACIÓN DIRECTA DE TECLAS (Ignoramos el CharacterController mentiroso)
        // Detecta tanto minúsculas como mayúsculas, y las flechas del teclado por si acaso
        bool pulsandoTeclas = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                              Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                              Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                              Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        // Comprobamos si tu script GroundChecker dice que estás tocando el suelo
        bool estaEnElSuelo = groundChecker != null ? groundChecker.Tocando : true;

        // Preguntamos a FMOD si el sonido ya está reproduciéndose
        FMOD.Studio.PLAYBACK_STATE estado;
        instanciaPasos.getPlaybackState(out estado);
        bool estaSonando = estado == FMOD.Studio.PLAYBACK_STATE.PLAYING;

        // Si pulsas dirección Y estás pisando el suelo... ¡A CAMINAR!
        if (pulsandoTeclas && estaEnElSuelo)
        {
            if (!estaSonando)
            {
                instanciaPasos.start();
            }
        }
        else
        {
            // Si sueltas las teclas o saltas, el sonido se detiene suavemente
            if (estaSonando)
            {
                instanciaPasos.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        // Pegamos el sonido al jugador para que se mueva con él en el espacio 3D
        instanciaPasos.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    void OnDestroy()
    {
        // Limpieza obligatoria de memoria al cerrar el juego
        FMOD.Studio.PLAYBACK_STATE estado;
        instanciaPasos.getPlaybackState(out estado);
        if (estado == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            instanciaPasos.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        instanciaPasos.release();
    }
}