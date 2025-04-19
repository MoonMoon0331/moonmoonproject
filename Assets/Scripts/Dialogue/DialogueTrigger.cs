using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset dialogueFile; // Reference to the text file containing the dialogue
    public string dialogueChapterName; // Name of the chapter to start from
    public PlayableDirector playableDirector; // Reference to the PlayableDirector component
    
    void Start()
    {   
        DialogueManager.Instance.endEyeCatchDialogueAction += resumeplayableDirector; // Subscribe to the event
    }

    public void TriggerDialogue()
    {
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        DialogueManager.Instance.startEyeCatcherDialogue(dialogueFile, dialogueChapterName);
    }

    public void resumeplayableDirector()
    {
        if (playableDirector != null )
        {
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
}
