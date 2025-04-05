using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Singleton 實例
    public static GameManager Instance { get; private set; }

    // 遊戲狀態或數據
    public bool isDialogueButtonIsOn = false;

    //遊戲時間數據
    public int currentDay = 1;
    public float timeHour = 0;
    
    //遊戲狀態
    private enum GameState{MainMenu,InGame,PauseMenu}
    private GameState currentGameState = GameState.InGame;
    private GameState originalGameState = GameState.InGame;

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

    }

    public void PauseGame()
    {
        originalGameState = currentGameState;
        //暫停遊戲
        currentGameState = GameState.PauseMenu;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        //恢復遊戲
        currentGameState = originalGameState;
        Time.timeScale = 1;
    }

}
