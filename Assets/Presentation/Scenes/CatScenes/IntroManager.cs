using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    [Header("Настройки")]
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    private InputAction skipAction;
    private bool isSkipping;

    void OnEnable()
    {
        skipAction = new InputAction("SkipIntro");
        skipAction.AddBinding("<Keyboard>/anyKey");
        skipAction.AddBinding("<Mouse>/leftButton");
        skipAction.AddBinding("<Mouse>/rightButton");

        skipAction.performed += _ => SkipAndLoad();
        skipAction.Enable();
    }

    void OnDisable()
    {
        skipAction?.Disable();
        skipAction?.Dispose();
    }

    void Start()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = true;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void SkipAndLoad()
    {
        if (isSkipping) return;
        isSkipping = true;

        videoPlayer.prepareCompleted -= OnVideoPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.Stop();
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}