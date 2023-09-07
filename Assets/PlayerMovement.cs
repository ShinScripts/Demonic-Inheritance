using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float current_rotation = 0f;
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

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            current_rotation -= 90f;
            parent.transform.rotation = Quaternion.Euler(new Vector3(0f, current_rotation, 0f));
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            current_rotation += 90f;
            parent.transform.rotation = Quaternion.Euler(new Vector3(0f, current_rotation, 0f));
        }
    }
}
