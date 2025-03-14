using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
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
    

    [Header("計時器")]
    public GameObject timer;
    
    public Image timerBar;
    private bool isTimerRunning = false;
    private float timerDuration = 0f;
    private float timeLimit = 0f;

    
    [Header("角色資訊")]
    public Image Player;
    public Image Enemy;
    public TMP_Text nameTmpText;
    public GameObject nameBox;

    [Header("故事")]
    public TextAsset inkAsset;
    private Story story;

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

    public void Start()
    {
        StopBattle();
    }

    public void Update()
    {
        if (currentState == BattleState.InProgress && InputManager.Instance.GetSubmitInput())
        {ContinueBattle();}

        else if (currentState == BattleState.Choosing)
        {HandleChoiceSelection();}

        if(isTimerRunning)
        {
            timerDuration += Time.deltaTime;
            if(timerDuration >= timeLimit)
            {
                
                Debug.Log("timerDuration: " + timerDuration + " timeLimit: " + timeLimit);
                HandleTimeOut();
                StopTimer();
            }
        }
    }

    public void StopBattle()
    {
        InputManager.Instance.EnablePlayerInput();
        battleUI.SetActive(false);
    }

    public void StartBattle()
    {
        InputManager.Instance.EnableUIInput();
        
        battleUI.SetActive(true);
        if(story != null) return;

        story = new Story(inkAsset.text);
        currentState = BattleState.InProgress;
        
        ContinueBattle();
    }

    private void ContinueBattle()
    {
        if(story == null) return;
        if(!story.canContinue && story.currentChoices.Count == 0)
        {
            story = null;
            StopBattle();
            return;
        }
        if(story.currentChoices.Count > 0)
        {
            
            if(story.variablesState["timeLimit"] != null)
            {
                if(story.variablesState["timeLimit"] is int limit)
                {timeLimit = (float)limit;}
                else if(story.variablesState["timeLimit"] is float limitFloat)
                {timeLimit = limitFloat;}
                else if(story.variablesState["timeLimit"] is double limitDouble)
                {timeLimit = (float)limitDouble;}
                Debug.Log(timeLimit);
                if(timeLimit > 0){StartTimer(timeLimit);}
            }
            currentState = BattleState.Choosing;
            currentChoiceIndex = 0;
            UpdateChoiceButtons();
        }
        if(story.canContinue)
        {
            dialogueTmpText.text = story.Continue();
            currentState = BattleState.InProgress;

            //更新角色名稱
            List<string> tags = story.currentTags;
            if(tags.Count > 0)
            {
                if(tags.Count > 0)
                {nameTmpText.text = tags[0];}
            }
            else
            {
                nameTmpText.text = "";
                nameBox.SetActive(false);
            }

        }
    }

    public void StartTimer(float limit)
    {
        timeLimit = limit;
        timerDuration = 0f;
        isTimerRunning = true;
        timer.SetActive(true);
    }

    public void StopTimer()
    {
        timer.SetActive(false);
        timerDuration = 0f;
        isTimerRunning = false;
    }

    private void HandleTimeOut()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        
        currentState = BattleState.InProgress;
        story.ChoosePathString(story.variablesState["chapterName"].ToString());
        ContinueBattle();
    }

    private void UpdateChoiceButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(i < story.currentChoices.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<BattleChoiceButton>().UpdateButtonText(story.currentChoices[i].text);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        currentChoiceIndex = 0;
        HighlightCurrentChoice();
        currentState = BattleState.Choosing;
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
            {
                buttons[i].GetComponentInChildren<BattleChoiceButton>().OnSelected();
            }
            else
            {
                buttons[i].GetComponentInChildren<BattleChoiceButton>().OffSelected();
            }
        }
    }
}
