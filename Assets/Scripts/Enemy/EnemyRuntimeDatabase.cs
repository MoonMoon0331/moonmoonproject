using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRuntimeDatabase", menuName = "Data/EnemyRuntimeDatabase")]
public class EnemyRuntimeDatabase : ScriptableObject
{
    public List<EnemyRuntimeData> enemyRuntimeList = new List<EnemyRuntimeData>(); // 詐騙對象狀態清單
}
