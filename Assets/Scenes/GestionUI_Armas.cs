using UnityEngine;
using UnityEngine.UI;

public class GestionUI_Armas : MonoBehaviour
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
    public GameObject objetoArmaPortales; // El objeto "ArmaPortales" de la jerarquía

    [Tooltip("Arrastra aquí los objetos Portal1 y Portal2 de la jerarquía para saber si existen")]
    public GameObject portal1;
    public GameObject portal2;

    [Header("Las 4 Miras de los Portales")]
    public Sprite miraNingunPortal;    // Estado 0: Ninguno en el mapa
    public Sprite miraSoloPortalAzul;   // Estado 1: Solo el izquierdo/azul
    public Sprite miraSoloPortalNaranja;// Estado 2: Solo el derecho/naranja
    public Sprite miraAmbosPortales;    // Estado 3: Los dos puestos

    void Update()
    {
        ActualizarInterfazArmas();
    }

    void ActualizarInterfazArmas()
    {
        if (imagenMiraUI == null) return;

        // Aseguramos que el Alpha esté siempre a tope (255) para que no sea transparente
        imagenMiraUI.color = new Color(imagenMiraUI.color.r, imagenMiraUI.color.g, imagenMiraUI.color.b, 1f);

        // 1. ¿LLEVAMOS EL ARMA DE PORTALES EN LA MANO?
        if (objetoArmaPortales != null && objetoArmaPortales.activeInHierarchy)
        {
            // Comprobamos cuáles están activos físicamente en la escena
            bool p1Activo = portal1 != null && portal1.activeInHierarchy;
            bool p2Activo = portal2 != null && portal2.activeInHierarchy;

            // Cambiamos el sprite según la combinación de portales en el mapa
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

            return; // Salimos, ya procesamos el arma de portales
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
}