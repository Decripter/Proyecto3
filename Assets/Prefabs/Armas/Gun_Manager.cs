using UnityEngine;

public class Gun_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject[] Modos;
    public Color[] Colores;
    public Material[] Pantalla;
    public Material[] Arma;

    [Header("Cositas Arma")]
    public Renderer[] _RendererArma;
    public Renderer _RendererPantalla;

    private int indiceActual = 0;

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
            if (indiceActual >= Modos.Length) indiceActual = 0;
            SeleccionarModo();
        }
        else if (scroll < 0f)
        {
            indiceActual--;
            if (indiceActual < 0) indiceActual = Modos.Length - 1;
            SeleccionarModo();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { indiceActual = 0; SeleccionarModo(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Modos.Length > 1) { indiceActual = 1; SeleccionarModo(); }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Modos.Length > 2) { indiceActual = 2; SeleccionarModo(); }
    }

    void SeleccionarModo()
    {
        for (int i = 0; i < Modos.Length; i++)
        {
            Modos[i].SetActive(i == indiceActual);
        }
        //_Renderer.material.color = Colores[indiceActual];
        _RendererPantalla.material = Pantalla[indiceActual];

        foreach (Renderer rend in _RendererArma)
        {
            rend.material = Arma[indiceActual];
        }
        Debug.Log("Modo actual: " + Modos[indiceActual].name);
    }

    /*
     
             foreach (Renderer rend in renderersDelArma)
        {
            rend.material = materialObjetivo;
        }
     
     */
}
