using UnityEngine;

public class ZonaReset : MonoBehaviour
{
    [Header("Referencia al Script de Reset")]
    public ReiniciarHabitacion habitacionAResetear;

    // Con esto nos aseguramos de que solo se resetee una vez al entrar
    private bool yaResetado = false;

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que entra es el jugador (etiqueta "Player")
        if (other.CompareTag("Player") && !yaResetado)
        {
            if (habitacionAResetear != null)
            {
                habitacionAResetear.EjecutarReset();
                yaResetado = true; // Marcamos como resetado
                Debug.Log("Habitación reseteada por entrada en zona");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Al salir de la zona, permitimos que se vuelva a resetear en el futuro si vuelve a entrar
        if (other.CompareTag("Player"))
        {
            yaResetado = false;
        }
    }
}
