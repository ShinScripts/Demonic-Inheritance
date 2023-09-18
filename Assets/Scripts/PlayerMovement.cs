using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float target_rotation = 0f;
    public float rotation_speed = 90f;
    public float rotation_smoothness = 5f;

    private bool isMoving = false;

    public SoundSource sourceFront;
    public SoundSource sourceBack;
    public AudioSource sourceCenter;

    [Space(10)]
    [Header("~SoundClips")]
    public AudioClip[] footsteps_clips;
    public AudioClip[] rotations_clips;
    public AudioClip[] wall_bump_clips; //front array[0], back array[1]
    
    [Space(10)]
    public float maxSoundPitch = 1.2f;
    public float minSoundPitch = 0.8f;

    private void Start() {
        if (footsteps_clips == null) Debug.Log("ERROR: No footsteps clips");
        if (rotations_clips == null) Debug.Log("ERROR: No rotation clips");
        if (wall_bump_clips == null) Debug.Log("ERROR: No wall bump clips");
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
            return;

        // ** ROTATION **
        if (Input.GetKeyDown(KeyCode.A)) {
            target_rotation -= 90f;
            PlayRotation();
        } else if (Input.GetKeyDown(KeyCode.D)) {
            target_rotation += 90f;
            PlayRotation();
        }

        Quaternion current_rotation = transform.rotation;
        Quaternion target_quaternion = Quaternion.Euler(0f, target_rotation, 0f);
        transform.rotation = Quaternion.RotateTowards(current_rotation, target_quaternion, rotation_speed * Time.deltaTime * rotation_smoothness);

        //Don't continue unless rotation is done.
        if (Quaternion.Angle(current_rotation, target_quaternion) != 0f) {
            return;
        }

        // ** MOVEMENT **
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (ClearToMove())
            {
                transform.localPosition += transform.forward * increment;
                StartCoroutine(PlayFootsteps());
            }
            else
            {
                print("obstacle in front");
                float pitch = Random.Range(minSoundPitch, maxSoundPitch);
                //front array[0], back array[1]
                sourceFront.PlayOneShot(wall_bump_clips[0], pitch);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (ClearToMove(false))
            {
                transform.localPosition -= transform.forward * increment;
                StartCoroutine(PlayFootsteps());
            }
            else
            {
                print("obstacle behind");
                float pitch = Random.Range(minSoundPitch, maxSoundPitch);
                //front array[0], back array[1]
                sourceBack.PlayOneShot(wall_bump_clips[1], pitch);
            }
        }

    }

    // ** SOUND ***
    private void PlayRotation() {
        float rotationVolume = 4f;
        int clip = Random.Range(0, rotations_clips.Length);
        sourceCenter.pitch = Random.Range(0.75f, 1.05f);
        sourceCenter.clip = rotations_clips[clip];
        sourceCenter.PlayOneShot(sourceCenter.clip, rotationVolume);
    }

    IEnumerator PlayFootsteps()
    {
        float footSteepVolume = 2f;

        for (int i = 0; i < 3; i++)
        {
            isMoving = true;
            RandomizeFootstep();
            sourceCenter.PlayOneShot(sourceCenter.clip, footSteepVolume);
            yield return new WaitForSeconds(sourceCenter.clip.length + Random.Range(0.07f, 0.13f));
            isMoving = false;
        }
    }

    private void RandomizeFootstep() {
        int clip = Random.Range(0, footsteps_clips.Length);
        sourceCenter.pitch = Random.Range(0.8f, 1.1f);
        sourceCenter.clip = footsteps_clips[clip];

    }

}
