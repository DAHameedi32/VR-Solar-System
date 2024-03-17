using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purge : MonoBehaviour
{
    //On Button Pressed:
    public void ButtonPressed()
    {
        // Get all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Iterate over each GameObject found
        foreach (GameObject obj in allObjects)
        {
            // Check if the name matches "Projectile(Clone)"
            if (obj.name == "Projectile 1(Clone)")
            {
                // Destroy the GameObject
                Destroy(obj);
            }
        }

    }
}
