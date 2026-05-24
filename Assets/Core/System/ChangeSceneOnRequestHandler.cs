using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnRequestHandler : MonoBehaviour
{
    [SerializeField]
    private int TargetIndex = 1;
    public void ChangeScene()
    {
        SceneManager.LoadScene(TargetIndex);
    }
}
