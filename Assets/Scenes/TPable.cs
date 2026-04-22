using System;
using System.Collections;
using UnityEngine;

public class TPable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody rb;
    public bool Cruzando = false;
    private Collider _Collider;

    public int portalesTocando = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _Collider = GetComponent<Collider>();
        if (Cruzando) 
        {
        };
        
    }

    // Update is called once per frame
    void Update()
    {
        if(portalesTocando <= 0 && !Cruzando)
        {
            RestablecerColiciones();
            portalesTocando = 0;
        }
    }

    public void RegistrarEntrada()
    {
        portalesTocando++;
        //Physics.IgnoreLayerCollision(7, 8, true);
    }

    // Llamar a esto desde el Portal en OnTriggerExit
    public void RegistrarSalida()
    {
        portalesTocando--;
        if (portalesTocando < 0)
        { portalesTocando = 0; }
    }

    private void RestablecerColiciones()
    {
        Physics.IgnoreLayerCollision(7,8, false);
    }

    public void Teletransportar(Transform entrada, Transform salida, Collider ParedAtras, Collider ParedBtras)
    {
        Debug.Log("cruzando");
        //Cruzando = true;

        Vector3 posRelativa = entrada.InverseTransformPoint(transform.position);
        Vector3 posInvertida = Quaternion.Euler(0, 180, 0) * posRelativa;
        transform.position = salida.TransformPoint(posInvertida);

        Quaternion rotRelativa = Quaternion.Inverse(entrada.rotation) * transform.rotation;
        transform.rotation = salida.rotation * (Quaternion.Euler(0, 180, 0) * rotRelativa);

        Vector3 speedLocal = entrada.InverseTransformDirection(rb.linearVelocity);
        rb.linearVelocity = salida.TransformDirection(Quaternion.Euler(0, 180, 0) * speedLocal);

        StartCoroutine(ReactivarColision(entrada, salida, ParedAtras, ParedBtras));

        /*Vector3 posSalida = salida.InverseTransformPoint(transform.position);

        
        if (posSalida.z > 0.0005f) 
        {
            Physics.IgnoreLayerCollision(7, 8, false);
        }*/
    }

    private IEnumerator ReactivarColision(Transform entrada, Transform salida, Collider Atras, Collider BAtras)
    {
        // Esperamos a que el objeto esté completamente "fuera" del plano
        yield return new WaitUntil(() => {
            Cruzando = true;
            Vector3 posRelativa = salida.InverseTransformPoint(transform.position);
            
            return posRelativa.z > 0.005f; //
        });
        Cruzando = false;
        Physics.IgnoreCollision(_Collider, Atras, false);
        Physics.IgnoreCollision(_Collider, BAtras, false);
        //Physics.IgnoreLayerCollision(7, 8, false);

    }

}
