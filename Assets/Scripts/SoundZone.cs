using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class SoundZone : MonoBehaviour { 

    [SerializeField] private SoundZone[] activeZones;

    private void Start ()
    {
        gameObject.SetActive(false);
        AddActiveZone(this);

        for (int i = 0; i < activeZones.Length; i++)
        {
            if (!activeZones[i].Contains(this)) {
                activeZones[i].AddActiveZone(this);
            }
        }
    }

    private void AddActiveZone (SoundZone neighbor)
    {
        if(neighbor != null) 
        {
            Array.Resize<SoundZone>(ref activeZones, activeZones.Length + 1);
            activeZones[activeZones.Length - 1] = neighbor;
        }
    }

    public SoundZone[] GetNeighbors()
    {
        return activeZones;
    }

    public bool Contains(SoundZone zone)
    {
        for(int i = 0; i < activeZones.Length; i++)
        {
            if (activeZones[i] == zone)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Debug.Log("This zone is active: " + gameObject.name);
            gameObject.GetComponentInParent<SoundObstructionManager>().SetZoneActive(this);
        }
    }


}
