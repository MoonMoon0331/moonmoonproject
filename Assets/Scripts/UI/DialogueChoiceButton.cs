using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour
{
    [SerializeField]
    public GameObject arrow;
    public string text;
    public int index;

    private void Start() 
    {
        Transform choice = transform.parent; // Choice
        if (choice != null)
        {
            // 注意：要確認大小寫是否正確
            Transform arrowTransform = choice.Find("Arrow");
            if (arrowTransform != null)
            {
                arrow = arrowTransform.gameObject;
            }
            else
            {
                Debug.LogError("找不到 arrow，請確認層級結構和名稱！");
            }
        }
        else
        {
            Debug.LogError("button 沒有 parent，請確認層級結構！");
        }
        TMP_Text tmpText = GetComponentInChildren<TMP_Text>();

        
    }


    public void OnPointerEnter()
    {
        DialogueManager.Instance.UpdateCurrentChoiceIndex(index);
    }

    // public void OnPointerExit() //取消選取
    // {
    //     OffSelected();
    // }

    public void OnSelected()
    {arrow.SetActive(true);}

    public void OffSelected()
    {arrow.SetActive(false);}

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

    public void MakeChoice()
    {
        Debug.Log(index);
        DialogueManager.Instance.MakeChoice(index);
    }
}
