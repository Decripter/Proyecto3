using System.Collections.Generic;
using UnityEngine;

public class ReiniciarHabitacion : MonoBehaviour, IInteractable
{
    public List<Transform> gruposDeObjetos;
    public List<Transform> individuales;
    private Dictionary<Transform, (Vector3 pos, Quaternion rot)> posicionesIniciales = new Dictionary<Transform, (Vector3, Quaternion)>();

    public Camara_Portal Portal1;
    public Camara_Portal Portal2;

    [Header("NUEVO FMOD: Sonido")]
    public FMODUnity.EventReference sonidoTelefono;

    void Start()
    {
        // Bucle externo: Recorre cada Empty Parent que añadiste a la lista en el Inspector
        foreach (Transform grupo in gruposDeObjetos)
        {
            // Bucle interno: Recorre cada objeto físico dentro de ese Empty actual
            foreach (Transform hijo in grupo)
            {
                // Aquí haces tu lógica de guardado en el diccionario
                posicionesIniciales.Add(hijo, (hijo.position, hijo.rotation));
            }
        }

        foreach (Transform hijo in individuales)
        {
            posicionesIniciales.Add(hijo, (hijo.position, hijo.rotation));

        }
    }

    private void Update()
    {
    }

    public void EjecutarReset()
    {

        if (!sonidoTelefono.IsNull)
        {
            // ◄ SOLUCIÓN: Forzamos que el sonido 3D se genere exactamente en la cámara del jugador
            if (Camera.main != null)
            {
                FMODUnity.RuntimeManager.PlayOneShot(sonidoTelefono, Camera.main.transform.position);
            }
            else
            {
                // Por si acaso la cámara principal no tiene el Tag "MainCamera", usa la posición del objeto
                FMODUnity.RuntimeManager.PlayOneShot(sonidoTelefono, transform.position);
            }
        }

        foreach (var obj in posicionesIniciales)
        {
            Rigidbody rb = obj.Key.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.ResetInertiaTensor();

                // PASO 3 (Integración de tu código): Reseteamos la gravedad personalizada
                CambioGravedad gravedad = obj.Key.GetComponent<CambioGravedad>();
                if (gravedad != null)
                {
                    gravedad.Alterar(-9.8f);
                }
                rb.position = obj.Value.pos;
                rb.rotation = obj.Value.rot;
                rb.isKinematic = false;

            }
            else
            {

                obj.Key.position = obj.Value.pos;
                obj.Key.rotation = obj.Value.rot;
            }

        }
        Physics.SyncTransforms();


    }

    public void Interactuar()
    {
        Debug.Log("Ejecutar Reset");
        EjecutarReset();
    }

    public void Interactuar(mirada jugador)
    {
    }
}
