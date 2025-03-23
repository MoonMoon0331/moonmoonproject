using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;

public class EnemyCard : MonoBehaviour
{
    public Image portrait;
    public Image progressBar;
    public GameObject LockerBox;
    public TMP_Text nameText;
    public TMP_Text callNumberText;
    public TMP_Text informationText;

    public void InformationUpdate(EnemyData enemyData, EnemyRuntimeData enemyRuntimeData)
    {
        // 更新敵人資訊
        portrait.sprite = enemyData.enemyPortrait;
        nameText.text = enemyData.enemyName;
        callNumberText.text = enemyData.enemyCallNumber;
        informationText.text = enemyData.enemyInformation;

        // 更新進度條
        progressBar.fillAmount = (float)enemyRuntimeData.enemyCompletionRate / 100;

        // 更新鎖定狀態
        switch (enemyRuntimeData.state)
        {
            case EnemyRuntimeData.EnemyRuntimeState.Lock:
                LockerBox.gameObject.SetActive(true);
                break;
            case EnemyRuntimeData.EnemyRuntimeState.Available:
                LockerBox.gameObject.SetActive(false);
                break;
            case EnemyRuntimeData.EnemyRuntimeState.Completed:
                LockerBox.gameObject.SetActive(true);
                break;
        }
    }
}
