using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        NO_MONSTER,
        MONSTER_PRESENT,
        LEVEL_COMPLETE,
        LEVEL_FAILED
    }
    
    public static GameManager Instance;

    public GameState State { get; private set; } = GameState.NO_MONSTER;               // Current Game Status Indicator

    [Header("Properties")]
    [SerializeField] private int maxLevel = 3;                                             // Maximum Level Amount

    void OnEnable()
    {
        GameEvents.Instance.OnLevelFailed += GameOver;
    }

    void OnDisable()
    {
        GameEvents.Instance.OnLevelFailed -= GameOver;
    }

    #region Singleton
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    /// <summary>
    /// Sets the Game Status
    /// </summary>
    /// <param name="value"></param>
    public void SetGameState(GameState value)
    {
        State = value;
    }

    /// <summary>
    /// Resets Entire Level
    /// </summary>
    public void RestartGame()
    {        
        // Check if Level is Complete
        // If so, Increment the Current Level
        if (State == GameState.LEVEL_COMPLETE)
            PlayerManager.Instance.PlayerData.CurrentLevel++;

        if (PlayerManager.Instance.PlayerData.CurrentLevel < maxLevel)
        {
            // Load All Necessary Scenes for Gameplay
            string[] scenes = { "GameScene", "GameUIScene", "Level" + PlayerManager.Instance.PlayerData.CurrentLevel };

            // Load the Scenes with Fade In Transition
            SceneLoader.Instance.LoadScene(scenes, SceneLoader.LoadingStyle.FADE_IN);
        }
        else
        {
            // Load All Necessary Scenes for Gameplay
            string[] scenes = { "EndingScene" };

            // Load the Scenes with Fade In Transition
            SceneLoader.Instance.LoadScene(scenes, SceneLoader.LoadingStyle.FADE_IN);
        }
    }

    void GameOver()
    {
        SetGameState(GameState.LEVEL_FAILED);
        PanelManager.Instance.ActivatePanel("");
    }
}
