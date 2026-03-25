/*using System.Collections;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SocialPlatforms;*/

using System.Collections;
using Unity.Mathematics;
using UnityEngine;
// Estos son seguros para Build:
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//Physics.IgnoreCollision(other (el objeto), colliderDeLaPared, true); (usarlo con el raycast de la pared de atrás)
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<TPable>(out TPable obj))
        {
            // Buscamos la pared que está justo detrás del portal
            // Tiramos un pequeño rayo hacia atrás para encontrar el collider de la pared
            if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, 1f))
            {
                // LE DECIMOS A UNITY: "Este objeto específico ignora esta pared específica"
                Physics.IgnoreCollision(other, hit.collider, true);
            }
        }
        else 
        {
         esPlayer = true;
         Player_Cerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player_Cerca = false;
    }

    private void OnTriggerStay(Collider other)
    {
        var obj = other.GetComponent<TPable>();
        if (obj != null)
        {
            Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, 1f);

            // Calculamos si el centro del objeto cruzó el plano
            Vector3 posRelativa = transform.InverseTransformPoint(other.transform.position);

            if (posRelativa.z < 0) // Cruzó al lado negativo
            {
                obj.Teletransportar(transform, OtroPortal);
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
    void LateUpdate() //rotaciones, traslaciones de cámaraB
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

    void ActualizarPlanoDeCorte() //Esto es full Gemini, es para hacer el tp, antes del clipping de cámara
    {
        // 1. Definir el plano en el mundo (Portal B)
        // El "forward" del portal es la dirección normal del plano
        Vector3 normal = OtroPortal.forward;
        float distancia = -Vector3.Dot(normal,OtroPortal.position);
        Vector4 planoEnMundo = new Vector4(normal.x, normal.y, normal.z, distancia);

        // 2. Convertir el plano al espacio de la cámara
        Matrix4x4 matrix = OtraCamara.worldToCameraMatrix;
        Vector4 planoEnCamara = matrix.inverse.transpose * planoEnMundo;

        // 3. Aplicar la matriz oblicua a la cámara
        OtraCamara.projectionMatrix = OtraCamara.CalculateObliqueMatrix(planoEnCamara);
    }

    private void Update()
    {
        Vector3 portal_Camara = CamaraJugador.transform.position - transform.position; //Dirección

        float puntoPos = Vector3.Dot(transform.forward, portal_Camara);

        Vector3 portal_Jugador = CamaraJugador.transform.position - transform.position;
        float puntoPos2 = Vector3.Dot(transform.forward, portal_Jugador);

        if (puntoPos < 0f && Player_Cerca)
        {
                float puntoAngulo = Vector3.Dot(transform.forward, CamaraJugador.transform.forward);
                CharacterController controller = Jugador.GetComponent<CharacterController>();
                Physics.IgnoreLayerCollision(3, 8, true);
                Tp();
            /*if(puntoAngulo < 0f)
            {
            
            }*/

        }
    }

    void Tp()
    {
        Movement Movement_Player = Jugador.GetComponent<Movement>();
        CharacterController _Controller = Jugador.GetComponent<CharacterController>();

        Vector3 velocidadLocalPlayer;
        velocidadLocalPlayer = _Controller.velocity;

        _Controller.enabled = false;

        Vector3 posRelativa = transform.InverseTransformPoint(Jugador.transform.position);
        Vector3 posInvertida = Quaternion.Euler(0,180,0) * posRelativa;
        Jugador.transform.position = OtroPortal.TransformPoint(posInvertida);

        Quaternion rotRelativa = Quaternion.Inverse(transform.rotation) * Jugador.transform.rotation;
        Jugador.transform.rotation = OtroPortal.rotation * (Quaternion.Euler(0, 180, 0) * rotRelativa);
            /*
                Vector3 velLocal = transform.InverseTransformDirection(velocidadLocalPlayer);
                Vector3 velGirada = Quaternion.Euler(0, 180, 0) * velLocal;
                Vector3 nuevaVelocidadMundo = OtroPortal.TransformDirection(velGirada);
                Movement_Player.AplicarVelocidad(nuevaVelocidadMundo);*/

        _Controller.enabled = true;
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
        // Esperamos a que el jugador esté completamente "fuera" del plano
        yield return new WaitUntil(() => {
            //Vector3 posRelativa = OtroPortal.InverseTransformPoint(Jugador.transform.position);
            Vector3 posRelativa = OtroPortal.InverseTransformPoint(CamaraJugador.transform.position);
            return posRelativa.z > 0.00005f; //
        });

        //Physics.IgnoreLayerCollision(3, 9, false);
        //9 Muro Portal, que encima, ni hace falta
        Player_Cerca = false;

        float duracion = 0.5f; // Medio segundo para enderezarse
        float tiempoPasado = 0f;

        Quaternion rotInicial = Jugador.transform.rotation;

 
        Vector3 eulerActual = rotInicial.eulerAngles;
        Quaternion rotDestino = Quaternion.Euler(0, eulerActual.y, 0);


        if (Quaternion.Angle(rotInicial, rotDestino) > 0.1f && esPlayer)
        {
            while (tiempoPasado < duracion)
            {
                tiempoPasado += Time.deltaTime;
                float t = tiempoPasado / duracion;

                float tSuave = t * t * (3f - 2f * t);

                // gira el ratón mientras se endereza (para no perder el control)
                Vector3 eulerVivo = Jugador.transform.rotation.eulerAngles;
                Quaternion destinoVivo = Quaternion.Euler(0, eulerVivo.y, 0);

                Jugador.transform.rotation = Quaternion.Slerp(Jugador.transform.rotation, destinoVivo, tSuave);

                yield return null;
            }
            // Ajuste final perfecto
            Vector3 finalY = Jugador.transform.rotation.eulerAngles;
            Jugador.transform.rotation = Quaternion.Euler(0, finalY.y, 0);
        }
        esPlayer = false;
    }


    /*
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Vector3 posRelativa = transform.InverseTransformPoint(CamaraJugador.transform.position);

        bool estaEnAreaXY = Mathf.Abs(posRelativa.x) < 0.02f && Mathf.Abs(posRelativa.y) < 0.02f;
        // El TP ocurre cuando puntoPos < 0, o sea posRelativa.z < 0
        bool estaEnProfundidad = posRelativa.z < 0.05f && posRelativa.z > -0.1f;

        // CAMBIO DE COLOR: 
        // - Rojo: Estás fuera.
        // - Amarillo: Estás frente al portal (XY correcto) pero no has cruzado.
        // - Azul: Tp Estás dentro y el código debería dispararse.
        if (estaEnAreaXY && estaEnProfundidad) Gizmos.color = Color.blue;
        else if (estaEnAreaXY) Gizmos.color = Color.yellow;
        else Gizmos.color = Color.red;

        Vector3 centro = new Vector3(0, 0, -0.025f); // Centrado ligeramente hacia atrás
        Vector3 tamano = new Vector3(0.02f * 2, 0.02f * 2, 0.15f);

        Gizmos.DrawWireCube(centro, tamano); // WireCube para ver a través de él
    }
    */
}
