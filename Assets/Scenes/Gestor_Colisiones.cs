using UnityEngine;

public class Gestor_Colisiones : MonoBehaviour
{
    private int portalOverlaps = 0;
    public LayerMask paredPortalLayer;

    // Este método lo llamarás cuando el DotProduct diga que es hora de cruzar
    public void SetIgnoreCollisions(bool ignore)
    {
        // Usamos Physics.IgnoreLayerCollision para ser eficientes
        // El 3 es un ejemplo, usa los IDs de tus capas
        Physics.IgnoreLayerCollision(gameObject.layer, paredPortalLayer, ignore);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PortalTrigger"))
        {
            portalOverlaps++;
            ActualizarColisiones();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PortalTrigger"))
        {
            portalOverlaps--;
            ActualizarColisiones();
        }
    }

    void ActualizarColisiones()
    {
        if (portalOverlaps > 0)
            SetIgnoreCollisions(true);
        else
            SetIgnoreCollisions(false);
    }
}
