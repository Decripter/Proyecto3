using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask Suelo;
    public float RadioEsfera;
    public bool Tocando => _Tocando;
    private bool _Tocando;

    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, RadioEsfera);
    }

    void Update()
    {
        _Tocando = Physics.CheckSphere(transform.position, RadioEsfera, Suelo);
        if (_Tocando)
        {
            //Debug.Log("Estoy tocando suelo");
        }

    }
}
