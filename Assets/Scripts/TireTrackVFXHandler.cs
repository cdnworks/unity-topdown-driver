using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTrackVFXHandler : MonoBehaviour
{

    //Component references
    carControl carControl;
    TrailRenderer trailRenderer;



    //awake is called when the script instance is loaded
    private void Awake()
    {
        //get the car control script (get component in parent because this script is for a child object)
        carControl = GetComponentInParent<carControl>();

        //get the trail renderer component from unity
        trailRenderer = GetComponent<TrailRenderer>();

        //set the trail renderer to OFF to start.
        trailRenderer.emitting = false;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if the car is skidding, if so, deploy smokes!
        if (carControl.IsSkidding(out float lateralVelocity, out bool isCornerBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false;
    }
}
