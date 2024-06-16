using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.IO;

public class NeuralNetworkController : MonoBehaviour
{
    public string pythonScript  = "Assets/Scripts/predict_steering.py"; // Adjust the path to your Python script

    private Process pythonProcess;
    void Update()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "python";
        startInfo.Arguments = pythonScript;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        
        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        // Wait for the process to finish
        process.WaitForExit();

        // Read the output
        string output = process.StandardOutput.ReadToEnd();
        
        UnityEngine.Debug.Log("Python script output: " + output);
    }
}
