using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //按鈕List
    public List<GameObject> SelectBoxList = new List<GameObject>();


    private int currentSelectBoxIndex = 0; //當前按鈕
    private int lastSelectBoxIndex = 3; //上一個按鈕
    private int maxSelectBoxIndex = 3; //最大按鈕數量
    private int minSelectBoxIndex = 0; //最小按鈕數量


    void Start()
    {
        InputManager.Instance.EnableUIInput();

        //初始化按鈕狀態
        for (int i = 0; i < SelectBoxList.Count; i++)
        {SelectBoxList[i].SetActive(false);}
        //選擇第一個按鈕
        currentSelectBoxIndex = 0;
        lastSelectBoxIndex = 0;
        HighlightButton(currentSelectBoxIndex);
    }


    void Update()
    {
        if(InputManager.Instance.GetNavigateDownInput())
        {
            if(currentSelectBoxIndex < maxSelectBoxIndex)
            {
                currentSelectBoxIndex++;
                HighlightButton(currentSelectBoxIndex);
            }
            else
            {
                currentSelectBoxIndex = minSelectBoxIndex;
                HighlightButton(currentSelectBoxIndex);
            }
        }
        if(InputManager.Instance.GetNavigateUpInput())
        {
            if(currentSelectBoxIndex > minSelectBoxIndex)
            {
                currentSelectBoxIndex--;
                HighlightButton(currentSelectBoxIndex);
            }
            else
            {
                currentSelectBoxIndex = maxSelectBoxIndex;
                HighlightButton(currentSelectBoxIndex);
            }
        }
        if(InputManager.Instance.GetSubmitInput())
        {
            //根據當前按鈕索引執行相應操作
            switch (currentSelectBoxIndex)
            {
                case 0:
                    Debug.Log("Start Game");
                    NewGame();
                    break;
                case 1:
                    Debug.Log("Load Game");
                    //讀取遊戲
                    break;
                case 2:
                    Debug.Log("Options");
                    //選項設置
                    break;
                case 3:
                    Debug.Log("Exit Game");
                    Application.Quit();
                    break;
            }
        }
    }

    private void HighlightButton(int index)
    {
        //取消上個按鈕的高亮
        SelectBoxList[lastSelectBoxIndex].SetActive(false);
        //選擇新的按鈕
        SelectBoxList[index].SetActive(true);
        //更新當前按鈕索引
        lastSelectBoxIndex = index;
    }

    private void NewGame()
    {
        SceneTransitionManager.Instance.StartSceneTransition("Office");
        GameManager.Instance.SwitchGameState("InGame");
        InputManager.Instance.EnablePlayerInput();
    }
}
