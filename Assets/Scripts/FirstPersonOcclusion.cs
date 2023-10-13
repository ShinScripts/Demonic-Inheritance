using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FirstPersonOcclusion : MonoBehaviour
{
    [Header("FMOD Event")]
    [SerializeField]
    private EventReference selectAudio;
    private new EventInstance audio;
    private EventDescription audioDes;
    private StudioListener listener;
    private PLAYBACK_STATE pb;

    [Header("Occlusion Options")]
    [SerializeField]
    [Range(0f, 10f)]
    private float soundOcclusionWidening = 1f;
    [SerializeField]
    [Range(0f, 10f)]
    private float playerOcclusionWidening = 1f;

    private static LayerMask occlusionLayer; 
    private static LayerMask obstructionLayer;

    [SerializeField] private bool debugOcclusion = false;

    private bool audioIsVirtual;
    private float minDistance;
    private float maxDistance;
    private float listenerDistance;
    private float lineCastOcclusionHitCount = 0f;
    private float lineCastObstructionHitCount = 0f;
    private Color colour;
    private int obstructionCount;
    private string currentObstruction;
    private string objectsBetweenCount;

    private void Start()
    {
        audioDes = RuntimeManager.GetEventDescription(selectAudio);
        audioDes.getMinMaxDistance(out minDistance, out maxDistance);
        listener = FindObjectOfType<StudioListener>();
        currentObstruction = "hello world";

        occlusionLayer = LayerMask.GetMask(LayerMask.LayerToName(6));
        obstructionLayer = LayerMask.GetMask(LayerMask.LayerToName(7));

        //Debug.Log(occlusionLayer);
    }

    private void FixedUpdate()
    {
        if (audio.isValid())
        {
            audio.isVirtual(out audioIsVirtual);
            audio.getPlaybackState(out pb);
            listenerDistance = Vector3.Distance(transform.position, listener.transform.position);

            if (!audioIsVirtual && pb == PLAYBACK_STATE.PLAYING && listenerDistance <= maxDistance)
            {
                obstructionCount = CalculateObjectsBetween(transform.position, listener.transform.position);
                //currentObstruction = ObjectsBetweenCount(obstructionCount);
                if (debugOcclusion)
                {
                   Debug.Log("Objects between " + gameObject.name + " and listener: " + obstructionCount);
                }

               

                OccludeBetween(transform.position, listener.transform.position);


            }

            lineCastOcclusionHitCount = 0f;
            lineCastObstructionHitCount = 0f;
        }
    }

    private void OccludeBetween(Vector3 sound, Vector3 listener)
    {
        Vector3 SoundLeft = CalculatePoint(sound, listener, soundOcclusionWidening, true);
        Vector3 SoundRight = CalculatePoint(sound, listener, soundOcclusionWidening, false);

        Vector3 SoundAbove = new Vector3(sound.x, sound.y + soundOcclusionWidening, sound.z);
        Vector3 SoundBelow = new Vector3(sound.x, sound.y - soundOcclusionWidening, sound.z);

        Vector3 ListenerLeft = CalculatePoint(listener, sound, playerOcclusionWidening, true);
        Vector3 ListenerRight = CalculatePoint(listener, sound, playerOcclusionWidening, false);

        Vector3 ListenerAbove = new Vector3(listener.x, listener.y + playerOcclusionWidening * 0.5f, listener.z);
        Vector3 ListenerBelow = new Vector3(listener.x, listener.y - playerOcclusionWidening * 0.5f, listener.z);

        CastLine(SoundLeft, ListenerLeft);
        CastLine(SoundLeft, listener);
        CastLine(SoundLeft, ListenerRight);

        CastLine(sound, ListenerLeft);
        CastLine(sound, listener);
        CastLine(sound, ListenerRight);

        CastLine(SoundRight, ListenerLeft);
        CastLine(SoundRight, listener);
        CastLine(SoundRight, ListenerRight);

        CastLine(SoundAbove, ListenerAbove);
        CastLine(SoundBelow, ListenerBelow);

        if (playerOcclusionWidening == 0f || soundOcclusionWidening == 0f)
        {
            colour = Color.blue;
        }
        else
        {
            colour = Color.green;
        }

        SetParameters();
    }

    private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrNeg)
    {
        float x;
        float z;
        float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
        float mn = (m / n);
        if (posOrNeg)
        {
            x = a.x + (mn * (a.z - b.z));
            z = a.z - (mn * (a.x - b.x));
        }
        else
        {
            x = a.x - (mn * (a.z - b.z));
            z = a.z + (mn * (a.x - b.x));
        }
        return new Vector3(x, a.y, z);
    }

    private void CastLine(Vector3 Start, Vector3 End)
    {
        RaycastHit hit;
        Physics.Linecast(Start, End, out hit, occlusionLayer);

        if (hit.collider)
        {
            lineCastOcclusionHitCount++;
            Debug.DrawLine(Start, End, Color.red);
            return;
        }
        
        Physics.Linecast(Start, End, out hit, obstructionLayer);

        if (hit.collider)
        {
            lineCastObstructionHitCount++;
            Debug.DrawLine(Start, End, Color.black);
            return;
        }

        if (!hit.collider)
        {
            Debug.DrawLine(Start, End, colour);
        }

    }

    private void SetParameters()
    {

        SetObstructionParamater();

        if (debugOcclusion)
        {
           // Debug.Log("Free lines: " + (11 - lineCastOcclusionHitCount - lineCastObstructionHitCount) +
           //     " Occlusion lines: " + lineCastOcclusionHitCount + 
           //     " Obstruction lines: " + lineCastObstructionHitCount);
        }
        audio.setParameterByName("Occlusion", lineCastOcclusionHitCount / 11 + 2 * lineCastObstructionHitCount / 11);
    }

    private void OnEnable()
    {
        audio = RuntimeManager.CreateInstance(selectAudio);
        RuntimeManager.AttachInstanceToGameObject(audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
        audio.start();
    }

    private void OnDisable()
    {
        if (audio.isValid())
        {
            audio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            audio.release();
        }
    }

    private int CalculateObjectsBetween(Vector3 start, Vector3 end)
    {
        int objectsBetweenCount = 0;
        RaycastHit[] hits = Physics.RaycastAll(start, (end - start).normalized, Vector3.Distance(start, end), obstructionLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider)
            {
                objectsBetweenCount++;
            }
        }

        return objectsBetweenCount;
    }

    private string GetObstructionName()
    {

        /*  if (lineCastObstructionHitCount <= 0)
          {
              return "NoObstruction";
          }

          else if (lineCastObstructionHitCount < 11)
          {
              return "Obstruction_1_Wall";
          }

          else return "Full_Obstruction"; 

      */

        if (obstructionCount <= 0)
        {
            return "NoObstruction";
        }

        else if (obstructionCount < 2)
        {
            return "Obstruction_1_Wall";
        }

        else return "Full_Obstruction";

    }


    private void SetObstructionParamater()
    {
        string newObstruction = GetObstructionName();


        if (currentObstruction.Equals(newObstruction))
        {
            return;
        }

        else
        {
            currentObstruction = newObstruction;
            audio.setParameterByNameWithLabel("Obstruction", currentObstruction);
        }
    }
}
