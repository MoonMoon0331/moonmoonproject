using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }



    public GameObject enemyMenu;
    
    [Header("暫停選單")]
    public GameObject SettingMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        CloseSettingMenu();
    }

    private void Update()
    {
        if(InputManager.Instance.GetPauseInput() || InputManager.Instance.GetPauseInputInUI())
        {
            if (SettingMenu.activeSelf)
            {CloseSettingMenu();}
            else
            {OpenSettingMenu();}
        }
        //確認是否在暫停選單中
        if(GameManager.Instance.currentGameState == GameManager.GameState.SettingMenu)
        {SettingMenuInputSystem();}
    }

    //暫停選單
    public void OpenSettingMenu()
    {
        SettingMenu.SetActive(true);
        Time.timeScale = 0;
        InputManager.Instance.EnableUIInput();
        GameManager.Instance.currentGameState = GameManager.GameState.SettingMenu;
    }
    public void CloseSettingMenu()
    {
        SettingMenu.SetActive(false);
        Time.timeScale = 1;
        InputManager.Instance.EnablePlayerInput();
        GameManager.Instance.currentGameState = GameManager.GameState.InGame;
    }

    //暫停選單操作系統
    public void SettingMenuInputSystem()
    {
        //獲取暫停選單的選項->0.音效音量 1.音樂音量 2.語言 3.全螢幕選項 4.返回主選單 5.返回遊戲
        int currentIndex = 0;

        if(InputManager.Instance.GetNavigateDownInput())
        {}
        else if(InputManager.Instance.GetNavigateUpInput())
        {}
        else if(InputManager.Instance.GetNavigateLeftInput())
        {}
        else if(InputManager.Instance.GetNavigateRightInput())
        {}
        else if(InputManager.Instance.GetSubmitInput())
        {}
    }

    //敵人選單
    public void OpenEnemyMenu()
    {
        enemyMenu.SetActive(true);
        InputManager.Instance.EnableUIInput();
        enemyMenu.GetComponentInChildren<EnemyManager>().ShowEnemy();
    }
    public void CloseEnemyMenu()
    {
        enemyMenu.SetActive(false);
        InputManager.Instance.EnablePlayerInput();
    }
}