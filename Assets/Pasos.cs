using UnityEngine;

public class Pasos : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public FMODUnity.EventReference sonidoPasos;

    // Aquí guardamos la "cinta de casete" para poder darle a Play y Stop
    private FMOD.Studio.EventInstance instanciaPasos;

    void Start()
    {
        // Al empezar, preparamos el reproductor con tu sonido
        instanciaPasos = FMODUnity.RuntimeManager.CreateInstance(sonidoPasos);
    }

    void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        // Comprobamos si hay movimiento
        bool seEstaMoviendo = Mathf.Abs(movX) > 0.1f || Mathf.Abs(movZ) > 0.1f;

        // Preguntamos a FMOD: "¿Está sonando la cinta ahora mismo?"
        FMOD.Studio.PLAYBACK_STATE estado;
        instanciaPasos.getPlaybackState(out estado);
        bool estaSonando = estado == FMOD.Studio.PLAYBACK_STATE.PLAYING;

        if (seEstaMoviendo)
        {
            // Si nos movemos y el sonido estaba parado, le damos al PLAY
            if (!estaSonando)
            {
                instanciaPasos.start();
            }
        }
        else
        {
            // Si nos paramos y el sonido seguía sonando, le damos al STOP
            if (estaSonando)
            {
                // ALLOWFADEOUT hace que el corte de sonido sea un poco más suave
                instanciaPasos.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            instanciaPasos.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    void OnDestroy()
    {
        // Es muy importante limpiar la memoria cuando cerramos el juego
        // o si el personaje es destruido.
        instanciaPasos.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instanciaPasos.release();
    }
}