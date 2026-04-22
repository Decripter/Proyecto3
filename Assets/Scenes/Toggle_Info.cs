using UnityEngine;
using UnityEngine.InputSystem;

public class Toggle_Info : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Info;
    public bool Estado = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Info.SetActive(Estado);
            Estado = !Estado;
        };
    }
}
