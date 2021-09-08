using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireHandler : MonoBehaviour
{

    //components
    carControl carControl;



    //local vars
    float steerInput = 0;
    float carAngle = 0;
    float wheelLockAngle = 0;

    //get components
    private void Awake()
    {
        carControl = GetComponentInParent<carControl>();
    }




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //get the car's current angle and check if the car is turning. Rotate the tire assigned this script accordingly
        carAngle = carControl.GetCarAngle();
        steerInput = carControl.GetTurnInput();

        //if turning left, add 40deg to set wheel lock
        if (steerInput > 0)
        {
            wheelLockAngle = carAngle + 40;
        }
        //if turning right, subtract 40deg to set wheel lock
        if (steerInput < 0)
        {
            wheelLockAngle = carAngle - 40;
        }

        transform.Rotate(Vector3.forward, wheelLockAngle);





    }
}
