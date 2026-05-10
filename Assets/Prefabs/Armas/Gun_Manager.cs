using UnityEngine;

public class Gun_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject[] Modos;
    public Color[] Colores;
    private Renderer _Renderer;
    private int indiceActual = 0;

    void Start()
    {
        _Renderer = transform.GetComponent<Renderer>();
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

        // 2. Cambio con teclas numéricas (1, 2, 3...)
        if (Input.GetKeyDown(KeyCode.Alpha1)) { indiceActual = 0; SeleccionarModo(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Modos.Length > 1) { indiceActual = 1; SeleccionarModo(); }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Modos.Length > 2) { indiceActual = 2; SeleccionarModo(); }
    }

    void SeleccionarModo()
    {
        for (int i = 0; i < Modos.Length; i++)
        {
            // Solo activamos el que coincide con el índice, el resto se apaga

            Modos[i].SetActive(i == indiceActual);
        }
        _Renderer.material.color = Colores[indiceActual];
        Debug.Log("Modo actual: " + Modos[indiceActual].name);
    }
}
