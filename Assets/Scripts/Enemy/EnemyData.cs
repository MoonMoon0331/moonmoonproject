using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public string enemyName;   // 詐騙對象名稱
    public string description; // 詐騙對象描述或其他資訊
    // 可根據需求加入其他屬性
}

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Data/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemies;
}