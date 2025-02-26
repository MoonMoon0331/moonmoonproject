using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleChoiceButton : MonoBehaviour
{
    public GameObject arrow;
    public TMP_Text choiceText;

    public void Start()
    {
        Transform choice = transform.parent;
        Transform arrowTransform = choice.Find("Arrow");
        choiceText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateButtonText(string newText)
    {
        TMP_Text tmpText = GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = newText;
        }
        else
        {
            Debug.LogError("找不到 TMP_Text 元件，請確認層級結構！");
        }
    }

    public void OnSelected()
    {arrow.SetActive(true);}

    public void OffSelected()
    {arrow.SetActive(false);}
}
