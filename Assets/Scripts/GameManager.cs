using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerXrRig;
    public GameObject BlackCarPrefab;
    public GameObject RedCarPrefab;
    public GameObject BlueCarPrefab;
    public GameObject YellowCarPrefab;
    public GameObject AICarPrefab;
    
    private  Followcar FollowCarScript;
    private int finishLineCounter = 0;
    private bool checkPoint27Crossed = false;

    private void Update()
    {
        if(finishLineCounter > 1)
        {
            Debug.LogWarning("went over 1");
        }
        if(finishLineCounter > 1 && checkPoint27Crossed)
        {
            ResetScene();
        }
    }
    public void CarHover(GameObject hoveredCar)
    {
        //Debug.Log("Hovered over: " + hoveredCar.name);
    }
    public void CarHoverExit(GameObject hoveredCar)
    {
        //Debug.Log("Stopped hovering over: " + hoveredCar.name);
    }
    public void CarSelect(GameObject selectedCar)
    {
        //Debug.Log("Selected: " + selectedCar.name);
        SpawnAiCar();
        SpawnPlayerCar(selectedCar.name);
    }
    public void PlayerCarEntersTrigger(Collider checkpoint)
    {
        if(checkpoint.CompareTag("checkpointstartfinish"))
        {
            finishLineCounter++;
        }
        if(checkpoint.CompareTag("checkpoint27"))
        {
            checkPoint27Crossed = true;
        }
    }
    public void SpawnPlayerCar(string carName)
    {
        GameObject PlayerCar = Instantiate(ReturnCorrectPlayerCarPrefabBySelectedCarName(carName), new Vector3(-84.68f, 1.7f, 90.63f), Quaternion.Euler(1.532f, 89.656f, 0f));
        PlayerXrRig.transform.position = new Vector3(-84.68f, 1.7f, 90.63f);
        FollowCarScript = PlayerXrRig.AddComponent<Followcar>();
        FollowCarScript.car = PlayerCar.transform;
    }
    private void SpawnAiCar()
    {
        GameObject AICar = Instantiate(AICarPrefab, new Vector3(-84.68f, 1.7f, 90.63f), Quaternion.Euler(1.532f, 89.656f, 0f));
    }
    private void ResetScene()
    {
        Debug.LogWarning("finish line counter: " + finishLineCounter);
        Debug.LogWarning("checkpoint 27: " + checkPoint27Crossed);
        Debug.LogWarning("reset");
        SceneManager.LoadScene("SampleScene");
    }
    private GameObject ReturnCorrectPlayerCarPrefabBySelectedCarName(string carName)
    {
        switch (carName)
        {
            case "BlackCar":
                return BlackCarPrefab;
            case "RedCar":
                return RedCarPrefab;
            case "BlueCar":
                return BlueCarPrefab;
            case "YellowCar":
                return YellowCarPrefab;
            default:
                return BlackCarPrefab;
        }
    }
}
