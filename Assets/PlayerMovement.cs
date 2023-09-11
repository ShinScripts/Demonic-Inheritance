using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;
    public float rotation_degrees = 90f;

    public GameObject parent;


    bool ClearToMove(bool forward = true)
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, forward ? transform.forward : transform.forward * -1, out hit, increment);

        return !hit.collider;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
<<<<<<< Updated upstream
            transform.localPosition += Vector3.forward * increment;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localPosition += Vector3.back * increment;
=======

            if (ClearToMove())
            {
                transform.localPosition += transform.forward * increment;
            }
            else
            {
                print("obstacle in front");
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (ClearToMove(false))
            {
                transform.localPosition -= transform.forward * increment;
            }
            else
            {
                print("obstacle behind");
            }
>>>>>>> Stashed changes
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
