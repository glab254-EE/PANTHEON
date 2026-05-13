using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [field: SerializeField]
    private List<PlayerAttributeClass> playerAttributes;
    [field: SerializeField]
    private List<PlayerStatSO> playerCurrencies;
    public static Bootstrapper Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerStatisticsManager.SetUp(playerAttributes, playerCurrencies);
    }
    /*
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnSceneLoading()
    {

    }*/
}
