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
    private Vector3 lastPosition;
    float timeSinceLastCheck;
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
        lastPosition = carController.transform.position;
        timeSinceLastCheck = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(carController.transform.position);
        sensor.AddObservation(carController.transform.rotation);
        sensor.AddObservation(carController.carRb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        float moveInput = 0f;
        float steerInput = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0: moveInput = 0f;
                break;
            case 1: moveInput = -1f;
                break;
        }

        switch(actions.DiscreteActions[1]) {
            case 0:
                steerInput = 0f;
                break;
            case 1:
                steerInput = 1f;
                break;
            case 2:
                steerInput = -1f;
                break;
        }


        carController.GetInputs(moveInput, steerInput);


        if(moveInput == 0)
        {
            AddReward(-0.01f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
 
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = -Input.GetAxis("Vertical"); // throttle
        continuousActions[1] = Input.GetAxis("Horizontal"); // steer

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("checkpoint" + currentCheckpointIndex))
        {
            checkpointArray[currentCheckpointIndex] = true;
            AddReward(1f);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("invBarrier"))
        {
            SetReward(-0.5f);
            Debug.Log("Car touched wall");
            EndEpisode();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("invBarrier"))
        {
            SetReward(-0.5f);
            Debug.Log("still touching wall");
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
            AddReward(-0.5f);
            EndEpisode();
        }
        
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck > 3f)
        {
            float distanceMoved = Vector3.Distance(carController.transform.position, lastPosition);
            if (distanceMoved <= 0.1f)
            {
                Debug.Log("Time out! Car is stuck.");
                AddReward(-1f);
                timeSinceLastCheck = 0f;
                EndEpisode();
            }
            timeSinceLastCheck = 0f;
            lastPosition = carController.transform.position;
        }
        
    }
}
