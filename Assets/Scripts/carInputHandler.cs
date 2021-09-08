using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carInputHandler : MonoBehaviour
{
    //components
    //this script will use info from the carControl script
    carControl carControl;


    private void Awake()
    {
        carControl = GetComponent<carControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //collect user input
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        carControl.SetInputVector(inputVector);
    }
}
