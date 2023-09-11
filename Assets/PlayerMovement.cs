using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float target_rotation = 0f;
    public float rotation_speed = 90f;
    public float rotation_smoothness = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.localPosition += transform.forward * increment;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localPosition -= transform.forward * increment;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localPosition += transform.right * increment;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localPosition -= transform.right * increment;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            target_rotation -= 90f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            target_rotation += 90f;
        }

        Quaternion current_rotation = transform.rotation;
        Quaternion target_quaternion = Quaternion.Euler(0f, target_rotation, 0f);
        transform.rotation = Quaternion.RotateTowards(current_rotation, target_quaternion, rotation_speed * Time.deltaTime * rotation_smoothness);
    }
}
