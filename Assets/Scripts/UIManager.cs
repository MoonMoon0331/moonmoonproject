using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }



    public GameObject enemyMenu;
    
    [Header("暫停選單")]
    public GameObject SettingMenu;
    public List<GameObject> SettingMenuOptions = new List<GameObject>();
    private int settingMenyCurrentIndex = 0;//獲取暫停選單的選項->0.音效音量 1.音樂音量 2.語言 3.全螢幕選項 4.返回主選單 5.返回遊戲
    public TMP_Text BGMVolumeText;
    public TMP_Text SFXVolumeText;


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
            if(GameManager.Instance.currentGameState == GameManager.GameState.InGame)
            {
                if (SettingMenu.activeSelf)
                {CloseSettingMenu();}
                else
                {OpenSettingMenu();}
            }
        }
        //確認是否在暫停選單中
        if(GameManager.Instance.currentGameState == GameManager.GameState.SettingMenu)
        {SettingMenuInputSystem();}
    }

    /*_____________________________________________________*/
    /*________________________暫停選單______________________*/
    /*_____________________________________________________*/
    public void OpenSettingMenu()
    {
        settingMenyCurrentIndex = 0;
        HightlightSettingMenuOption(0);
        SettingMenu.SetActive(true);

        //獲取音效音量
        SFXVolumeText.text = AudioManager.Instance.sfxVolume.ToString("0.0") + " / 1.0";
        //獲取音樂音量
        BGMVolumeText.text = AudioManager.Instance.bgmVolume.ToString("0.0") + " / 1.0";

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
        if(InputManager.Instance.GetNavigateDownInput())
        {
            if(settingMenyCurrentIndex < 5)
            {
                settingMenyCurrentIndex++;
                HightlightSettingMenuOption(settingMenyCurrentIndex);
            }
            else
            {
                settingMenyCurrentIndex = 0;
                HightlightSettingMenuOption(settingMenyCurrentIndex);
            }
        }
        else if(InputManager.Instance.GetNavigateUpInput())
        {
            if(settingMenyCurrentIndex > 0)
            {
                settingMenyCurrentIndex--;
                HightlightSettingMenuOption(settingMenyCurrentIndex);
            }
            else
            {
                settingMenyCurrentIndex = 5;
                HightlightSettingMenuOption(settingMenyCurrentIndex);
            }
        }
        else if(InputManager.Instance.GetNavigateLeftInput())
        {
            if(settingMenyCurrentIndex == 0)
            {
                AudioManager.Instance.SetSFXVolume(-0.1f);
                SFXVolumeText.text = AudioManager.Instance.sfxVolume.ToString("0.0") + " / 1.0";
            }
            else if(settingMenyCurrentIndex == 1)
            {
                AudioManager.Instance.SetBGMVolume(-0.1f);
                BGMVolumeText.text = AudioManager.Instance.bgmVolume.ToString("0.0") + " / 1.0";
            }
            else if(settingMenyCurrentIndex == 2)
            {
                //語言選項
            }
            else if(settingMenyCurrentIndex == 3)
            {
                //全螢幕選項
            }
        }
        else if(InputManager.Instance.GetNavigateRightInput())
        {
            if(settingMenyCurrentIndex == 0)
            {
                AudioManager.Instance.SetSFXVolume(0.1f);
                SFXVolumeText.text = AudioManager.Instance.sfxVolume.ToString("0.0") + " / 1.0";
            }
            else if(settingMenyCurrentIndex == 1)
            {
                AudioManager.Instance.SetBGMVolume(0.1f);
                BGMVolumeText.text = AudioManager.Instance.bgmVolume.ToString("0.0") + " / 1.0";
            }
            else if(settingMenyCurrentIndex == 2)
            {
                //語言選項
            }
            else if(settingMenyCurrentIndex == 3)
            {
                //全螢幕選項
            }
        }
        else if(InputManager.Instance.GetSubmitInput())
        {
            if(settingMenyCurrentIndex == 4)
            {
                CloseSettingMenu();
                //返回主選單
                SceneTransitionManager.Instance.StartSceneTransition("MainMenu");
            }
            else if(settingMenyCurrentIndex == 5)
            {
                //返回遊戲
                CloseSettingMenu();
            }
        }
        else if(InputManager.Instance.GetCancelInput() || InputManager.Instance.GetPauseInputInUI())
        {
            //返回遊戲
            CloseSettingMenu();
        }
    }

    
    //獲取暫停選單的選項->0.音效音量 1.音樂音量 2.語言 3.全螢幕選項 4.返回主選單 5.返回遊戲
    public void HightlightSettingMenuOption(int index)
    {
        foreach(GameObject option in SettingMenuOptions)
        {
            option.transform.Find("LeftArrow").gameObject.SetActive(false);
            option.transform.Find("RightArrow").gameObject.SetActive(false);
        }
        SettingMenuOptions[index].transform.Find("LeftArrow").gameObject.SetActive(true);
        SettingMenuOptions[index].transform.Find("RightArrow").gameObject.SetActive(true);
    }

    /*_____________________________________________________*/
    /*________________________敵人選單______________________*/
    /*_____________________________________________________*/

    //敵人選單
    public void OpenEnemyMenu()
    {
        GameManager.Instance.currentGameState = GameManager.GameState.BattleUI;

        enemyMenu.SetActive(true);
        InputManager.Instance.EnableUIInput();
        enemyMenu.GetComponentInChildren<EnemyManager>().ShowEnemy();
    }
    public void CloseEnemyMenu()
    {
        GameManager.Instance.currentGameState = GameManager.GameState.InGame;

        enemyMenu.SetActive(false);
        InputManager.Instance.EnablePlayerInput();
    }
}