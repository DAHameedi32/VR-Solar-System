using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile_Controller : MonoBehaviour
{
    //Game Object for each thruster.
    public float Thrust;

    //finds all projectiles in the scene
    List<GameObject> FindProjectiles()
    {
        List<GameObject> Projectiles = new List<GameObject>();
        GameObject[] CelestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        foreach (GameObject i in CelestialObjects)
        {
            if(i.GetComponent<Rigidbody>().mass < 0.1)
            {
                Projectiles.Add(i);
            }
        }
        return Projectiles;
    }
    public void ForwardButtonPressed()
    {
        List<GameObject> Projectiles = FindProjectiles();
        foreach (GameObject a in Projectiles)
        {
            a.GetComponent<Rigidbody>().AddForce(a.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(a.GetComponent<Transform>().forward * -1).normalized* Thrust, ForceMode.Impulse);
        }
    }
    public void BackwardButtonPressed()
    {
        List<GameObject> Projectiles = FindProjectiles();
        foreach (GameObject a in Projectiles)
        {
            a.GetComponent<Rigidbody>().AddForce(a.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(a.GetComponent<Transform>().forward).normalized* Thrust, ForceMode.Impulse);
        }
    }
    public void RightButtonPressed()
    {
        List<GameObject> Projectiles = FindProjectiles();
        foreach (GameObject a in Projectiles)
        {
            a.GetComponent<Rigidbody>().AddForce(a.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(a.GetComponent<Transform>().right * -1).normalized* Thrust, ForceMode.Impulse);
        }
    }
    public void LeftButtonPressed()
    {
        List<GameObject> Projectiles = FindProjectiles();
        foreach (GameObject a in Projectiles)
        {
            a.GetComponent<Rigidbody>().AddForce(a.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(a.GetComponent<Transform>().right).normalized* Thrust, ForceMode.Impulse);
        }
    }
    public void UpButtonPressed()
    {
        List<GameObject> Projectiles = FindProjectiles();
        foreach (GameObject a in Projectiles)
        {
            a.GetComponent<Rigidbody>().AddForce(a.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(a.GetComponent<Transform>().up * -1).normalized* Thrust, ForceMode.Impulse);
        }
    }
    public void DownButtonPressed()
    {
        List<GameObject> Projectiles = FindProjectiles();
        foreach (GameObject a in Projectiles)
        {
            a.GetComponent<Rigidbody>().AddForce(a.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(a.GetComponent<Transform>().up).normalized * Thrust, ForceMode.Impulse);
        }
    }
}
