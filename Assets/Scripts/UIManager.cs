using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // public GameObject pauseMenu;
    // public GameObject inventoryMenu;

    public GameObject enemyMenu;

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
        ClosePauseMenu();
        CloseInventoryMenu();
    }

    public void OpenPauseMenu()
    {
        // pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    {
        // pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenInventoryMenu()
    {
        // inventoryMenu.SetActive(true);
    }

    public void CloseInventoryMenu()
    {
        // inventoryMenu.SetActive(false);
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