using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgentdemo : Agent
{
    private List<float[]> actions = new List<float[]>();
    private int currentActionIndex = 0;
    public CarController carController;
    private bool[] checkpointArray = new bool[126];
    private int currentCheckpointIndex = 0;
    private float lastCheckpointTime;
    private Vector3 lastPosition;
    private float timeSinceLastCheck;
    public float actionDelay = 0.02f; // Adjust as needed
    private bool isTakingActions = true;
    private float steerInput;
    void Start()
    {
        // Read CSV file and store actions
        string path = "Assets/ImitationData/training_data2.csv"; // Update with your actual file path
        StreamReader reader = new StreamReader(path);
        reader.ReadLine();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            float[] action = new float[1];
            
            action[0] = float.Parse(values[10]);
            actions.Add(action);
        }

        reader.Close();
    }
    public override void Initialize()
    {
        carController = GetComponent<CarController>();
    }
    public override void OnEpisodeBegin()
    {
        carController.ResetCar();
        checkpointArray = new bool[126];
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
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int action = actionBuffers.DiscreteActions[0];
        switch (action)
    {
        case 0:
            // Perform action 0 (e.g., move forward)
            steerInput = 0.0f;
            break;
        case 1:
            // Perform action 1 (e.g., turn left)
            steerInput = -1.0f;
            break;
        case 2:
            // Perform action 2 (e.g., turn right)
            steerInput = 1.0f;
            break;
        default:
            // Handle unexpected action index
            Debug.LogError("Unexpected action index received: " + action);
            break;
    }
        //float moveInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 0f);
        //float steerInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float moveInput = -1.0f;
        carController.MoveInput(moveInput);
        carController.SteerInput(steerInput);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions.Clear();
        float steer;
        if (-1.0f == actions[currentActionIndex][0])
        {
            discreteActions[0] = 1; // Action index 1: Turn left
            steer = -1.0f;
        }
        else if (1.0f == actions[currentActionIndex][0])
        {
            discreteActions[0] = 2; // Action index 2: Turn right
            steer = 1.0f;
        }
        else if (0.0f == actions[currentActionIndex][0])
        {
            discreteActions[0] = 0; // Action index 0: Move forward
            steer = 0.0f;
        }
        currentActionIndex++;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Checkpoint ("+currentCheckpointIndex+")")
        {
            checkpointArray[currentCheckpointIndex] = true;
            AddReward(0.1f);
            Debug.Log("Checkpoint " + currentCheckpointIndex + " achieved");
            lastCheckpointTime = Time.time;

            currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpointArray.Length;

            if (currentCheckpointIndex == 1 && DidTheCarCollectAllCheckpoints())
            {
                Debug.Log("Car crosses the finish line after completing the course");
                AddReward(5.0f);
                // recorder.Record = false;
                EndEpisode();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("invBarrier"))
        {
            AddReward(-1f);
            //Debug.Log("Car touched wall");
            // recorder.Record = false;
            EndEpisode();
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
            // recorder.Record = false;
            EndEpisode();
        }
        
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck > 3f)
        {
            float distanceMoved = Vector3.Distance(carController.transform.position, lastPosition);
            if (distanceMoved <= 0.1f)
            {
                //Debug.Log("Time out! Car is stuck.");
                AddReward(-1f);
                timeSinceLastCheck = 0f;
                // recorder.Record = false;
                EndEpisode();
            }
            timeSinceLastCheck = 0f;
            lastPosition = carController.transform.position;
        }
        
    }
    private void LogData(Vector3 position, Quaternion rotation, Vector3 velocity, float steer, float throttle)
    {
        // Specify the file path
        string directoryPath = Path.Combine(Application.dataPath, "ImitationData/");
        string filePath = Path.Combine(directoryPath, "training_data.csv");

        // Ensure the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Format the data to be logged
        string data = $"{position.x},{position.y},{position.z}," +
                      $"{rotation.x},{rotation.y},{rotation.z},{rotation.w}," +
                      $"{velocity.x},{velocity.y},{velocity.z}," +
                      $"{steer},{throttle},\n";

        // Append the data to the CSV file
        File.AppendAllText(filePath, data);
    }
    IEnumerator WaitForNextAction()
    {
        // Prevent taking actions while waiting
        isTakingActions = false;

        // Wait for the specified action delay
        yield return new WaitForSeconds(actionDelay);

        // Allow taking actions again
        isTakingActions = true;
    }
}
