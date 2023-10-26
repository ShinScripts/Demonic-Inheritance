using System.Collections;
using TMPro;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using FMOD.Studio;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float target_rotation = 0f;
    public float rotation_speed = 90f;
    public float rotation_smoothness = 5f;
    [SerializeField] private float movementSpeed = 3.3f;
    private float movementTime;

    [SerializeField] private Transform audioFrontPosition;
    [SerializeField] private Transform audioBehindPosition;
    [SerializeField] private Transform audioLeftPosition;
    [SerializeField] private Transform audioRightPosition;

    [SerializeField] private SoundObstructionManager soundObstructionManager;

    private SoundZone lastZoneChecked;
    private bool isMoving = false;
    private bool isRotating = false;

    private Vector3 lastGeneratorPosition;

    public EventInstance lastSoundTriggerReference;

    private float movementStartTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    private string currentSoundZoneName;

    [SerializeField] private string[] obstacleTags;

    [SerializeField] private PlayerAudioManager playerAudioManager;

    public bool IsBusy { get => (isMoving || isRotating); }
    public string CurrentSoundZoneName { get => currentSoundZoneName; }
    public bool CanMove { get => canMove; set => canMove = value; }

    private string currentObstacle;
    private float rightDirection;
    public bool is_dead;
    private bool canMove = true;

    private void Start()
    {
        movementTime = 1 / movementSpeed;

        currentObstacle = string.Empty;

        lastGeneratorPosition = transform.position;
    }

    private bool ClearToMove(string direction)
    {
        RaycastHit hit;

        switch (direction)
        {
            case "forward":
                if (Physics.Raycast(transform.position, transform.forward, out hit, increment))
                {
                    currentObstacle = hit.collider.tag;

                    //Debug.Log(currentObstacle);
                    return !IsObstacleTag(currentObstacle);
                }
                break;
            case "backward":
                if (Physics.Raycast(transform.position, transform.forward * -1f, out hit, increment))
                {
                    currentObstacle = hit.collider.tag;

                    //Debug.Log(currentObstacle);
                    return !IsObstacleTag(currentObstacle);
                }
                break;
            case "left":
                if (Physics.Raycast(transform.position, transform.right * -1f, out hit, increment))
                {
                    currentObstacle = hit.collider.tag;

                    //Debug.Log(currentObstacle);
                    return !IsObstacleTag(currentObstacle);
                }
                break;
            case "right":
                if (Physics.Raycast(transform.position, transform.right, out hit, increment))
                {
                    currentObstacle = hit.collider.tag;

                    //Debug.Log(currentObstacle);
                    return !IsObstacleTag(currentObstacle);
                }
                break;
        }

        currentObstacle = string.Empty;
        return true;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward);
    }

    private void Update()
    {
        print(Input.GetAxis("Xbox Rotate"));
        if (is_dead)
        {
            //dead

            return;
        }

        if (!CanMove)
        {
            //is locked to move
            return;
        }

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
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Vertical C") < -0.5f) && !isMoving)
        {
            if (ClearToMove("forward"))
            {
                playerAudioManager.PlayFootstep("Move");
                StartCoroutine(FootstepsDelay());
                StartMovement(transform.forward);
            }
            else
            {
                print("obstacle in front");
                playerAudioManager.PlayWallHitSound(currentObstacle, audioFrontPosition, "Front");
            }
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical C") > 0.5f) && !isMoving)
        {
            if (ClearToMove("backward"))
            {
                playerAudioManager.PlayFootstep("Move");
                StartCoroutine(FootstepsDelay());
                StartMovement(-transform.forward);
            }
            else
            {
                print("obstacle behind");
                playerAudioManager.PlayWallHitSound(currentObstacle, audioBehindPosition, "Back");
            }
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal C") < -0.5f) && !isMoving)
        {
            if (ClearToMove("left"))
            {
                playerAudioManager.PlayFootstep("Move");
                StartCoroutine(FootstepsDelay());
                StartMovement(-transform.right);
            }
            else
            {
                print("obstacle to the left");
                playerAudioManager.PlayWallHitSound(currentObstacle, audioLeftPosition, "Left");
            }
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal C") > 0.5f) && !isMoving)
        {
            if (ClearToMove("right"))
            {
                playerAudioManager.PlayFootstep("Move");
                StartCoroutine(FootstepsDelay());
                StartMovement(transform.right);
            }
            else
            {
                print("obstacle to the right");
                playerAudioManager.PlayWallHitSound(currentObstacle, audioRightPosition, "Right");
            }
        }

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Xbox Rotate") < -0.5f) && !isRotating)
        {
            playerAudioManager.PlayFootstep("Rotate");
            StartRotation(-90f);
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Xbox Rotate") > 0.5f) && !isRotating)
        {
            playerAudioManager.PlayFootstep("Rotate");
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

    IEnumerator FootstepsDelay()
    {
        isMoving = true;
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

    private bool IsObstacleTag(string tag)
    {
        for (int i = 0; i < obstacleTags.Length; i++)
        {
            if (obstacleTags[i].Equals(tag))
                return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SoundZones"))
        {
            currentSoundZoneName = other.tag;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("DirectionZones"))
        {
            rightDirection = other.GetComponent<RightOrientation>().orientation;
        }

    }

    public void SetCheckPoint(Vector3 position)
    {
        lastZoneChecked = soundObstructionManager.CurrentActiveZone;
        lastGeneratorPosition = position;
    }

    public string GetCorrentDirection()
    {

        print(rightDirection);


        float playerRotation = transform.rotation.eulerAngles.y < 0 ? transform.eulerAngles.y + 360f : transform.eulerAngles.y;

        float direction = (rightDirection - playerRotation >= 0) ? rightDirection - playerRotation : rightDirection - playerRotation + 360f;

        print(direction);

        switch (direction)
        {
            case 0:
                return "Front";
            case 90f:
                return "Right";
            case 180f:
                return "Back";
            case 270f:
                return "Left";
            default:
                return "Front";

        }
        
    }

    public void Respawn()
    {
        FreezePlayer();
        gameObject.transform.position = lastGeneratorPosition;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        soundObstructionManager.SetZoneActive(lastZoneChecked);
        canMove = true;
        is_dead = false;
    }

    public void FreezePlayer()
    {
        print("it will respawn at: " + lastGeneratorPosition);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

}
