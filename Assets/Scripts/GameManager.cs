using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton 實例
    public static GameManager Instance { get; private set; }

    // 遊戲狀態或數據
    public bool isDialogueButtonIsOn = false;

    //遊戲時間數據
    public int currentDay ;
    public float timeHour ;
    
    //遊戲狀態
    [HideInInspector]
    public enum GameState{MainMenu,InGame,SettingMenu,BattleUI,EyeCatchDialogue}
    public GameState currentGameState = GameState.InGame;
    public GameState originalGameState = GameState.InGame;

    //主角資訊
    public string playerName = "江東升";
    public Sprite playerSprite;
    public Sprite playerSpriteAngry;
    public Sprite playerSpriteSad;
    public Sprite playerSpriteHappy;
    public Sprite playerSpriteSurprised;
    public Sprite playerSpriteThinking;
    public Sprite playerPortrait;
    public Sprite playerPortraitAngry;
    public Sprite playerPortraitSad;
    public Sprite playerPortraitHappy;
    public Sprite playerPortraitSurprised;
    public Sprite playerPortraitThinking;

    //現在場景
    private Scene currentScene;
    [HideInInspector]
    public string currentSceneName;


    private void Awake()
    {
        // 確保 Singleton 的唯一性
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //初始化遊戲狀態
        currentDay = 1;

        //取得目前的活動場景
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        if(currentScene.name == "MainMenu")
        {currentGameState = GameState.MainMenu;}

        //切換背景音樂
        SwitchBGM(currentSceneName);
        
    }

    public void PauseGame()
    {
        originalGameState = currentGameState;
        //暫停遊戲
        currentGameState = GameState.SettingMenu;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        //恢復遊戲
        currentGameState = originalGameState;
        Time.timeScale = 1;
    }
    
    public void SwitchGameState(string newGameState)
    {
        //切換遊戲狀態
        switch (newGameState)
        {
            case "MainMenu":
                currentGameState = GameState.MainMenu;
                break;
            case "InGame":
                currentGameState = GameState.InGame;
                break;
            case "PauseMenu":
                currentGameState = GameState.SettingMenu;
                break;
        }
    }

    public void SwitchBGM(string sceneName)
    {
        //切換背景音樂
        switch (sceneName)
        {
            case "Office":
                AudioManager.Instance.PlayBGM("OfficeBGM");
                break;
            default:
                AudioManager.Instance.PlayBGM("OfficeBGM");
                break;
        }
    }

}
