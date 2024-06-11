using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public CarController carController;
    private bool[] checkpointArray = new bool[29];
    private int currentCheckpointIndex = 0;
    private float lastCheckpointTime;

    public override void Initialize()
    {
        carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        carController.ResetCar();
        checkpointArray = new bool[29];
        currentCheckpointIndex = 0;
        lastCheckpointTime = Time.time;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(carController.transform.position);
        sensor.AddObservation(carController.transform.rotation);
        sensor.AddObservation(carController.carRb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float steerInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        carController.MoveInput(moveInput);
        carController.SteerInput(steerInput);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical"); // throttle
        continuousActions[1] = Input.GetAxis("Horizontal"); // steer

        AddReward(-0.01f); // Time penalty, encourage the car to move faster
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("checkpoint" + currentCheckpointIndex))
        {
            checkpointArray[currentCheckpointIndex] = true;
            AddReward(1.0f);
            Debug.Log("Checkpoint " + currentCheckpointIndex + " achieved");
            lastCheckpointTime = Time.time;

            currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpointArray.Length;

            if (currentCheckpointIndex == 1 && DidTheCarCollectAllCheckpoints())
            {
                Debug.Log("Car crosses the finish line after completing the course");
                AddReward(5.0f);
                EndEpisode();
            }
        }
    }

    private bool DidTheCarCollectAllCheckpoints()
    {
        foreach (bool checkpointflag in checkpointArray)
        {
            if (!checkpointflag)
            {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if (Time.time - lastCheckpointTime > 60f) // If more than a minute passed since the last checkpoint
        {
            Debug.Log("Time out! More than a minute passed since the last checkpoint.");
            EndEpisode();
        }
    }
}
