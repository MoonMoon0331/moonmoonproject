using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.Playables;

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

    [Header("閱讀速度")]
    public float readSpeed = 0.02f; //每個字的閱讀速度
    private bool isReadingDialogue = false;
    private string currentDialogueFullText = "";
    private Coroutine readingCoroutine = null;

    public Action endEyeCatchDialogueAction; //結束對話的事件

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
            AudioManager.Instance.PlaySFX("Click(Confirm)SFX"); //播放音效
            if (isReadingDialogue)// 跳過打字
            {CompleteReading();}
            else// 進入下一句
            {NextDialog();}
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
        LoadingDialogueChapter();
        
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
            //如果是過場對話
            if(GameManager.Instance.currentGameState == GameManager.GameState.EyeCatchDialogue) 
            {stopEyeCatcherDialogue();return;}
            //如果是一般對話
            story = null;
            StopDialogue();
            return;
        }

        if (story.canContinue) //如果可以繼續下一句對話，執行 story.Continue()
        {
            string nextLine = story.Continue(); //讀取對話內容
            currentDialogueFullText = nextLine; //將對話內容存入變數
            
            //打字機效果
            if (readingCoroutine != null)
            {StopCoroutine(readingCoroutine);}
            readingCoroutine = StartCoroutine(ReadingDialogue(currentDialogueFullText));

            // 讀取 Tags
            if(story.currentTags.Count > 0)
            {
                if(story.currentTags[0] == "U")
                {UpdateDialogueInformation(story.currentTags);}
            }
        }
        if (story.currentChoices.Count > 0) //取得目前對話選項數量，如果 > 0 則設定選項按鈕
        {
            currentState = DialogueState.Choosing;
            currentChoiceIndex = 0;
            UpdateChoices();
        }
    }
    //打字機效果
    private IEnumerator ReadingDialogue(string fullText)
    {
        isReadingDialogue = true;
        dialogueTmpText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueTmpText.text += fullText[i];
            yield return new WaitForSeconds(readSpeed);

            if (!isReadingDialogue)
                break;
        }

        dialogueTmpText.text = fullText;
        isReadingDialogue = false;
        readingCoroutine = null;
    }
    ///當按下跳過鍵時，停止打字機效果
    private void CompleteReading()
    {
        if (readingCoroutine != null)
        {
            StopCoroutine(readingCoroutine);
            readingCoroutine = null;
        }

        dialogueTmpText.text = currentDialogueFullText;
        isReadingDialogue = false;
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
        //如果沒有選項，則不執行
        if (story.currentChoices == null || story.currentChoices.Count == 0)
        return;

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
        if (story.currentChoices == null || index < 0 || index >= story.currentChoices.Count)
        {
            Debug.LogWarning("選擇索引超出範圍：" + index);
            return;
        }

        story.ChooseChoiceIndex(index); //使用 ChooseChoiceIndex 選擇當前選項
        for (int i = 0; i < buttons.Length; i++) //選擇完，將按鈕隱藏
        {buttons[i].gameObject.SetActive(false);}
        currentState = DialogueState.InProgress; //將狀態改回對話中
        NextDialog();
    }

    public void UpdateCurrentChoiceIndex(int index)
    {
        currentChoiceIndex = index;
        HighlightCurrentChoice();
    }

    //更新對話資訊(1.角色名字 2.角色表情)
    public void UpdateDialogueInformation(List<string> tags)
    {
        //初始化
        nameBox.SetActive(false);
        //如果沒有標籤，則不執行
        if(tags.Count == 0 || tags[0] != "U") return; 

        int npcID = Int32.TryParse(tags[1], out npcID) ? npcID : 0; //將字串轉換成整數
        string currentEmotion = tags[2];
        string npcName = tags[3];

        //更新對話中使用的表情
        if(npcID == 999) //如果是主角
        {
            switch (currentEmotion)
            {
                case "D":
                    playerPortraitImage.sprite = GameManager.Instance.playerPortrait;
                    break;
                case "H":
                    playerPortraitImage.sprite = GameManager.Instance.playerPortraitHappy;
                    break;
                case "S":
                    playerPortraitImage.sprite = GameManager.Instance.playerPortraitSad;
                    break;
                case "A":
                    playerPortraitImage.sprite = GameManager.Instance.playerPortraitAngry;
                    break;
                case "T":
                    playerPortraitImage.sprite = GameManager.Instance.playerPortraitThinking;
                    break;
                case "K":
                    playerPortraitImage.sprite = GameManager.Instance.playerPortraitSurprised;
                    break;
                default:
                    playerPortraitImage.sprite = GameManager.Instance.playerPortrait;
                    break;
            }
        }
        else //如果是NPC
        {
            switch (currentEmotion)
            {
                case "D":
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortrait;
                    break;
                case "H":
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortraitHappy;
                    break;
                case "S":
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortraitSad;
                    break;
                case "A":
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortraitAngry;
                    break;
                case "T":
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortraitThinking;
                    break;
                case "K":
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortraitSurprised;
                    break;
                default:
                    playerPortraitImage.sprite = npcDataBase.GetNPCData(npcID).npcPortrait;
                    break;

            }
        }

        //更新對話中使用的名字
            if(npcName == "none")
            {nameTmpText.text = npcName;nameBox.SetActive(false);}
            else if(npcName == "Player")
            {nameTmpText.text = GameManager.Instance.playerName;nameBox.SetActive(true);}
            else if(npcName == "ID")
            {nameTmpText.text = npcDataBase.GetNPCData(npcID).npcName;nameBox.SetActive(true);}
            else
            {nameTmpText.text = npcName;nameBox.SetActive(true);}
    }

    //載入對話背景資訊
    public void LoadingDialogueChapter()
    {
        switch(GameManager.Instance.currentDay)
        {
            case 1:
                story.ChoosePathString("Day1"); //選擇對話路徑
                break;
            case 2:
                story.ChoosePathString("Day2"); //選擇對話路徑
                break;
            case 3:
                story.ChoosePathString("Day3"); //選擇對話路徑
                break;
            case 4:
                story.ChoosePathString("Day4"); //選擇對話路徑
                break;
            case 5:
                story.ChoosePathString("Day5"); //選擇對話路徑
                break;
            case 6:
                story.ChoosePathString("Day6"); //選擇對話路徑
                break;
            case 7:
                story.ChoosePathString("Day7"); //選擇對話路徑
                break;
            default:
                Debug.LogError("無效的天數！");
                break;
        }
    }

    //_______________________________________________//
    //_________________過場對話______________________//
    //______________________________________________//

    public void startEyeCatcherDialogue(TextAsset _inkAssets,string chapterName)
    {
        GameManager.Instance.currentGameState = GameManager.GameState.EyeCatchDialogue; //將遊戲狀態改為過場對話

        dialogueBox.SetActive(true);

        if (story != null) return;

        story = new Story(_inkAssets.text); //new Story 裡面放json檔的文字，讓 Story 初始化
        currentState = DialogueState.InProgress;

        //載入對話數據
        story.ChoosePathString(chapterName); //選擇對話路徑
        
        //更改輸入系統
        InputManager.Instance.EnableUIInput();
        NextDialog();
    }
    public void stopEyeCatcherDialogue()
    {
        dialogueBox.SetActive(false);
        currentState = DialogueState.Ended;
        InputManager.Instance.DisableAllInputs();
        story = null;
        GameManager.Instance.currentGameState = GameManager.GameState.InGame; //將遊戲狀態改回遊戲中
        endEyeCatchDialogueAction?.Invoke(); //執行結束對話的事件
        endEyeCatchDialogueAction = null; //清空事件
    }
}
