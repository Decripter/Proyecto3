using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Update()
    {

        if(Input.GetKeyUp(KeyCode.V))
        {
            spawnonmouse();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 position = new Vector3(1, 0, 0);
            for (int i = 0; i < 10; i++) 
            {
                float x = Random.Range(-10f, 10f);
                Vector3 position2 = new Vector3(x, 0, 0);
                SpawnOne(position2);
            }
        }
    }
    
    private void SpawnOne()
    {
            Instantiate(prefab, transform.position, transform.rotation);
    }

    private void SpawnOne(Vector3 pos)
    {
        GameObject cubo =  Instantiate(prefab, pos, transform.rotation);
        cubo.transform.localScale = Vector3.one * Random.value;
        cubo.transform.SetParent(transform);

        cubo.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0f);
    }

    // Update is called once per frame

    void spawnonmouse()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos2 = Input.mousePosition;
        pos2.z = 10;
        SpawnOne(pos);
    }

}
