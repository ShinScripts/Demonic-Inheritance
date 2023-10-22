using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DoorSoundTrigger : MonoBehaviour
{
    [SerializeField] private EventReference OpenDoorDirectionalEvent;
    [SerializeField] private EventReference EnemyOpenDoorEvent;
    private EventInstance soundTriggerIstance;
    private EventInstance enemySoundTriggerIstance;
    private Vector3 playerEnterPosition;
    private Vector3 playerExitPosition;

    void Start()
    {
        soundTriggerIstance = RuntimeManager.CreateInstance(OpenDoorDirectionalEvent);
        enemySoundTriggerIstance = RuntimeManager.CreateInstance(EnemyOpenDoorEvent);
        enemySoundTriggerIstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemySoundTriggerIstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(other.transform.position));
            enemySoundTriggerIstance.start();
        }

        if (other.CompareTag("Player"))
        {
            playerEnterPosition = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerExitPosition = other.transform.position;
            string result = SoundDirection(GetEnterDirection(), GetPlayerDirection(other));
            soundTriggerIstance.setParameterByNameWithLabel("Direction", result);
            //Debug.Log(result);
            soundTriggerIstance.start();

        }

    }

    private Vector3 GetPlayerDirection(Collider other)
    {
        float playerRotation = other.transform.rotation.eulerAngles.y < 0 ? other.transform.eulerAngles.y + 360f : other.transform.eulerAngles.y;

        switch (playerRotation)
        {

            case 0f:        //Player is facing foward;
                return Vector3.forward;

            case 90f:       //Player is facing right
                return Vector3.right;

            case 180f:      //Player is facing backwards
                return Vector3.back;

            case 270f:      //Player is facing left
                return Vector3.left;

            default:
                return Vector3.zero;
        }

    }

    private Vector3 GetEnterDirection()
    {
        float deltaX = Mathf.Round(playerEnterPosition.x - playerExitPosition.x);
        float deltaZ = Mathf.Round(playerEnterPosition.z - playerExitPosition.z);

        if (Mathf.Approximately(deltaX, 0f))
        {
            if (deltaZ < 0f)
            {
                return Vector3.forward;
            }

            else
            {
                return Vector3.back;
            }
        }

        else
        {
            if (deltaX > 0f)
            {
                return Vector3.left;
            }

            else
            {
                return Vector3.right;
            }
        }
    }

    private string SoundDirection(Vector3 enterDirection, Vector3 playerDirection)
    {
        float direction = Vector3.Dot(enterDirection, playerDirection);

        switch (direction)
        {
            case -1f:
                return "BackToBack";
            case 1f:
                return "FrontToBack";
            case 0f:
                float side = Vector3.Cross(enterDirection, playerDirection).y;
                if (side < 0f)
                {
                    return "RightToLeft";
                }
                else
                {
                    return "LeftToRight";
                }
            default:
                return "";
        }
    }
}
