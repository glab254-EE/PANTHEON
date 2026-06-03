using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;
    [SerializeField] private GameObject ToEnableForSkipping;
    [SerializeField] private float SkipWaitingDelay = 1;
    private InputAction skipAction;
    private bool isSkipping;
    void Start()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = true;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;

        StartCoroutine(AwaitSkipDelayed());
    }
    void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
    void OnVideoPrepared(VideoPlayer vp)
    {
        if (vp.isPlaying) return;
        vp.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }
    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator AwaitSkipDelayed()
    {
        yield return new WaitForSecondsRealtime(SkipWaitingDelay);
        if (ToEnableForSkipping != null) ToEnableForSkipping.SetActive(true);
        InputSystem.onAnyButtonPress
        .CallOnce(_=>LoadNextScene());
    }
}