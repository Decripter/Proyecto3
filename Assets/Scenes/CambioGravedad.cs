using UnityEngine;

public class CambioGravedad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool Invertir;
    private Rigidbody rb;

    [SerializeField] private float Valor;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Valor = -9.8f;
        Invertir = true;
    }

    // Update is called once per frame
    void Update()
    {
        Alterar(Valor);
    }

    public float GetValor()
    {
        return Valor;
    }

    public void Alterar(float NuevoValor)
    {
        Valor = NuevoValor;
    }
    void FixedUpdate()
    {
        // Desactivamos la gravedad normal y aplicamos una hacia ARRIBA
        if (!Invertir)
        {
            rb.useGravity = false;
            rb.AddForce(Vector3.up * Valor, ForceMode.Acceleration);
        }

        else
        {
            rb.useGravity = true;
        }
    }
}
