using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("詐騙對象資料庫")]
    public EnemyDatabase enemyDB;  // 參考你的 ScriptableObject
    public EnemyRuntimeDatabase enemyRuntimeDB; // 參考你的 ScriptableObject

    private Dictionary<int, EnemyRuntimeData> runtimeData = new Dictionary<int, EnemyRuntimeData>(); //敵人的狀態
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

    //選擇對象
    int selectedIndex = 0;
    private List<GameObject> enemyCardList = new List<GameObject>();

    public void Awake()
    {
        // 初始化每個敵人的動態資料
        foreach (var enemyData in enemyDB.enemyList)
        {runtimeData[enemyData.enemyID] = enemyRuntimeDB.enemyRuntimeList.Find(x => x.enemyID == enemyData.enemyID);}
    }

    public void Start()
    {
        // 計算最大頁數
        maxPage = Mathf.CeilToInt((float)enemyDB.enemyList.Count / maxEnemyPerPage);
        currentPage = 0;
    }

    public void Update()
    {
        // 按下ESC鍵關閉敵人選單
        if (InputManager.Instance.GetCancelInput())
        {
            UIManager.Instance.CloseEnemyMenu();
        }
        // 敵人選擇
        if (InputManager.Instance.GetSubmitInput())
        {
            if (selectedIndex + currentPage * maxEnemyPerPage < enemyDB.enemyList.Count)
            {
                int enemyID = enemyDB.enemyList[selectedIndex + currentPage * maxEnemyPerPage].enemyID;
                SelectEnemy(enemyID);
            }
        }
        // 選擇框移動
        if (InputManager.Instance.GetNavigateUpInput()){MoveSelectionUp();}
        else if (InputManager.Instance.GetNavigateDownInput()){MoveSelectionDown();}
        else if (InputManager.Instance.GetNavigateLeftInput()){MoveSelectionLeft();}
        else if (InputManager.Instance.GetNavigateRightInput()){MoveSelectionRight();}
    }

    // 顯示敵人
    public void ShowEnemy()
    {
        // 選擇第一個敵人
        selectedIndex = 0;
        // 清空完成敵人清單
        availableEnemyList.Clear();
        completedEnemyList.Clear();
        enemyCardList.Clear();

        // 顯示敵人前，先清空所有敵人卡片
        foreach (Transform child in transform)
        {Destroy(child.gameObject);}

        // 顯示敵人
        for (int i = 0; i < maxEnemyPerPage; i++)
        {
            int index = i + currentPage * maxEnemyPerPage;
            GameObject enemyCardObj = Instantiate(enemyPrefab, transform);
            EnemyCard card = enemyCardObj.GetComponent<EnemyCard>();

            if (index < enemyDB.enemyList.Count)
            {
                // 有資料時，更新該卡片資訊
                EnemyData enemyData = enemyDB.enemyList[index];
                card.InformationUpdate(enemyData, runtimeData[enemyData.enemyID]);
            }
            else
            {
                // 沒有資料時，顯示預設內容
                card.InformationUpdateDefault();
            }
            enemyCardList.Add(enemyCardObj);
        }

        enemyCardList[selectedIndex].GetComponent<EnemyCard>().Selected();

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
        Debug.Log("Call Enemy " + enemyData.enemyID);
        // 呼叫敵人的對話
        BattleManager.Instance.CallEnemy(enemyData);   
        transform.parent.gameObject.SetActive(false);
    }

    // 以下方法供外部調用來更新狀態
    public EnemyRuntimeData.EnemyRuntimeState GetEnemyState(int enemyID)
    {
        if (runtimeData.ContainsKey(enemyID))
            return runtimeData[enemyID].state;
        runtimeData[enemyID] = new EnemyRuntimeData();
        return runtimeData[enemyID].state;
    }    

    // 將敵人設置為可用狀態
    public void SetEnemyAvailable(int enemyID)
    {
        if (runtimeData.ContainsKey(enemyID))
            runtimeData[enemyID].state = EnemyRuntimeData.EnemyRuntimeState.Available;
    }

    // 將敵人標記為完成（例如已被詐騙）
    public void MarkEnemyScammed(int enemyID)
    {
        if (runtimeData.ContainsKey(enemyID))
            runtimeData[enemyID].state = EnemyRuntimeData.EnemyRuntimeState.Completed;
    }

    void MoveSelectionUp()
    {
        int newIndex = selectedIndex - 3;
        if (newIndex >= 0)
        {
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Deselected();
            selectedIndex = newIndex;
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Selected();
        }
    }

    void MoveSelectionDown()
    {
        int newIndex = selectedIndex + 3;
        if (newIndex < enemyCardList.Count)
        {
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Deselected();
            selectedIndex = newIndex;
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Selected();
        }
    }

    void MoveSelectionLeft()
    {
        int newIndex = selectedIndex - 1;
        if (newIndex >= 0 && (selectedIndex % 3) != 0)
        {
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Deselected();
            selectedIndex = newIndex;
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Selected();
        }
    }

    void MoveSelectionRight()
    {
        int newIndex = selectedIndex + 1;
        if (newIndex < enemyCardList.Count && ((selectedIndex + 1) % 3) != 0)
        {
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Deselected();
            selectedIndex = newIndex;
            enemyCardList[selectedIndex].GetComponent<EnemyCard>().Selected();
        }
    }

    // 選擇敵人
    public void SelectEnemy(int enemyID)
    {
        Debug.Log("Select Enemy " + enemyID);
        EnemyData enemyData = enemyDB.enemyList.Find(x => x.enemyID == enemyID);
        EnemyRuntimeData enemyRuntimeData = runtimeData[enemyID];

        // 依照敵人狀態執行不同的操作
        switch (enemyRuntimeData.state)
        {
            case EnemyRuntimeData.EnemyRuntimeState.Lock:
                Debug.Log("Enemy is locked");
                break;
            case EnemyRuntimeData.EnemyRuntimeState.Available:
                Debug.Log("Enemy is available");
                OnCallEnemy(enemyData);
                break;
            case EnemyRuntimeData.EnemyRuntimeState.Completed:
                Debug.Log("Enemy is completed");
                break;
        }
    }
}
