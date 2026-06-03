using UnityEngine;

public class ArmaRecogible : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Referencias a inyectar")]
    public Gun_Manager managerJugador;

    [Header("Datos de este Modo")]
    public GameObject objetoModoAsociado;
    public Material matPantalla;
    public Material matArma;
    public Sprite iconoMira;



    // Versión para si interactúas con raycast directo
    public void Interactuar(mirada jugador)
    {
        Recoger();
    }

    // Versión sin parámetros (por el contrato de tu interfaz)
    public void Interactuar()
    {
        Recoger();
    }

    private void Recoger()
    {


        // Le mandamos todo el pack de datos al manager
        managerJugador.AgregarNuevoModo(objetoModoAsociado, matPantalla, matArma, iconoMira);

        // Destruimos el objeto 3D del nivel
        Destroy(gameObject);
    }
}
