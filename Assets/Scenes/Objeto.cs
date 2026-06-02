using UnityEngine;

public class Objeto : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interactuar(mirada jugador)
    {
        // Si el jugador no tiene las manos llenas, le decimos que nos agarre
        if (!jugador.Ocupado)
        {
            jugador.AgarrarObjeto(rb);
        }
    }
    public void Interactuar()
    {

    }
}
