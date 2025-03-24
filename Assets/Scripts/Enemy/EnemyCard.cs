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

    [Header("選擇框")]
    public GameObject selectionBox;

    [Header("Default Information")]
    public Sprite defaultPortrait;
    public string defaultName = "No Data";
    public string defaultCallNumber = "N/A";
    public string defaultInformation = "No information available.";

    public void InformationUpdate(EnemyData enemyData, EnemyRuntimeData enemyRuntimeData)
    {
        // 更新敵人資訊
        portrait.sprite = enemyData.enemyPortrait;
        nameText.text = enemyData.enemyName;
        callNumberText.text = enemyData.enemyCallNumber;
        informationText.text = enemyData.enemyInformation;

        // 更新進度條
        progressBar.rectTransform.localScale = new Vector3((float)enemyRuntimeData.enemyCompletionRate / 100, 1, 1);
    

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

    // 顯示預設內容
    public void InformationUpdateDefault()
    {
        portrait.sprite = defaultPortrait;
        nameText.text = defaultName;
        callNumberText.text = defaultCallNumber;
        informationText.text = defaultInformation;
        progressBar.rectTransform.localScale = new Vector3(0, 1, 1);
        LockerBox.SetActive(true);
    }

    public void Selected()
    {
        selectionBox.SetActive(true);
    }

    public void Deselected()
    {
        selectionBox.SetActive(false);
    }
}
