using UnityEngine;

public class AllPuzzleCompleteChecker : MonoBehaviour
{
    // References to the LightPuzzleParent & EndDoor GameObjects
    public Transform LightPuzzleParent;
    public GameObject EndDoor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checks if all lights are on
        if (AreAllLightsOn())
        {
            // Send a debug log
            Debug.Log("All light are on");

            // Disable the door
            EndDoor.SetActive(false);
        }
    }

    // Bool function to check if all lights are enabled and thus if all puzzles are completed.
    public bool AreAllLightsOn()
    {
        // Go through all children in the LightPuzzleParent gameobject
        foreach (Transform child in LightPuzzleParent)
        {
            // Log all children names (LightPuzzle1 to LP3)
            //Debug.Log(child.name);

            // Go through all children of all the LightPuzzle gameobjects and specifically grab the Light component
            Light light = child.GetComponentInChildren<Light>();

            // Check if the light exists and if it is disabled
            if (light == null || !light.enabled)
            {
                //Debug.Log("Light is not enabled");

                // Return false is 1 (or more) lights are disabled
                return false;
            }

        }
        // Return true if all lights are enabled
        return true;
    }
}
