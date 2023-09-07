using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public bool beenTaken;
    public int generatorNumber;

    void OnTriggerEnter (Collider collision)
    {
        if (!beenTaken && collision.CompareTag("Player"))
        {
            beenTaken = true;
            Debug.Log("Generator " + generatorNumber + " has been taken!");
            GetComponent<AudioSource>().Stop();

        }
    }
}
