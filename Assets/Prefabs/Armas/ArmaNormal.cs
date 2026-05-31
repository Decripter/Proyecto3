using UnityEngine;

public class ArmaNormal : MonoBehaviour
{
    public bala proyectil;
    public balaGravedad proyectilGravedad;
    public Transform canyon;
    public GameObject Balas;
    public GameObject BalasGravedad;
    public Animator _animatorArma;
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
        _animatorArma.SetTrigger("DisparoPesado");
        bala proyectilx = Instantiate(proyectil, canyon.position, canyon.rotation);
        proyectilx.transform.SetParent(Balas.transform);

        // Reproduce PistolaLaser V2 en la posición del arma
        FMODUnity.RuntimeManager.PlayOneShot(sonidoPistolaLaser, transform.position);
    }
}
