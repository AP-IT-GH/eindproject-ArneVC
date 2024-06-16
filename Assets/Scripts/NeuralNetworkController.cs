using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.IO;
using System;

public class NeuralNetworkController : Agent
{
    public CarController carController;
    private bool[] checkpointArray = new bool[126];
    private int currentCheckpointIndex = 0;
    private float lastCheckpointTime;
    private Vector3 lastPosition;
    private float timeSinceLastCheck;
    private string actionFilePath = "Assets/ImitationData/output_data.csv"; // Path to action CSV file

    private DateTime lastReadTime;
    private FileSystemWatcher fileWatcher;

    public override void Initialize()
    {
        carController = GetComponent<CarController>();
        lastReadTime = File.GetLastWriteTime(actionFilePath);
        
        // Initialize file watcher to monitor changes in the action CSV file
        fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(actionFilePath));
        fileWatcher.Filter = Path.GetFileName(actionFilePath);
        fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
        fileWatcher.Changed += OnActionFileChanged;
        fileWatcher.EnableRaisingEvents = true;
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
    public override void OnActionReceived(ActionBuffers actions)
    {
        //float moveInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 0f);
        float throttle = -1.0f;
        float steer = 0f;
        //AddReward(-0.01f); // Time penalty, encourage the car to move faster
        Vector3 position = carController.transform.position;
        Quaternion rotation = carController.transform.rotation;
        Vector3 velocity = carController.carRb.velocity;
        float moveInput = -1.0f;
        carController.MoveInput(moveInput);
        LogData(position, rotation, velocity, steer, throttle);
        
        
    }
    private void OnActionFileChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType == WatcherChangeTypes.Changed)
        {
            Debug.Log("Action file change detected: " + e.FullPath);

            // Check if the file was modified after last read
            DateTime lastWriteTime = File.GetLastWriteTime(actionFilePath);
            if (lastWriteTime > lastReadTime)
            {
                // Read new actions since last read
                ReadNewActions();
                lastReadTime = lastWriteTime;
            }
        }
    }

    private void ReadNewActions()
    {
        if (File.Exists(actionFilePath))
        {
            using (StreamReader reader = new StreamReader(actionFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    if (values.Length >= 2)
                    {
                        string actionString = values[1]; // Assuming action is in the second column
                        // Convert actionString to appropriate action for your agent
                        // Trigger action in your agent's code here
                        Debug.Log("Triggering action: " + actionString);
                        // Example: PerformAction(actionString);
                        float moveInput = -1.0f;
                        float steerInput = float.Parse(actionString);
                        carController.MoveInput(moveInput);
                        carController.SteerInput(steerInput);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Action CSV file not found!");
        }
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
}
