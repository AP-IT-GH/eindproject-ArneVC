using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void CarHover(GameObject hoveredCar)
    {
        Debug.Log("Hovered over: " + hoveredCar.name);
    }
    public void CarSelect(GameObject selectedCar)
    {
        Debug.Log("Selected: " + selectedCar.name);
    }
}
