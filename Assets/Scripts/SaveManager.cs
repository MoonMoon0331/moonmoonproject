using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    [Header("遊戲中時間")]
    public int day; //遊戲中天數
    public float time; //遊戲中時間

    [Header("玩家資訊")]
    public Vector2 playerPosition; //玩家當前位置
    public string SceneName; //當前場景名稱
}

[System.Serializable]
public class NPCRuntimeData
{
    public int npcID; //NPC ID
    public string name;
    public string currentStoryNodeName; //當前故事節點名稱
}

[System.Serializable]// 敵人的狀態資料
public class EnemyRuntimeData
{
    public int enemyID;
    public enum EnemyRuntimeState{Lock,Available,Completed};
    public int enemyCompletionRate;
    public EnemyRuntimeState state = EnemyRuntimeState.Lock;
}



public class SaveManager : MonoBehaviour
{

    private string saveFilePath; //存檔路徑

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json"); //設定存檔路徑
    }

    public void SaveGame(SaveData saveData, List<NPCRuntimeData> npcDataList, List<EnemyRuntimeData> enemyRuntimeDataList)
    {
        //將存檔資料轉換為JSON格式
        string json = JsonUtility.ToJson(saveData, true);
        string npcJson = JsonUtility.ToJson(npcDataList, true);
        string enemyJson = JsonUtility.ToJson(enemyRuntimeDataList, true);

        //將JSON格式的資料寫入檔案
        File.WriteAllText(saveFilePath, json + "\n" + npcJson + "\n" + enemyJson);
    }

    public SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            //讀取檔案內容
            string json = File.ReadAllText(saveFilePath);
            //將JSON格式的資料轉換為存檔資料物件
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            return saveData;
        }
        else
        {
            Debug.LogError("Save file not found!");
            return null;
        }
    }

    public List<NPCRuntimeData> LoadNPCData()
    {
        if (File.Exists(saveFilePath))
        {
            //讀取檔案內容
            string json = File.ReadAllText(saveFilePath);
            //將JSON格式的資料轉換為NPC資料物件
            List<NPCRuntimeData> npcDataList = JsonUtility.FromJson<List<NPCRuntimeData>>(json);
            return npcDataList;
        }
        else
        {
            Debug.LogError("Save file not found!");
            return null;
        }
    }
}

