using Unity.VisualScripting;
using UnityEngine;

public class arma : MonoBehaviour
{

    public bala proyectil;
    public Transform canyon;
    public GameObject Balas;
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
    }

    private void spawnbala()
    {
        bala proyectilx = Instantiate(proyectil, canyon.position, canyon.rotation);
        proyectilx.transform.SetParent(Balas.transform);
    }
}
