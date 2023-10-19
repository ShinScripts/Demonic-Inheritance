using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool beenTaken;
    public string itemName;

    public AudioClip item_collected_clip;
    AudioSource source;
    AudioSource sourceCollected;
    private float initialDistance;
    [SerializeField] private float TargetDistanceMultiplier = 3.5f;

    private ItemManager itemManager;

    private void Awake() {
        itemManager = FindObjectOfType<ItemManager>();
        source = GetComponent<AudioSource>();
        sourceCollected = GetComponent<AudioSource>();
        initialDistance = source.maxDistance;

        if (itemManager == null) Debug.LogWarning("No items allowed without ItemManager!");
        if (item_collected_clip == null) Debug.LogError("ERROR: Need Item Collected Clip in order to work");
    }

    void OnTriggerEnter (Collider collision)
    {
        if (!beenTaken && collision.CompareTag("Player"))
        {
            beenTaken = true;
            Debug.Log("Item " + itemName + " has been taken!");

            StartCoroutine(PlaySound());
            itemManager.ItemTaken(this);
        }
    }

    IEnumerator PlaySound() {
        source.Stop();
        sourceCollected.PlayOneShot(item_collected_clip);
        yield return new WaitForSeconds(item_collected_clip.length);
        sourceCollected.Stop();
    }

    //Adjust Volume max distance if item is target.
    public void AdjustVolumeRange(bool isTarget) {
        if(isTarget) {
            source.maxDistance = initialDistance * TargetDistanceMultiplier;
        }
    }
}
