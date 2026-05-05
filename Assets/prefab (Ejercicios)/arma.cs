using Unity.VisualScripting;
using UnityEngine;

public class arma : MonoBehaviour
{

    public bala proyectil;
    public balaGravedad proyectilGravedad;
    public Transform canyon;
    public GameObject Balas;
    public GameObject BalasGravedad;
    float ultimotiro;
    public float cadencia;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void intento()
    {
        if (Time.time > (cadencia + ultimotiro))
        {
            ultimotiro = Time.time;
            spawnbala();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            intento();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            spawnbala2();
        }
    }

    private void spawnbala()
    {
        bala proyectilx = Instantiate(proyectil, canyon.position, canyon.rotation);
        proyectilx.transform.SetParent(Balas.transform);
    }

    private void spawnbala2()
    {
        balaGravedad proyectilx = Instantiate(proyectilGravedad, canyon.position, canyon.rotation);
        proyectilx.transform.SetParent(Balas.transform);
    }
}
