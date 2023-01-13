using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EnterBuilding : MonoBehaviour
{
    private bool isInDistance = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter(InputAction.CallbackContext context)
    {
        if(context.performed && isInDistance)
        {
            Debug.Log("enters building");

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        isInDistance = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        isInDistance = false;
    }

}
