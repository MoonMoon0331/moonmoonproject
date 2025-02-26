using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Singleton 實例
    public static GameManager Instance { get; private set; }

    // 遊戲狀態或數據
    public bool isGamePaused = false;
    public bool isDialogueButtonIsOn = false;


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
        isGamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }
}
