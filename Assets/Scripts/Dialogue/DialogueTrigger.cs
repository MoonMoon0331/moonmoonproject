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
    
    public void TriggerDialogue()
    {
        DialogueManager.Instance.startEyeCatcherDialogue(dialogueFile, dialogueChapterName, playableDirector);
    }
}
