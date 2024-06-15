using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject BlackCarPrefab;
    public GameObject RedCarPrefab;
    public GameObject BlueCarPrefab;
    public GameObject YellowCarPrefab;
    public GameObject AICarPrefab;
    public void CarHover(GameObject hoveredCar)
    {
        Debug.Log("Hovered over: " + hoveredCar.name);
    }
    public void CarHoverExit(GameObject hoveredCar)
    {
        Debug.Log("Stopped hovering over: " + hoveredCar.name);
    }
    public void CarSelect(GameObject selectedCar)
    {
        Debug.Log("Selected: " + selectedCar.name);
        SpawnAiCar();
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
    private void SpawnAiCar()
    {
        GameObject AICar = Instantiate(AICarPrefab, new Vector3(-84.68f, 1.7f, 90.63f), Quaternion.identity);
    }
    private void ResetScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
