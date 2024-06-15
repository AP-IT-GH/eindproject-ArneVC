using UnityEngine;
using System;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveCar;
    float steerCar;

    public Rigidbody carRb;



    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        //GetInputs();
        AnimateWheels();
        ResetRotationdrift();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    //public void MoveInput(float input)
    //{
    //    moveCar = input;
    //}

    //public void SteerInput(float input)
    //{
    //    steerCar = input;
    //}

    public void GetInputs(float moveInput, float steerInput)
    {
        moveCar = moveInput;
        steerCar = steerInput;
        
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveCar = Input.GetAxis("Vertical");
            steerCar = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveCar * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerCar * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveCar == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }

        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }

        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
    void ResetRotationdrift()
    {
        Quaternion currentRotation = carRb.rotation;

        Quaternion newRotation = Quaternion.Euler(
                currentRotation.eulerAngles.x,
                currentRotation.eulerAngles.y,
                0f
        );
        carRb.MoveRotation(newRotation);
    }

    public void ResetCar()
    {
        carRb.velocity = Vector3.zero;
        carRb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(-84.68f, 1.7f, 90.63f);  // Adjust this position as necessary
        transform.rotation = Quaternion.Euler(1.532f, 89.656f, 0f);  // Adjust this rotation as necessary
        moveCar = 0;
        steerCar = 0;
    }

}