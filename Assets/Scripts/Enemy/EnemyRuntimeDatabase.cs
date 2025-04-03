using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRuntimeDatabase : ScriptableObject
{
    public List<EnemyRuntimeData> enemyRuntimeList = new List<EnemyRuntimeData>(); // 詐騙對象狀態清單
}
