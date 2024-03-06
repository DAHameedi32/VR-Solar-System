using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    GameObject[] celestialObjects;// get all the celestial objects at the start and store them in a celestial objects array:
    string[] all_objectives = {"Jupiter", "Saturn", "Uranus", "Neptune"}; //all planetary objectives
    GameObject currentObjective; //current planet to put something in orbit around
    GameObject launchSite; //launch planet

    float[] HillRadii = {
        9.103292413f, //Jupiter
        11.16713507f, //Saturn
        12.01577405f, //Uranus
        19.85178895f}; //Neptune
    // Start is called before the first frame update
    void Start()
    {
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        //sets the first current objective as jupiter:
        foreach (GameObject i in celestialObjects)
        {
            if(i.name == "Jupiter")
            {
                currentObjective = i;
                Debug.Log("Current Objective: " + currentObjective.name);
            }

        } 
        //set the launch site as earth
        launchSite = celestialObjects[3];
    }
    //check to see whether the objective has been completed
    bool CheckObjective(GameObject targ, int index)
    {
        bool return_value = false;
        //We currently know the Hill-Spheres of the planets, so we need to find all satellites, and check the distance to the target planet.
        //since all satellites have mass <<1, I'm hesitant to use the Gind Game Objects with tag method because it's pretty slow, but i'll do it for now to expedite getting the code written
        GameObject[] All_Objects = GameObject.FindGameObjectsWithTag("celestial_body");
        foreach (GameObject i in All_Objects)
        {
            if(i.GetComponent<Rigidbody>().mass <1)
            {
                //if the distance to the target planet is < Hill-radius, then the objective is considered completed, and a new objective is given.
                if((targ.transform.position - i.transform.position).magnitude < HillRadii[index])
                {
                    Debug.Log("Velocity: "+i.GetComponent<Rigidbody>().velocity);
                    Debug.Log("Objective Complete!");
                    return_value = true;
                }
            } 
        }
        return return_value;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //first objective is current objective, and index 0
        if(CheckObjective(currentObjective, 0) == true)
        {
            Debug.Log("Next Objective Loading...");
        }
    }
}
