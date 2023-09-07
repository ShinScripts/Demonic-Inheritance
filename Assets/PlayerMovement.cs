using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;
    public float rotation_degrees = 90f;

    public GameObject parent;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.localPosition += Vector3.forward * increment;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localPosition += Vector3.back * increment;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localPosition += Vector3.right * increment;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localPosition += Vector3.left * increment;
        }
    }
}
