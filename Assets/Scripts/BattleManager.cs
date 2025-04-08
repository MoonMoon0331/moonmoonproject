using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using System.Threading;
using System;
// using Microsoft.Unity.VisualStudio.Editor;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    [Header("戰鬥系統UI")]
    public GameObject battleUI;
    [Header("對話選項")]
    public GameObject[] buttons;
    [Header("對話框")] 
    public GameObject dialogueBox; 
    public TMP_Text dialogueTmpText;
    
    [Header("閱讀文字相關變數")]
    public float readingSpeed = 0.025f; //對話框速度
    public float optionLetterDeleteDelay = 0.05f; // 選項刪除速度
    public float optionLetterAddDelay = 0.025f; // 選項新增速度
    private bool isReadingDialogue = false;
    private string currentDialogueFullText= "";
    private Coroutine readingCoroutine = null;
    private Dictionary<GameObject, Coroutine> deletionCoroutines = new Dictionary<GameObject, Coroutine>();

    [Header("計時器")]
    public GameObject timer;
    
    public Image timerBar;
    private bool isTimerRunning = false;
    private float timerDuration = 0f;
    private float timeLimit = 0f;
    //總計時器時間
    private float totalTimeLimit = 0f;
    private float totalTimeDuration = 0f;
    private bool isTotalTimeRunning = false;

     
    [Header("角色資訊")]
    public Image Player;
    public Image Enemy;
    public TMP_Text nameTmpText;
    public GameObject nameBox;

    [Header("故事")]
    public TextAsset inkAsset;
    private Story story;

    [Header("敵人資料庫")]
    public EnemyDataBase enemyDataBase; //敵人資料庫

    //選擇
    private int currentChoiceIndex = 0;

    //戰鬥階段
    private enum BattleState { InProgress, Choosing, Ended }
    private BattleState currentState = BattleState.Ended;



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

    public void Update()
    {
        
        if(isTimerRunning)
        {
            //計算剩餘時間
            timerDuration += Time.deltaTime;
            float remainingTime = timeLimit - timerDuration; //剩餘時間
            
            //計算連續對話剩餘時間
            if(isTotalTimeRunning)
            {
                totalTimeDuration += Time.deltaTime;
                float remainingTotalTime = totalTimeLimit - totalTimeDuration;
                float t = Mathf.Clamp01(remainingTotalTime / totalTimeLimit);
                timerBar.rectTransform.localScale = new Vector3(t, 1, 1); //更新計時器UI
            }

            if(timerDuration >= timeLimit)
            {
                timerBar.rectTransform.localScale = new Vector3(0, 1, 1);
                StopTimer();
                HandleTimeOut();
                ContinueBattle();
                if(story.variablesState["itCanChoosing"] is bool itCanChoosing) //如果可以選擇，繼續戰鬥
                {
                    if(itCanChoosing){ContinueBattle();}
                }
                else
                {isTotalTimeRunning = false;}
            }
        }

        if (currentState == BattleState.InProgress && InputManager.Instance.GetSubmitInput())
        {
            if(isReadingDialogue)
            {CompleteReading();}
            else
            {ContinueBattle();}
        }

        else if (currentState == BattleState.Choosing)
        {HandleChoiceSelection();}
    }

    public void StopBattle() //停止戰鬥
    {
        InputManager.Instance.EnablePlayerInput(); //啟用玩家輸入
        UIManager.Instance.OpenEnemyMenu(); //開啟敵人選單
        currentState = BattleState.Ended; //設定戰鬥狀態為結束
        battleUI.SetActive(false); //關閉戰鬥UI
    }

    public void StartBattle() //開始戰鬥
    {
        InputManager.Instance.EnableUIInput();
        
        battleUI.SetActive(true);
        if(story != null) return;

        totalTimeDuration = 0f;
        story = new Story(inkAsset.text); //讀取Ink檔案
        currentState = BattleState.InProgress; //設定戰鬥狀態為進行中

        //載入日期及對話節點
        LoadingStory();
        
        ContinueBattle();  //繼續戰鬥
    }

    private void ContinueBattle()
    {
        List<string> tags = new List<string>();
        if(story.currentTags.Count > 0)
        {tags = story.currentTags;}
        if(story == null) return;
        
        //如果故事結束，停止戰鬥
        if(!story.canContinue && story.currentChoices.Count == 0)
        {
            story = null;
            StopBattle();
            return;
        }

        //如果有選項，顯示選項
        if(story.currentChoices.Count > 0)
        {
            
            if(story.variablesState["timeLimit"] != null)
            {
                //總時間限制
                if(story.variablesState["totalTimeLimit"] is int totalLimit)
                {totalTimeLimit = (float)totalLimit;}
                else if(story.variablesState["totalTimeLimit"] is float totalLimitFloat)
                {totalTimeLimit = totalLimitFloat;}
                else if(story.variablesState["totalTimeLimit"] is double totalLimitDouble)
                {totalTimeLimit = (float)totalLimitDouble;}
                if(totalTimeLimit > 0){isTotalTimeRunning = true;}
                //單個對話時間限制
                if(story.variablesState["timeLimit"] is int limit)
                {timeLimit = (float)limit;}
                else if(story.variablesState["timeLimit"] is float limitFloat)
                {timeLimit = limitFloat;}
                else if(story.variablesState["timeLimit"] is double limitDouble)
                {timeLimit = (float)limitDouble;}
                if(timeLimit > 0){StartTimer(timeLimit,totalTimeLimit);}
            }

            currentState = BattleState.Choosing;
            currentChoiceIndex = 0;

            //更新選項按鈕
            UpdateChoiceButtons();
        }
        //如果有對話可繼續
        if(story.canContinue)
        {
            //重置選項及計時器
            HiddenButtons();
            timer.SetActive(false);

            //取得對話內容
            string fullDialogue = story.Continue();

            if(story.currentTags.Count > 0)
            {
                if(story.currentTags[0] == "U")
                {UpdateBattleDialogueInfo(story.currentTags);}
            }

            currentDialogueFullText = fullDialogue;
            if(readingCoroutine != null)
            {
                StopCoroutine(readingCoroutine);
                readingCoroutine = null;
            }
            readingCoroutine = StartCoroutine(ReadingDialogue(fullDialogue));
            isReadingDialogue = true;

            currentState = BattleState.InProgress;

            
        }
    }

    public void StartTimer(float limit,float totalLimit)
    {
        if(isTotalTimeRunning == false)
        {
            totalTimeLimit = totalLimit;
            totalTimeDuration = 0f;
            isTotalTimeRunning = true;
        }
        //單個對話計時器
        timeLimit = limit;
        timerDuration = 0f;
        isTimerRunning = true;
        timer.SetActive(true);
    }

    public void StopTimer()
    {
        timerDuration = 0f;
        isTimerRunning = false;
    }

    private void HandleTimeOut()
    {
        HiddenButtons();
        
        currentState = BattleState.InProgress;
        story.ChoosePathString(story.variablesState["chapterName"].ToString());
        timer.SetActive(false);
    }

    private void UpdateChoiceButtons()
    {
        StartCoroutine(UpdateChoiceButtonsCoroutine());
    }

    private IEnumerator UpdateChoiceButtonsCoroutine()
    {
        int activeCount = Mathf.Min(buttons.Length, story.currentChoices.Count);
        int finishedCount = 0;
        
        

        for (int i = 0; i < activeCount; i++)
        {
            //這個選項的 tags
            List<string> tags = story.currentChoices[i].tags;
            string buttonTextColor = "#000000";
            string buttonAction = "";
            if(story.currentChoices[i].tags.Count > 6)
            {
                if(story.currentChoices[i].tags[6] != null && story.currentChoices[i].tags[6] != "BLACK")
                    buttonTextColor = story.currentChoices[i].tags[6];
            }
            if(story.currentChoices[i].tags.Count > 7)
            {
                if(story.currentChoices[i].tags[7] != null)
                    buttonAction = story.currentChoices[i].tags[7];
            }

            // 如果該按鈕正在執行刪除動畫，先等待它結束
            if (deletionCoroutines.ContainsKey(buttons[i]))
            {
                yield return deletionCoroutines[buttons[i]];
                deletionCoroutines.Remove(buttons[i]);
            }
            buttons[i].SetActive(true);
            // 同時啟動新增文字的動畫，不做 yield return 來等待
            StartCoroutine(MakeChoiceButton(buttons[i], story.currentChoices[i].text,buttonTextColor, () => { finishedCount++; }));
        }
        
        // 等待直到所有按鈕都完成動畫
        yield return new WaitUntil(() => finishedCount == activeCount);
        
        currentChoiceIndex = 0;
        HighlightCurrentChoice();
    }

    private void HandleChoiceSelection()
    {
        if(InputManager.Instance.GetNavigateDownInput())
        {
            int newIndex = (currentChoiceIndex > 0) ? currentChoiceIndex - 1 : story.currentChoices.Count - 1;
            UpdateCurrentChoiceIndex(newIndex);
        }
        else if(InputManager.Instance.GetNavigateUpInput())
        {
            int newIndex = (currentChoiceIndex < story.currentChoices.Count - 1) ? currentChoiceIndex + 1 : 0;
            UpdateCurrentChoiceIndex(newIndex);
        }
        else if(InputManager.Instance.GetSubmitInput())
        {
            MakeChoice(currentChoiceIndex);
        }
    }

    public void MakeChoice(int index)
    {
        story.ChooseChoiceIndex(index);
        currentState = BattleState.InProgress;
        isTotalTimeRunning = false;
        StopTimer();
        ContinueBattle();
    }

    public void UpdateCurrentChoiceIndex(int index)
    {
        currentChoiceIndex = index;
        HighlightCurrentChoice();
    }

    public void HighlightCurrentChoice()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(i == currentChoiceIndex)
            {buttons[i].GetComponentInChildren<BattleChoiceButton>().OnSelected();}
            else
            {buttons[i].GetComponentInChildren<BattleChoiceButton>().OffSelected();}
        }
    }

    private IEnumerator ReadingDialogue(string fullText)
    {
        isReadingDialogue = true;
        dialogueTmpText.text = "";

        // 如果你希望始終打字機效果，忽略 itCanChoosing 的設定
        // 或者你可以根據其它條件來決定
        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueTmpText.text += fullText[i];
            yield return new WaitForSeconds(readingSpeed);
            if (!isReadingDialogue)
                break;
        }

        // 確保完整文字被顯示
        dialogueTmpText.text = fullText;
        isReadingDialogue = false;
        readingCoroutine = null;
    }
    public void HiddenButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].activeSelf)
            {
                // 先確認是否已有該按鈕的刪除 Coroutine
                if (deletionCoroutines.ContainsKey(buttons[i]))
                {
                    StopCoroutine(deletionCoroutines[buttons[i]]);
                    deletionCoroutines.Remove(buttons[i]);
                }
                // 啟動刪除 Coroutine，並記錄它的引用
                Coroutine c = StartCoroutine(DeleteButtonTextLetterByLetter(buttons[i]));
                deletionCoroutines[buttons[i]] = c;
            }
        }
    }

    // 按下確定鍵時，直接顯示完整對話
    private void CompleteReading()
    {
        if(readingCoroutine != null)
        {
            StopCoroutine(readingCoroutine);
            readingCoroutine = null;
        }
        dialogueTmpText.text = currentDialogueFullText;
        isReadingDialogue = false;
    }

    private IEnumerator DeleteButtonTextLetterByLetter(GameObject button)
    {
        TMP_Text btnText = button.GetComponentInChildren<TMP_Text>();
        if(btnText == null)
            yield break;
        string fullText = btnText.text;
        int total = fullText.Length;
        for(int i = 0; i < total; i++)
        {
            btnText.text = fullText.Substring(0, fullText.Length - i - 1);
            yield return new WaitForSeconds(optionLetterDeleteDelay);
        }
        btnText.text = "";
        button.SetActive(false);
    }

    private IEnumerator MakeChoiceButton(GameObject button,string text,string _color, System.Action onComplete = null)
    {

        TMP_Text btnText = button.GetComponentInChildren<TMP_Text>();
        btnText.text = "";

        if(_color == "#000000")
        {btnText.color = Color.black;}
        else if(_color == "RED")
        {btnText.color = Color.red;}
        
        for (int i = 0; i < text.Length; i++)
        {
            btnText.text += text[i];
            yield return new WaitForSeconds(optionLetterAddDelay);
        }
        btnText.text = text;
        if (onComplete != null)
        onComplete();
    }

    public void CallEnemy(EnemyData enemyData)
    {
        inkAsset = enemyData._inkAssets;
        InputManager.Instance.EnableUIInput();
        StartBattle();
    }
    
    //根據天數及節點來載入故事
    private void LoadingStory()
    {
        switch (GameManager.Instance.currentDay)
        {
            case 1:
                story.ChoosePathString("Day1");
                break;
            case 2:
                story.ChoosePathString("Day2");
                break;
            case 3:
                story.ChoosePathString("Day3");
                break;
            case 4:
                story.ChoosePathString("Day4");
                break;
            case 5:
                story.ChoosePathString("Day5");
                break;
            case 6:
                story.ChoosePathString("Day6");
                break;
            case 7:
                story.ChoosePathString("Day7");
                break;
            default:
                Debug.LogError("無效的天數！");
                story.ChoosePathString("Day1"); //預設載入第一天的故事
                break;
        }
    }

    //更新對話角色表情及對話框名字
    private void UpdateBattleDialogueInfo(List<string> tags)
    {
        //初始化
        nameBox.SetActive(false);

        int leftEnemyID = Int32.TryParse(tags[1], out leftEnemyID) ? leftEnemyID : 0;
        string leftCurrentEmotion = tags[2];
        int rightEnemyID = Int32.TryParse(tags[3], out rightEnemyID) ? rightEnemyID : 0;
        string rightCurrentEmotion = tags[4];
        string npcName = tags[5];

        UpdatePlayerEmotion(leftEnemyID,leftCurrentEmotion,Player); //更新玩家表情
        UpdatePlayerEmotion(rightEnemyID,rightCurrentEmotion,Enemy); //更新敵人表情

        //更新對話框名字
        if(npcName == "none")
        {nameTmpText.text = npcName;nameBox.SetActive(false);}
        else if(npcName == "Player")
        {nameTmpText.text = GameManager.Instance.playerName;nameBox.SetActive(true);}
        else if(npcName == "ID")
        {nameTmpText.text = enemyDataBase.GetEnemyData(rightEnemyID).enemyName;nameBox.SetActive(true);}
        else
        {nameTmpText.text = npcName;nameBox.SetActive(true);}
    }
    private void UpdatePlayerEmotion(int npcID,string emotion,Image image)
    {
        if(npcID == 999)
        {
            switch(emotion)
            {
                case "D":
                    image.sprite = GameManager.Instance.playerSprite;
                    break;
                case "H":
                    image.sprite = GameManager.Instance.playerSpriteHappy;
                    break;
                case "A":
                    image.sprite = GameManager.Instance.playerSpriteAngry;
                    break;
                case "S":
                    image.sprite = GameManager.Instance.playerSpriteSad;
                    break;
                case "T":
                    image.sprite = GameManager.Instance.playerSpriteThinking;
                    break;
                case "K":
                    image.sprite = GameManager.Instance.playerSpriteSurprised;
                    break;
                default:
                    image.sprite = GameManager.Instance.playerSprite;
                    break;
            }
        }
        else
        {
            switch(emotion)
            {
                case "D":
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySprite;
                    break;
                case "H":
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySpriteHappy;
                    break;
                case "A":
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySpriteAngry;
                    break;
                case "S":
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySpriteSad;
                    break;
                case "T":
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySpriteThinking;
                    break;
                case "K":
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySpriteSurprised;
                    break;
                default:
                    image.sprite = enemyDataBase.GetEnemyData(npcID).enemySprite;
                    break;
            }
        }

        
    }
        
}
