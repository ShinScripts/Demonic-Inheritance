using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Put me on player with camera

public class DebugController : MonoBehaviour
{
    public Canvas canvas;

    private bool isShiftPressed = false;

    //DEV Cheats
    [Header("Cheats")]
    public bool isSceneVisible;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (canvas == null) {
            Debug.Log("No canvas in player found");
        }

        canvas.enabled = !isSceneVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            isShiftPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            isShiftPressed = true;
        }

        if (isShiftPressed && Input.GetKeyDown(KeyCode.Alpha1)) { 
            isSceneVisible = !isSceneVisible;
            canvas.enabled = !isSceneVisible;
            Debug.Log("CHEAT: SceneVisibility: " + canvas.enabled);
        }
    }
}
