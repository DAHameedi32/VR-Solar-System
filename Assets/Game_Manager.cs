using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    GameObject[] celestialObjects;// get all the celestial objects at the start and store them in a celestial objects array:
    GameObject launchSite; //launch planet

    void Start()
    {
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        //set the launch site as earth
        launchSite = celestialObjects[3];
    }

}
