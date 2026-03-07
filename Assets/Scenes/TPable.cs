using System.Collections;
using UnityEngine;

public class TPable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody rb;
    public bool Cruzando = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (Cruzando) 
        {
        };
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teletransportar(Transform entrada, Transform salida)
    {
        Debug.Log("cruzando");
        Cruzando = true;

        Vector3 posRelativa = entrada.InverseTransformPoint(transform.position);
        Vector3 posInvertida = Quaternion.Euler(0, 180, 0) * posRelativa;
        transform.position = salida.TransformPoint(posInvertida);

        Quaternion rotRelativa = Quaternion.Inverse(entrada.rotation) * transform.rotation;
        transform.rotation = salida.rotation * (Quaternion.Euler(0, 180, 0) * rotRelativa);

        Vector3 speedLocal = entrada.InverseTransformDirection(rb.linearVelocity);
        rb.linearVelocity = salida.TransformDirection(Quaternion.Euler(0, 180, 0) * speedLocal);

        StartCoroutine(ReactivarColision(entrada, salida));
    }

    private IEnumerator ReactivarColision(Transform entrada, Transform salida)
    {
        // Esperamos a que el objeto esté completamente "fuera" del plano
        yield return new WaitUntil(() => {
            Vector3 posRelativa = salida.InverseTransformPoint(transform.position);
            return posRelativa.z > 0.005f; //
        });

        Physics.IgnoreLayerCollision(7, 8, false);
        Cruzando = false;
    }

}
