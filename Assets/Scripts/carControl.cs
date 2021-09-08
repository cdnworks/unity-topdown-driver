using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carControl : MonoBehaviour
{
    [Header("Car Attributes")]
    public float driftRate = 0.95f;
    public float accRate = 30.0f;
    public float turnRate = 2.5f;
    public float forwardSpeedTurnRateFactor = 8f;
    public float maxSpeed = 20.0f;
    public float dragForce =3.0f;
    public float slideThreshold = 4.0f;

    //local car variables
    float accInput = 0;
    float steerInput = 0;

    float rotationAng = 0;

    float velocityVsUp = 0;


    //components
    Rigidbody2D carRigidBody2D;

    private void Awake()
    {
        carRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyEngineForce();

        ApplySteeringForce();

        RemOrthogonalVelocity();
    }

    void ApplyEngineForce()
    {
        //calculate 'forward'(read 'up transform') magnitude vs. the velocity magnitude
        velocityVsUp = Vector2.Dot(transform.up, carRigidBody2D.velocity);

        //cap max speeds in forward, reverse and arbitrary directions respectively
        if (velocityVsUp > maxSpeed && accInput > 0)
            return;

        if (velocityVsUp < -maxSpeed*0.5f && accInput < 0)
            return;

        if (carRigidBody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accInput > 0)
            return;

        //create a drag force, slows down car when there is no acceleration; stands in for drag/friction
        if (accInput == 0)
            carRigidBody2D.drag = Mathf.Lerp(carRigidBody2D.drag, dragForce, Time.fixedDeltaTime * 3);
        else carRigidBody2D.drag = 0;

        //forward 'engine' vector
        Vector2 engineForceVector = transform.up * accInput * accRate;

        //apply vector as force
        carRigidBody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteeringForce()
    {
        //limit car rotation based on forward momentum (shouldn't be able to turn in place)
        float minSpeedToTurn = carRigidBody2D.velocity.magnitude / forwardSpeedTurnRateFactor; // normalize by forwardSpeedTurnRateFactor; this is arbitrary, affects feel
        minSpeedToTurn = Mathf.Clamp01(minSpeedToTurn);

        //get a rotation angle from input, scale by turn rate and the minimum speed to turn
        rotationAng -= steerInput * turnRate * minSpeedToTurn;

        //apply this angle to the car
        carRigidBody2D.MoveRotation(rotationAng);



    }

    //reduce 'sideways' components of velocity. Reduces floaty feel
    void RemOrthogonalVelocity()
    {
        //calculate forward velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidBody2D.velocity, transform.up);
        //calculate right angle velocity
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidBody2D.velocity, transform.right);

        //reduce right angle vector (orthogonal to forward vec) magnitude based  on drift rate
        carRigidBody2D.velocity = forwardVelocity + rightVelocity * driftRate;
    }


    float GetLateralVelocity()
    {
        //find and return sideways momentum magnitude.
        return Vector2.Dot(transform.right, carRigidBody2D.velocity);
    }




    public void SetInputVector(Vector2 inputVector)
    {
        //collect input from carInputHandler.cs
        steerInput = inputVector.x;
        accInput = inputVector.y;
    }

    public float GetTurnInput()
    {
        //find and return player input, for controlling wheels
        return steerInput;
    }

    public float GetCarAngle()
    {
        return transform.eulerAngles.z;
    }


    //checking for brake inputs and if reversing; for fun lights I guess
    public void IsStopping(out bool isBraking, out bool isReversing)
    {
        //set initial states to false
        isBraking = false;
        isReversing = false;

        //check for brake input and reverse movement
        if (accInput < 0 && velocityVsUp < 0)
        {
            isReversing = true;
        }

        //check for regular brake conditions (i.e. not reversing yet)
        if (accInput < 0 && velocityVsUp >= 0)
        {
            isBraking = true;
        }

        else return;
    }


    public bool IsSkidding(out float lateralVelocity, out bool isCornerBraking)
    {
        lateralVelocity = GetLateralVelocity();
        //set isBraking to false initially.
        isCornerBraking = false;


        //check if we're moving forward and if we're hitting the brakes, if so, that's a skid baybeeeeeeeeeeee
        if (accInput < 0 && velocityVsUp > 0)
        {
            isCornerBraking = true;
            return true;
        }

        //check if you're just sliding around a lot. Slide threshold is arbitrary, season to taste.
        if (Mathf.Abs(GetLateralVelocity()) > slideThreshold)
            return true;

        //if you aint sliding or braking hard, we aint skidding
        return false;
    }

}
