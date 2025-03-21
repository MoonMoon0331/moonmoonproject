using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyDatabase enemyDB;  // 參考你的 ScriptableObject

    private Dictionary<int, EnemyRuntimeState> runtimeStates = new Dictionary<int, EnemyRuntimeState>();

    void Awake()
    {
        foreach (var enemyData in enemyDB.enemyList)
        {
            runtimeStates[enemyData.enemyID] = EnemyRuntimeState.UnLock;
        }
    }

    public EnemyRuntimeState GetEnemyState(int enemyID)
    {
        if (runtimeStates.ContainsKey(enemyID))
            return runtimeStates[enemyID];
        return runtimeStates[enemyID] = new EnemyRuntimeState();
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
