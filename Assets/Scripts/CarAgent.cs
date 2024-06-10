using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Runtime.InteropServices.WindowsRuntime;

public class CarAgent : Agent
{
    public CarController carController;
    private bool[] checkpointArray = new bool[29]; //should always be an array of false bools as that is the default value
    private int currentCheckpointIndex = 0;

    public override void Initialize()
    {
        carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the car and environment at the beginning of each episode
        carController.ResetCar();
        checkpointArray = new bool[29];
        currentCheckpointIndex = 0;
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

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < checkpointArray.Length; i++)
        {
            if (other.CompareTag("checkpoint" + i))
            {
                if (i == currentCheckpointIndex)
                {
                    checkpointArray[i] = true;
                    AddReward(1.0f);
                    Debug.Log("Checkpoint " + i + " achieved");

                    currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpointArray.Length;

                    if (currentCheckpointIndex == 1)
                    {
                        if (DidTheCarCollectAllCheckpoints())
                        {
                            Debug.Log("Car crosses the finish line after completing the course");
                            AddReward(5.0f);
                            EndEpisode();
                        }
                        else
                        {
                            Debug.Log("Car crosses the finish line to start a new lap");
                        }
                    }
                }
                else
                {
                    Debug.Log("Checkpoint " + i + " achieved in the wrong order");
                    AddReward(-1.0f);
                    EndEpisode();
                }
                return;
            }
        }
    }
    private bool DidTheCarCollectAllCheckpoints()
    {
        bool returnvalue = true;
        foreach(bool checkpointflag in checkpointArray)
        {
            if(checkpointflag == false)
            {
                returnvalue = false;
            }
        }
        return returnvalue;
    }
    private int IncrementCurrentCheckpointIndex(int oldCheckpointIndex)
    {
        if(oldCheckpointIndex < 29)
        {
            return oldCheckpointIndex++;
        }
        else
        {
            return 0;
        }
    }
}
