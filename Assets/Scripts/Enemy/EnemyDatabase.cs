using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Data/EnemyDataBase")]
public class EnemyDataBase : ScriptableObject
{
    public List<EnemyData> enemyList = new List<EnemyData>(); // 詐騙對象清單
}