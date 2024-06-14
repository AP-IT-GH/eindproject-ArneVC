using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void CarSelect(GameObject selectedCar)
    {
        Debug.Log("Selected: " + selectedCar.name);
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
