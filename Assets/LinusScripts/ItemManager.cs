using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    List<GameObject> items = new List<GameObject>();
    GameObject[] itemArray;
    GameObject current_item;

    // Start is called before the first frame update
    void Start()
    {
        itemArray = GameObject.FindGameObjectsWithTag("Item");
        if (itemArray == null) Debug.LogWarning("No Items placed on map! Place Items or remove ItemManager!");
        items.AddRange(itemArray);
        items[0].GetComponent<Item>().AdjustVolumeRange(true);
    }

    private void Update() {
        // Check if the current item has been taken and remove it if necessary
        if (current_item != null && current_item.GetComponent<Item>().beenTaken) {
            // Remove the current item from the list
            items.Remove(current_item);
        }

        //TODO: Continue working on shit after items have collected.
        if(items.Count == 0) {
            Debug.Log("All Items collected");
        }

    }


    public void ItemTaken(Item takenItem) {
        GameObject takenObject = takenItem.gameObject;

        // Remove the taken item from the list
        items.Remove(takenObject);
        if(items.Count == 0) {

        }

        foreach(GameObject go in items) {
            if(go == items[0]) {
                go.GetComponent<Item>().AdjustVolumeRange(true);
            } else {
                go.GetComponent<Item>().AdjustVolumeRange(false);
            }
        }
    }
}
