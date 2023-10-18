using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundObstructionManager : MonoBehaviour
{
    [SerializeField] private SoundZone[] soundZoneList;
    [SerializeField] private SoundZone firstZone;

    // Start is called before the first frame update
    void Start()
    {
        firstZone.gameObject.SetActive(true);
    }

    public void SetZoneActive(SoundZone zone)
    {
        SwitchOtherZones(zone.GetNeighbors());
 
    }

    private void SwitchOtherZones(SoundZone[] activeNeighboors)
    {
        for (int i = 0; i < soundZoneList.Length; i++)
        {
            bool foundZone = false;

            //Debug.Log("soundZoneList size: " + soundZoneList.Length);
            for (int j = 0; j < activeNeighboors.Length; j++)
            {
                //Debug.Log("active Neightboors of: " + activeNeighboors[j] + " are: " + activeNeighboors.Length);
                if (activeNeighboors[j] == soundZoneList[i])
                {
                    activeNeighboors[j].gameObject.SetActive(true);
                    foundZone = true;
                    break;
                }
            }

            if (!foundZone)
            {
                soundZoneList[i].gameObject.SetActive(false);
            }
        }
    }

  
}
