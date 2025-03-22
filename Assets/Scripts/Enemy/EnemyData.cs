using UnityEngine;
using System.Collections.Generic;
using Ink.Runtime;

[System.Serializable]
public class EnemyData
{
    public int enemyID; // 詐騙對象ID
    public string enemyName;   // 詐騙對象名稱
    public string enemyCallNumber; // 詐騙對象的電話號碼
    public string enemyInformation; // 詐騙對象的資訊
    public TextAsset _inkAssets; // 詐騙對象的對話內容
    public Sprite enemySprite; // 詐騙對象的圖片
}

// 敵人的狀態
[System.Serializable]

public class EnemyRuntimeData
{public enum EnemyRuntimeState{UnLock,Available,Completed};public int enemyCompletionRate;public EnemyRuntimeState state = EnemyRuntimeState.UnLock;}

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Data/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemyList = new List<EnemyData>(); // 詐騙對象清單
}