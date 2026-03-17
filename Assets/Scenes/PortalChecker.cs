using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PortalChecker : MonoBehaviour
{
    public bool EnZonaDePortal;
    public bool Manual;
    public LayerMask capaPortales;

    public Vector3 Box;


    void Update()
    {
        EnZonaDePortal = Physics.CheckBox(transform.position, Box, Quaternion.identity, capaPortales);

        if (EnZonaDePortal)
        {
            Physics.IgnoreLayerCollision(3, 8, true);
        }
        else
        {
            Physics.IgnoreLayerCollision(3, 8, false);
        }

        /*
        if (Manual)
        {
            Physics.IgnoreLayerCollision(3, 8, true);
        }
        else
        {
            Physics.IgnoreLayerCollision(3, 8, false);
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, Box);
    }
}
