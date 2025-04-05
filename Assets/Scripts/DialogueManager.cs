using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialogueBox;

    [Header("對話框")]
    public TMP_Text dialogueTmpText;
    
    public GameObject[] buttons;
    public TextAsset _inkAssets;
    Story story = null;

    private int currentChoiceIndex = 0;

    [Header("角色名字")]
    public GameObject nameBox;
    public TMP_Text nameTmpText;

    private enum DialogueState { InProgress, Choosing, Ended }
    private DialogueState currentState = DialogueState.Ended;

    [Header("角色大頭貼")]
    public Image playerPortraitImage;

    [Header("NPC資料庫")]
    public NPCDataBase npcDataBase; 

    //對話資訊


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 如果已有實例，刪除重複的
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        StopDialogue();
    }

    private void Update() 
    {
        if (currentState == DialogueState.InProgress && InputManager.Instance.GetSubmitInput())
        {
            NextDialog();
        }
        else if (currentState == DialogueState.Choosing)
        {
            HandleChoiceSelection();
        }
    }

    public bool StartDialogue(TextAsset _inkAssets)
    {
        dialogueBox.SetActive(true);

        if (story != null) return false;

        story = new Story(_inkAssets.text); //new Story 裡面放json檔的文字，讓 Story 初始化
        currentState = DialogueState.InProgress;

        //載入對話數據
        LoadingDialogueInformation();

        //更改輸入系統
        InputManager.Instance.EnableUIInput();
        NextDialog();

        return true;
    }

    public void StopDialogue()
    {
        dialogueBox.SetActive(false);
        currentState = DialogueState.Ended;
        InputManager.Instance.EnablePlayerInput();
    }

    public void NextDialog() 
    {
        if (story == null) return;


        if (!story.canContinue && story.currentChoices.Count == 0) //如果story不能繼續 && 沒有選項，則代表對話結束
        { 
            story = null;
            StopDialogue();
            return;
        }
        if (story.currentChoices.Count > 0) //取得目前對話選項數量，如果 > 0 則設定選項按鈕
        {
            currentState = DialogueState.Choosing;
            currentChoiceIndex = 0;
            UpdateChoices();
        }
        if (story.canContinue) //如果可以繼續下一句對話，執行 story.Continue()
        {
            dialogueTmpText.text = story.Continue();
            currentState = DialogueState.InProgress;

            // 讀取 Tags
            if(story.currentTags.Count > 0)
            {
                if(story.currentTags[0] == "U")
                {UpdateDialogueInformation();}
            }
        }
    }

    private void UpdateChoices()
    {
        for (int i = 0; i < buttons.Length; i++) 
        {
            if(i < story.currentChoices.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<DialogueChoiceButton>().UpdateButtonText(story.currentChoices[i].text);
                Debug.Log(story.currentChoices[i].text);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        currentChoiceIndex = 0; //讓選擇變回第一個

        HighlightCurrentChoice();
    }

    private void HighlightCurrentChoice()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i == currentChoiceIndex)
            {
                buttons[i].GetComponentInChildren<DialogueChoiceButton>().OnSelected();
            }
            else
            {
                buttons[i].GetComponentInChildren<DialogueChoiceButton>().OffSelected();
            }
        }
    }

    private void HandleChoiceSelection()
    {
        if (InputManager.Instance.GetNavigateDownInput())
        {
            currentChoiceIndex = (currentChoiceIndex > 0) ? currentChoiceIndex - 1 : story.currentChoices.Count - 1;
            HighlightCurrentChoice();
            Debug.Log(currentChoiceIndex);
        }
        else if (InputManager.Instance.GetNavigateUpInput())
        {
            currentChoiceIndex = (currentChoiceIndex < story.currentChoices.Count - 1) ? currentChoiceIndex + 1 : 0;
            HighlightCurrentChoice();
            Debug.Log(currentChoiceIndex);
        }
        else if (InputManager.Instance.GetSubmitInput())
        {
            MakeChoice(currentChoiceIndex);
        }
    }

    public void MakeChoice(int index)
    {
        story.ChooseChoiceIndex(index); //使用 ChooseChoiceIndex 選擇當前選項
        for (int i = 0; i < buttons.Length; i++) //選擇完，將按鈕隱藏
        {buttons[i].gameObject.SetActive(false);}

        NextDialog();
    }

    public void UpdateCurrentChoiceIndex(int index)
    {
        currentChoiceIndex = index;
        HighlightCurrentChoice();
    }

    //更新對話資訊(1.角色名字 2.角色表情)
    public void UpdateDialogueInformation()
    {
        //初始化
        nameBox.SetActive(false);
        //更新對話中使用的表情
        if(story.variablesState.TryGetDefaultVariableValue("currentEmotion"))
        {
            
        }

        

        //更新對話中使用的名字
        if(story.variablesState.TryGetDefaultVariableValue("NPCName"))
        {
            string npcName = story.variablesState["NPCName"].ToString();
            if(npcName == "")
            {nameTmpText.text = npcName;nameBox.SetActive(false);}
            else if(npcName == "Player")
            {nameTmpText.text = GameManager.Instance.playerName;nameBox.SetActive(true);}
            else
            {nameTmpText.text = npcName;nameBox.SetActive(true);}
        }
        else{nameBox.SetActive(false);}
    }

    //載入對話背景資訊
    public void LoadingDialogueInformation()
    {
        if(story.variablesState.TryGetDefaultVariableValue("currentDay"))
        {story.variablesState["currentDay"] = GameManager.Instance.currentDay;}
    }
}
