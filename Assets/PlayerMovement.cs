using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(0.0f, 0.0f, increment);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(0.0f, 0.0f, -increment);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(increment, 0.0f, 0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-increment, 0.0f, 0.0f);
        }
    }
}
