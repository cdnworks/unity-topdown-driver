using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSmokeVFXHandler : MonoBehaviour
{
    //local vars
    float particleEmissionRate = 0f;


    //components
    carControl carControl;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    //make assignments
    private void Awake()
    {
        carControl = GetComponentInParent<carControl>();
        particleSystemSmoke = GetComponent<ParticleSystem>();

        //get the emission component from the unity particle system
        particleSystemEmissionModule = particleSystemSmoke.emission;

        //set the emission module to 0 to start
        particleSystemEmissionModule.rateOverTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        //reduce particles over time
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        //check if the car is in a skid state, apply particles accordingly
        if (carControl.IsSkidding(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
                particleEmissionRate = 30; //lots of smoke from standing on the brakes

            else particleEmissionRate = Mathf.Abs(lateralVelocity) * 2; //smoke based on how hard you're drifting
        }



    }
}
