using System.Collections;
using TMPro;
using UnityEngine;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float target_rotation = 0f;
    public float rotation_speed = 90f;
    public float rotation_smoothness = 5f;
    [SerializeField] private float movementSpeed = 3.3f;
    private float movementTime;

    public bool isMoving = false;
    private bool isRotating = false;

    private float movementStartTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    [SerializeField] private PlayerAudioManager playerAudioManager;

    public bool IsBusy { get => (isMoving || isRotating); }

    private void Start()
    {
        movementTime = 1 / movementSpeed;
    }

    private bool ClearToMove(bool forward = true)
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, forward ? transform.forward : transform.forward * -1, out hit, increment);

        return !(hit.collider && !hit.collider.CompareTag("Generator"));
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlayer();
        }
        else if (isRotating)
        {
            RotatePlayer();
        }
        else
        {
            HandleInput();
        }
    }

    private void MovePlayer()
    {
        // Lerping position
        float t = Mathf.Clamp01((Time.time - movementStartTime) / movementTime);
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1.0f)
        {
            isMoving = false;
        }
    }

    private void RotatePlayer()
    {
        // Lerping rotation
        float t = Mathf.Clamp01((Time.time - movementStartTime) / movementTime);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        if (t >= 1.0f)
        {
            isRotating = false;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            if (ClearToMove())
            {
                StartCoroutine(PlayFootsteps());
                StartMovement(transform.forward);
            }
            else
            {
                print("obstacle in front");
                playerAudioManager.PlayWallHitSoundFront();

            }
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isMoving)
        {
            if (ClearToMove(false))
            {
                StartCoroutine(PlayFootsteps());
                StartMovement(-transform.forward);
            }
            else
            {
                print("obstacle behind");
                playerAudioManager.PlayWallHitSoundBack();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !isRotating)
        {
            StartRotation(-90f);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isRotating)
        {
            StartRotation(90f);
        }
    }

    private void StartMovement(Vector3 direction)
    {
        isMoving = true;
        targetPosition = transform.localPosition + direction * increment;
        movementStartTime = Time.time;
        startPosition = transform.localPosition;
    }

    private void StartRotation(float rotation)
    {
        isRotating = true;
        target_rotation += rotation;
        startRotation = transform.rotation;
        movementStartTime = Time.time;
        targetRotation = Quaternion.Euler(0f, target_rotation, 0f);
    }


    /* public void RandomizeFootstep()
     {
         int clip = Random.Range(0, footsteps.Length);
         sourceCenter.pitch = Random.Range(0.8f, 1.1f);
         sourceCenter.clip = footsteps[clip];

     }
    */

    IEnumerator PlayFootsteps()
    {
        isMoving = true;
        //playerAudioManager.PlayFootstep();
        yield return new WaitForSeconds(movementTime);
        isMoving = false;
    }

    /* IEnumerator PlayFootsteps()
    {
        for (int i = 0; i < 3; i++)
        {
            isMoving = true;
            RandomizeFootstep();
            sourceCenter.PlayOneShot(sourceCenter.clip);
            yield return new WaitForSeconds(sourceCenter.clip.length + Random.Range(0.125f, 0.17f));
            isMoving = false;
        }
    }
    */
}
