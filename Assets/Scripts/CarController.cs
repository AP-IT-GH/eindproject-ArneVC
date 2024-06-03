using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarController : Agent
{
    public enum ControlMode
    {
        Keyboard,
        Buttons,
        MLAgent
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

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    public override void Initialize()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    public override void OnEpisodeBegin()
    {
        ResetCar();
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(moveInput);
        sensor.AddObservation(steerInput);
        sensor.AddObservation(carRb.velocity);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        moveInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        steerInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);


        Move();
        Steer();
        Brake();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical"); // throttle
        continuousActions[1] = Input.GetAxis("Horizontal"); // steer
    }

    void Update()
    {
        if (control == ControlMode.Keyboard)
        {
            GetInputs();
        }
        AnimateWheels();
    }

    void LateUpdate()
    {
        if (control != ControlMode.MLAgent)
        {
            Move();
            Steer();
            Brake();
        }
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

    void ResetCar()
    {
        carRb.velocity = Vector3.zero;
        carRb.angularVelocity = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        moveInput = 0;
        steerInput = 0;
    }
}
