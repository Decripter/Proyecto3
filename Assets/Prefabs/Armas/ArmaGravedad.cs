using UnityEngine;

public class ArmaGravedad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bala proyectil;
    public balaGravedad proyectilGravedad;
    public Transform canyon;
    public GameObject Balas;
    public GameObject BalasGravedad;
    float ultimotiro;
    public float cadencia;

    [Header("Sonidos de FMOD")]
    // Aquí asignaremos el evento PistolaLaser V2 en el Inspector
    public FMODUnity.EventReference sonidoPistolaLaser;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            intento();
        }
    }


    private void spawnbala()
    {
        balaGravedad proyectilx = Instantiate(proyectilGravedad, canyon.position, canyon.rotation);
        // Tip: He cambiado Balas por BalasGravedad aquí para que coincida con tu variable
        proyectilx.transform.SetParent(BalasGravedad.transform);

        // Si quieres que la bala de gravedad suene igual, usamos la misma variable
        FMODUnity.RuntimeManager.PlayOneShot(sonidoPistolaLaser, transform.position);
    }
}
