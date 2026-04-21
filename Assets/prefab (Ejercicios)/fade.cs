using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class fade : MonoBehaviour
{
    private Renderer Renderer;
    public float min_alpha;
    public float max_alpha;
    public float speed;
    public float fadeduration;
    public Gradient migradiente;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            randomcolor();
            /*StartCoroutine(fadeaway());
            fadeaway();*/
            //cambiogradiente();
        }
        float f = Mathf.Sin(Time.time * speed) / +0.5f;
    }
    void randomcolor()
    {
        Color X = new Color(Random.value, Random.value, Random.value, Random.value);
        Renderer.material.color = X;
        float f = math.sin(Time.time);
    }

    void setalpha(float x)
    {
        float alpha = math.lerp(min_alpha, max_alpha, x);
        Color c = Renderer.material.color;
        c.a = alpha;
        Renderer.material.color = c;
    }

    /*IEnumerator retrasofade()
    {
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(cambiogradiente());
    }*/

    IEnumerator cambiogradiente()
    {
        for (float i = 0; i < fadeduration; tag+=Time.deltaTime)
        {
            Renderer.material.color = migradiente.Evaluate(i / fadeduration);
            yield return null;
        }
    }


    IEnumerator fadeaway()
    {
        for(float i = fadeduration; i < 10; i-=Time.deltaTime)
        {
            setalpha(i/fadeduration);
            yield return null;
        }
    }
}
