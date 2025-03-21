using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("詐騙對象資料庫")]
    public EnemyDatabase enemyDB;  // 參考你的 ScriptableObject

    private Dictionary<int, EnemyRuntimeState> runtimeStates = new Dictionary<int, EnemyRuntimeState>(); //敵人的狀態
    private List<int> availableEnemyList = new List<int>(); // 可用的敵人清單
    private List<int> completedEnemyList = new List<int>(); // 完成的敵人清單


    [Header("詐騙對象Prefab")]
    public GameObject enemyPrefab; // 參考你的敵人Prefab
    [Header("每頁最多顯示角色數量")]
    public int maxEnemyPerPage = 9; // 每頁最多顯示角色數量

    [Header("翻頁按鈕")]
    public Button previousButton; // 上一頁按鈕
    public Button nextButton; // 下一頁按鈕

    private int currentPage = 0; // 目前頁數
    private int maxPage = 0; // 最大頁數


    void Awake()
    {
        foreach (var enemyData in enemyDB.enemyList)
        {runtimeStates[enemyData.enemyID] = EnemyRuntimeState.UnLock;}
    }

    public void Start()
    {
        // 計算最大頁數
        maxPage = Mathf.CeilToInt((float)enemyDB.enemyList.Count / maxEnemyPerPage);

        currentPage = 0;
    }


    // 顯示敵人
    public void ShowEnemy()
    {
        // 清空可用敵人清單
        availableEnemyList.Clear();

        foreach (Transform child in transform)
        {Destroy(child.gameObject);}

        // 顯示敵人
        for (int i = 0; i < maxEnemyPerPage; i++)
        {
            int index = i + currentPage * maxEnemyPerPage;
            if (index < enemyDB.enemyList.Count)
            {
                EnemyData enemyData = enemyDB.enemyList[index];
                if (runtimeStates[enemyData.enemyID] == EnemyRuntimeState.Available)
                {
                    availableEnemyList.Add(enemyData.enemyID);
                    GameObject enemy = Instantiate(enemyPrefab, transform);
                }
            }
        }

        // 更新翻頁按鈕狀態
        previousButton.interactable = (currentPage > 0);
        nextButton.interactable = (currentPage < maxPage - 1);
    }

    // 上一頁
    public void PreviousPage()
    {currentPage--;ShowEnemy();}

    // 下一頁
    public void NextPage()
    {currentPage++;ShowEnemy();}

    void OnCallEnemy(EnemyData enemyData)
    {
        Debug.Log("Call " + enemyData.enemyName + " at " + enemyData.enemyCallNumber);
    }

    public EnemyRuntimeState GetEnemyState(int enemyID)
    {
        if (runtimeStates.ContainsKey(enemyID))
        return runtimeStates[enemyID];
        runtimeStates[enemyID] = EnemyRuntimeState.UnLock;
        return runtimeStates[enemyID];
    }

    //將敵人設置為可用
    public void IsEnemyAvailable(int enemyID)
    {
        if(runtimeStates.ContainsKey(enemyID))
            runtimeStates[enemyID] = EnemyRuntimeState.Available;
    }

    //將敵人設置為完成狀態
    public void MarkEnemyScammed(int enemyID)
    {
        if (runtimeStates.ContainsKey(enemyID))
            runtimeStates[enemyID] = EnemyRuntimeState.Completed;
    }

}
