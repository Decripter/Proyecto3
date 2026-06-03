using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public List<GameObject> Modos = new List<GameObject>();
    public List<Material> Pantalla = new List<Material>();
    public List<Material> Arma = new List<Material>();
    public List<Sprite> Miras = new List<Sprite>();

    public Image Centro;
    public Sprite MiraDefault;
    [Header("Cositas Arma")]
    public Renderer[] _RendererArma;
    public Renderer _RendererPantalla;

    public Animator ArmaFull;
    public Animator ArmaAcciones;
    public CharacterController jugador;

    private int indiceActual = 0;

    [Header("NUEVO FMOD: Sonido")]
    public FMODUnity.EventReference sonidoRecoger; // ◄ NUEVO: Arrastra aquí el evento de FMOD
    void Start()
    {
        SeleccionarModo(); // Inicializamos con el primero activo
        
    }

    void Update()
    {
        // 1. Cambio con la rueda del ratón (Scroll)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            indiceActual++;
            if (indiceActual >= Modos.Count) indiceActual = 0;
            SeleccionarModo();
        }
        else if (scroll < 0f)
        {
            indiceActual--;
            if (indiceActual < 0) indiceActual = Modos.Count - 1;
            SeleccionarModo();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            indiceActual = 0; SeleccionarModo(); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Modos.Count > 1)
        { 
            indiceActual = 1; SeleccionarModo(); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Modos.Count > 2) 
        { indiceActual = 2; SeleccionarModo(); 
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            ArmaAcciones.SetTrigger("Inspeccionar");
        }

        /*Vector3 velocidadPlana = new Vector3(jugador.velocity.x, 0, jugador.velocity.z);
        armaAnimator.SetFloat("Velocidad", velocidadPlana.magnitude);*/

        float movimiento = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));

        // Limitamos el máximo a 1 para que el Animator no reciba números raros si pulsas dos teclas a la vez
        float velocidadPlana = Mathf.Clamp01(movimiento);

        ArmaFull.SetFloat("Velocidad", velocidadPlana);

    }

    void SeleccionarModo()
    {
        ActualizarVisuales();
        ArmaAcciones.SetTrigger("CambioModo");
    }

    void ActualizarVisuales()
    {

        // 1. REPRODUCIR SONIDO CON FMOD (Antes de destruir el objeto)
        if (!sonidoRecoger.IsNull)
        {
            // Reproduce el sonido en la posición exacta en 3D donde estaba el arma
            FMODUnity.RuntimeManager.PlayOneShot(sonidoRecoger, transform.position);
        }
        if (Modos.Count == 0)
        {
            Centro.sprite = MiraDefault;

            // --- NUEVO: Apagamos los renderers si no hay armas ---
            if (_RendererPantalla != null) _RendererPantalla.enabled = false;
            foreach (Renderer rend in _RendererArma)
            {
                if (rend != null) rend.enabled = false;
            }
        }
        else
        {
            // --- NUEVO: Encendemos los renderers porque ya tenemos arma ---
            if (_RendererPantalla != null) _RendererPantalla.enabled = true;
            foreach (Renderer rend in _RendererArma)
            {
                if (rend != null) rend.enabled = true;
            }

            for (int i = 0; i < Modos.Count; i++)
            {
                Modos[i].SetActive(i == indiceActual);
            }

            Centro.sprite = Miras[indiceActual];
            _RendererPantalla.material = Pantalla[indiceActual];

            foreach (Renderer rend in _RendererArma)
            {
                rend.material = Arma[indiceActual];
            }

        }
    }



    /*
     
             foreach (Renderer rend in renderersDelArma)
        {
            rend.material = materialObjetivo;
        }
     
     */

    public void AgregarNuevoModo(GameObject nuevoObjetoModo, Material matPantalla, Material matArma, Sprite nuevaMira)
    {
        bool esPrimeraVez = Modos.Count == 0;

        // 1. Añadimos el nuevo contenido a nuestras listas dinámicas
        Modos.Add(nuevoObjetoModo);
        Pantalla.Add(matPantalla);
        Arma.Add(matArma);
        Miras.Add(nuevaMira);

        // 2. Nos saltamos directamente al último índice (el arma recién cogida)
        indiceActual = Modos.Count - 1;

        // 3. Control de Animaciones
        if (esPrimeraVez)
        {
            ArmaAcciones.SetTrigger("Recogida"); // ◄ Trigger para cuando consigues el arma entera
            ActualizarVisuales();
        }
        else
        {
            // Si ya tenías armas, usamos tu método normal que incluye el trigger "CambioModo"
            SeleccionarModo();

        }
    }
}
