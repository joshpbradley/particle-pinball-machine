using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

// Handles the transitions between scenes.
public class SceneTransitionController : MonoBehaviour
{
    // The animator that controls the transition animaions.
    public Animator Transition;
    // The wait time between the start and end transition animations.
    public float TransitionTime;

    private static int s_numberOfScenes;

    private void Awake()
    {
        s_numberOfScenes = SceneManager.sceneCountInBuildSettings;
    }

    // Starts coroutine to load the next scene.
    public void LoadNextScene()
    {

        if(IsLastScene())
        {
            StartCoroutine(LoadScene(0));
        }
        else
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    // Load the scene with the given index.
    IEnumerator LoadScene(int sceneIndex)
    {
        // Trigger the start of the transition.
        Transition.SetTrigger("Start");
        // Wait for the supplied number of seconds.
        yield return new WaitForSeconds(TransitionTime);
        // Destroys all running tween instances between scenes.
        DOTween.KillAll();
        // Load the scene with the supplied index.
        SceneManager.LoadScene(sceneIndex);
    }

    bool IsLastScene()
    {
        return SceneManager.GetActiveScene().buildIndex == s_numberOfScenes - 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            KinectController.GetInstance().CloseSensor();
            PlayerPrefs.DeleteAll();
            StartCoroutine(LoadScene(0));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            KinectController.GetInstance().CloseSensor();
            PlayerPrefs.DeleteAll();

            Application.Quit();

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
