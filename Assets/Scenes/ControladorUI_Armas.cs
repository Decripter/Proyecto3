using UnityEngine;
using UnityEngine.UI;

public class ControladorUI_Armas : MonoBehaviour
{
    [System.Serializable]
    public class InterfazArma
    {
        public string nombreArma;
        public GameObject objetoArmaEnMano;
        public Sprite spriteMira;
        public Color colorMira = Color.white;
    }

    [Header("Configuración de la UI")]
    public Image imagenMiraUI;

    [Header("Lista de Armas Estándar")]
    public InterfazArma armaNormal;
    public InterfazArma armaGravedad;

    [Header("Configuración Especial: Arma Portales")]
    public GameObject objetoArmaPortales;
    public GameObject portal1;
    public GameObject portal2;

    [Header("Las 4 Miras de los Portales")]
    public Sprite miraNingunPortal;
    public Sprite miraSoloPortalAzul;
    public Sprite miraSoloPortalNaranja;
    public Sprite miraAmbosPortales;

    void Update()
    {
        ActualizarInterfazArmas();
    }

    void ActualizarInterfazArmas()
    {
        if (imagenMiraUI == null) return;

        // 1. ¿LLEVAMOS EL ARMA DE PORTALES EN LA MANO?
        if (objetoArmaPortales != null && objetoArmaPortales.activeInHierarchy)
        {
            imagenMiraUI.color = Color.white;

            // Detección pasiva para el sistema de tu compañero
            bool p1Activo = ComprobarSiPortalEstaPuesto(portal1);
            bool p2Activo = ComprobarSiPortalEstaPuesto(portal2);

            if (!p1Activo && !p2Activo)
            {
                imagenMiraUI.sprite = miraNingunPortal;
            }
            else if (p1Activo && !p2Activo)
            {
                imagenMiraUI.sprite = miraSoloPortalAzul;
            }
            else if (!p1Activo && p2Activo)
            {
                imagenMiraUI.sprite = miraSoloPortalNaranja;
            }
            else if (p1Activo && p2Activo)
            {
                imagenMiraUI.sprite = miraAmbosPortales;
            }

            return;
        }

        // 2. ¿LLEVAMOS EL ARMA NORMAL?
        if (armaNormal.objetoArmaEnMano != null && armaNormal.objetoArmaEnMano.activeInHierarchy)
        {
            if (armaNormal.spriteMira != null) imagenMiraUI.sprite = armaNormal.spriteMira;
            imagenMiraUI.color = armaNormal.colorMira;
            return;
        }

        // 3. ¿LLEVAMOS EL ARMA DE GRAVEDAD?
        if (armaGravedad.objetoArmaEnMano != null && armaGravedad.objetoArmaEnMano.activeInHierarchy)
        {
            if (armaGravedad.spriteMira != null) imagenMiraUI.sprite = armaGravedad.spriteMira;
            imagenMiraUI.color = armaGravedad.colorMira;
            return;
        }
    }

    bool ComprobarSiPortalEstaPuesto(GameObject objetoPortal)
    {
        if (objetoPortal == null) return false;
        if (!objetoPortal.activeInHierarchy) return false;

        MeshRenderer malla = objetoPortal.GetComponentInChildren<MeshRenderer>();
        if (malla != null)
        {
            return malla.enabled;
        }

        Camera camaraPortal = objetoPortal.GetComponentInChildren<Camera>();
        if (camaraPortal != null)
        {
            return camaraPortal.enabled;
        }

        return true;
    }
}