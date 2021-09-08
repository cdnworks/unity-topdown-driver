using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLightVFXHandler : MonoBehaviour
{

    //components
    carControl carControl;
    SpriteRenderer spriteRenderer;

    //local variables
    bool brake = false;
    bool reverse = false;

    //get components
    private void Awake()
    {
        carControl = GetComponentInParent<carControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get car reverse input status
        carControl.IsStopping(out brake,out reverse);
        if (brake)
        {
            //turn on the brake lights
            spriteRenderer.enabled = true;
        }
        //otherwise the lights are off
        else spriteRenderer.enabled = false;

    }
}
