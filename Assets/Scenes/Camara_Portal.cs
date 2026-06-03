using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
// Estos son seguros para Build:
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//Physics.IgnoreCollision(other (el objeto), colliderDeLaPared, true); (usarlo con el raycast de la pared de atr�s)
public class Camara_Portal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Jugador;
    public Camera CamaraJugador;
    public Camera OtraCamara;
    public Transform OtroPortal;

    public bool Player_Cerca;
    public float umbral = 0.31f;
    public bool esPlayer = false;
    public List<Collider> ParedesAtrasA = new List<Collider>();
    public List<Collider> ParedesAtrasB = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<TPable>(out TPable obj))
        {
            // --- LIMPIEZA INICIAL ---
            ParedesAtrasA.Clear();
            ParedesAtrasB.Clear();

            // --- PORTAL A: Rayo perforante hacia atrás ---
            RaycastHit[] hitsA = Physics.RaycastAll(transform.position, -transform.forward, 1f);
            foreach (RaycastHit hit in hitsA)
            {
                Collider col = hit.collider;
                // Evitamos apagar la colisión con nosotros mismos u objetos importantes
                if (col != other && !col.isTrigger)
                {
                    ParedesAtrasA.Add(col);
                    Physics.IgnoreCollision(other, col, true);
                }
            }

            // --- PORTAL B: Rayo perforante hacia atrás ---
            RaycastHit[] hitsB = Physics.RaycastAll(OtroPortal.position, -transform.forward, 1f);
            foreach (RaycastHit hit in hitsB)
            {
                Collider col = hit.collider;
                if (col != other && !col.isTrigger)
                {
                    ParedesAtrasB.Add(col);
                    Physics.IgnoreCollision(other, col, true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Restauramos las colisiones del Portal A
        foreach (Collider col in ParedesAtrasA)
        {
            if (col != null) Physics.IgnoreCollision(other, col, false);
        }
        ParedesAtrasA.Clear();

        // Restauramos las colisiones del Portal B
        foreach (Collider col in ParedesAtrasB)
        {
            if (col != null) Physics.IgnoreCollision(other, col, false);
        }
        ParedesAtrasB.Clear();

        Player_Cerca = false;
    }

    private void OnTriggerStay(Collider other)
    {
        var obj = other.GetComponent<TPable>();
        if (obj != null)
        {
            Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, 1f);

            // Calculamos si el centro del objeto cruz� el plano
            Vector3 posRelativa = transform.InverseTransformPoint(other.transform.position);

            if (posRelativa.z < 0) // Cruz� al lado negativo
            {
                obj.Teletransportar(transform, OtroPortal, ParedesAtrasA, ParedesAtrasB);
            }
        }
        else
        {
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate() //rotaciones, traslaciones de c�maraB
    {
        //OtraCamara.transform.rotation = CamaraJugador.transform.rotation;

        Quaternion rotacionRelativa = Quaternion.Inverse(transform.rotation) * CamaraJugador.transform.rotation;
        Quaternion giro180 = Quaternion.Euler(0, 180, 0);

        Vector3 posicionRelativa = transform.InverseTransformPoint(CamaraJugador.transform.position);
        Vector3 posicionInvertida = giro180 * posicionRelativa;

        OtraCamara.transform.position = OtroPortal.TransformPoint(posicionInvertida);
        OtraCamara.transform.rotation = OtroPortal.rotation * giro180 * rotacionRelativa;

        ActualizarPlanoDeCorte();
    }

    void ActualizarPlanoDeCorte() //Esto es full Gemini, es para hacer el tp, antes del clipping de c�mara
    {
        // 1. Definir el plano en el mundo (Portal B)
        // El "forward" del portal es la direcci�n normal del plano
        Vector3 normal = OtroPortal.forward;
        float distancia = -Vector3.Dot(normal, OtroPortal.position);
        Vector4 planoEnMundo = new Vector4(normal.x, normal.y, normal.z, distancia);

        // 2. Convertir el plano al espacio de la c�mara
        Matrix4x4 matrix = OtraCamara.worldToCameraMatrix;
        Vector4 planoEnCamara = matrix.inverse.transpose * planoEnMundo;

        // 3. Aplicar la matriz oblicua a la c�mara
        OtraCamara.projectionMatrix = OtraCamara.CalculateObliqueMatrix(planoEnCamara);
    }

    private void Update()
    {
        Vector3 portal_Camara = CamaraJugador.transform.position - transform.position; //Direcci�n

        float puntoPos = Vector3.Dot(transform.forward, portal_Camara);

        Vector3 portal_Jugador = CamaraJugador.transform.position - transform.position;
        float puntoPos2 = Vector3.Dot(transform.forward, portal_Jugador);

        /*
        if (puntoPos < 0f && Player_Cerca)
        {
                float puntoAngulo = Vector3.Dot(transform.forward, CamaraJugador.transform.forward);
                Physics.IgnoreLayerCollision(3, 8, true);
                Tp();
            /*if(puntoAngulo < 0f){};
            
            
            

        }*/
    }

    void Tp()
    {
        Movement Movement_Player = Jugador.GetComponent<Movement>();
        Rigidbody rb = Jugador.GetComponent<Rigidbody>();

        Vector3 posRelativa = transform.InverseTransformPoint(Jugador.transform.position);
        Vector3 posInvertida = Quaternion.Euler(0, 180, 0) * posRelativa;
        rb.position = OtroPortal.TransformPoint(posInvertida);

        Quaternion rotRelativa = Quaternion.Inverse(transform.rotation) * Jugador.transform.rotation;
        rb.rotation = OtroPortal.rotation * (Quaternion.Euler(0, 180, 0) * rotRelativa);

        Vector3 Velocidad = transform.InverseTransformPoint(rb.linearVelocity);
        rb.linearVelocity = -OtroPortal.transform.forward * rb.linearVelocity.y * 2;


        StartCoroutine(ReactivarColision());
        StartCoroutine(ReactivarColision2());
    }
    private IEnumerator ReactivarColision2()
    {
        yield return new WaitUntil(() => {
            //Vector3 posRelativa = OtroPortal.InverseTransformPoint(Jugador.transform.position);
            Vector3 posRelativa = OtroPortal.InverseTransformPoint(Jugador.transform.position);
            return posRelativa.z > 0.005f; //
        });
        Physics.IgnoreLayerCollision(3, 8, false);
        //8 Muro Portal
    }
    private IEnumerator ReactivarColision()
    {
        // Esperamos a que el jugador est� completamente "fuera" del plano
        yield return new WaitUntil(() => {
            //Vector3 posRelativa = OtroPortal.InverseTransformPoint(Jugador.transform.position);
            Vector3 posRelativa = OtroPortal.InverseTransformPoint(CamaraJugador.transform.position);
            return posRelativa.z > 0.00005f; //
        });

        //Ponhysics.IgnoreLayerCollisi(3, 9, false);
        //9 Muro Portal, que encima, ni hace falta
        Player_Cerca = false;

        Rigidbody rb = Jugador.gameObject.GetComponent<Rigidbody>();

        float duracion = 0.5f; // Medio segundo para enderezarse
        float tiempoPasado = 0f;

        Quaternion rotInicial = rb.rotation;

        Vector3 eulerActual = rotInicial.eulerAngles;
        Quaternion rotDestino = Quaternion.Euler(0, eulerActual.y, 0);

        //rb.rotation = OtroPortal.rotation * (Quaternion.Euler(0, 180, 0) * rotRelativa);
        if (Quaternion.Angle(rotInicial, rotDestino) > 0.1f && esPlayer)
        {
            while (tiempoPasado < duracion)
            {
                tiempoPasado += Time.deltaTime;
                float t = tiempoPasado / duracion;

                float tSuave = t * t * (3f - 2f * t);

                // gira el rat�n mientras se endereza (para no perder el control)
                Vector3 eulerVivo = rb.rotation.eulerAngles;
                Quaternion destinoVivo = Quaternion.Euler(0, eulerVivo.y, 0);

                rb.rotation = Quaternion.Slerp(rb.rotation, destinoVivo, tSuave);

                yield return null;
            }
            // Ajuste final perfecto
            Vector3 finalY = rb.rotation.eulerAngles;
            rb.rotation = Quaternion.Euler(0, finalY.y, 0);
        }
        esPlayer = false;
    }


}