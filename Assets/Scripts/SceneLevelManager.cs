using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLevelManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private FirstPersonOcclusion[] audioSources;
    private int sceneIndexToLoad;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.CanMove = false;
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        foreach (FirstPersonOcclusion source in audioSources) {
            source.AudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndexToLoad);
        yield return null;
    }
}
