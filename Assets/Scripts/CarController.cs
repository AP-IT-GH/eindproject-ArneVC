using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.EventSystems;


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
        //public GameObject wheelEffectObj;
        //public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;



    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
        ResetRotationdrift();
        //WheelEffects();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
        else
        {
            if (Input.GetKey(KeyCode.I))
            {
                moveInput = -1;
            }
            if (Input.GetKey(KeyCode.K))
            {
                moveInput += 1;
            }
            if (Input.GetKey(KeyCode.J))
            {
                steerInput += -1;
            }
            if (Input.GetKey(KeyCode.L))
            {
                steerInput += 1;
            }
        }
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0)
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

    //void WheelEffects()
    //{
    //    foreach (var wheel in wheels)
    //    {
    //        //var dirtParticleMainSettings = wheel.smokeParticle.main;

    //        if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f)
    //        {
    //            wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
    //            wheel.smokeParticle.Emit(1);
    //        }
    //        else
    //        {
    //            wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
    //        }
    //    }
    //}
}
