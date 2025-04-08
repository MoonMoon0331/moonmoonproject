using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Data/EnemyDataBase")]
public class EnemyDataBase : ScriptableObject
{
    public List<EnemyData> enemyList = new List<EnemyData>(); // 詐騙對象清單

    public EnemyData GetEnemyData(int enemyID)
    {
        foreach (var enemy in enemyList)
        {
            if(enemy.enemyID == enemyID) // 如果找到對應的EnemyID
            {
                return enemy;
            }
        }
        return null;
    }
}