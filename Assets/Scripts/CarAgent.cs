using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public CarController carController;
    //private bool[] checkpointArray = new bool[29];
    [SerializeField]private GameObject checkpointsContainer;
    private Transform[] checkpoints;
    private Transform[] crossedCheckpoints;
    private Transform expectedCheckpoint;
    private int indexExpectedCheckpoint;
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
        //checkpointArray = new bool[120];
        indexExpectedCheckpoint = 0;
        Transform[] allTransforms = checkpointsContainer.GetComponentsInChildren<Transform>();
        checkpoints = new Transform[allTransforms.Length - 1];
        for (int i = 1; i < allTransforms.Length; i++)
        {
            checkpoints[i - 1] = allTransforms[i];
        }
        expectedCheckpoint = checkpoints[indexExpectedCheckpoint];
        crossedCheckpoints = new Transform[checkpoints.Length];
        lastCheckpointTime = Time.time;
        lastPosition = carController.transform.position;
        timeSinceLastCheck = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(indexExpectedCheckpoint);
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
            case 2: moveInput = 1f;
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
            AddReward(-0.1f);
        }

        if (moveInput == 1)
        {
            AddReward(-0.01f);
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int move = 0;
        int steer = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            move = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            move = 2;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            steer = 2;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            steer = 1;
        }

        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = move;
        discreteActions[1] = steer;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("checkpoint"))
        {
            Debug.Log("expected checkpoint:" + expectedCheckpoint.name);

            if (other.gameObject.name == expectedCheckpoint.name)
            {
                if (!DidTheCarCollectAllCheckpoints())
                {
                    AddReward(1f);
                    Debug.Log("reached " + other.gameObject.name);
                    crossedCheckpoints[indexExpectedCheckpoint] = expectedCheckpoint;
                    indexExpectedCheckpoint = (indexExpectedCheckpoint + 1) % checkpoints.Length;
                    expectedCheckpoint = checkpoints[indexExpectedCheckpoint];
                    lastCheckpointTime = Time.time;
                }
                else
                {
                    Debug.Log("Car crosses the finish line after completing the course");
                    AddReward(5.0f);
                    EndEpisode();
                }




            } else if(other.gameObject.name != expectedCheckpoint.name)
            {

                Debug.Log("Wrong checkpoint!");
                AddReward(-1f);
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
        //foreach (bool checkpointflag in checkpointArray)
        //{
        //    if (!checkpointflag)
        //    {
        //        return false;
        //    }
        //}
        //return true;
        return crossedCheckpoints == checkpoints;
    }

    private void Update()
    {
        if (Time.time - lastCheckpointTime > 300f) // If more than a minute passed since the last checkpoint
        {
            Debug.Log("Time out! More than 5 minutes passed since the last checkpoint.");
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
