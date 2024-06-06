using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public CarController carController;

    public override void Initialize()
    {
        carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the car and environment at the beginning of each episode
        carController.ResetCar();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect observations for the agent
        sensor.AddObservation(carController.transform.position);
        sensor.AddObservation(carController.transform.rotation);
        sensor.AddObservation(carController.carRb.velocity);
        // Add more observations as needed
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Convert actions to control signals
        float moveInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float steerInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        // Apply the controls using the CarController script
        carController.MoveInput(moveInput);
        carController.SteerInput(steerInput);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Implement heuristic controls for testing
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical"); // throttle
        continuousActions[1] = Input.GetAxis("Horizontal"); // steer
    }
}
