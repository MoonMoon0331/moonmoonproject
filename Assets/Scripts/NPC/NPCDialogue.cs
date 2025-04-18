using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Data")]
    public NPCDataBase npcDataBase; // NPC資料管理器
    public int npcID; // NPC ID
    private NPCData npcData;

    [Header("其他組件")]
    public GameObject Button;
    public GameObject NPCSprite;

    private bool isNearPlayer = false;

    public TextAsset inkAssets;

    private void Start()
    {
        // Get the NPCData from the NPCDataManager
        npcData = npcDataBase.npcList[npcID];
        inkAssets = npcData._inkAssets;

        // Set the NPC sprite and animator controller
        // NPCSprite.GetComponent<Animator>().runtimeAnimatorController = npcData.animatorController;

        Button.SetActive(false);
    }

    private void Update()
    {
        if(InputManager.Instance.GetInteractInput() && isNearPlayer)
        {DialogueManager.Instance.StartDialogue(npcData._inkAssets);}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !GameManager.Instance.isDialogueButtonIsOn)
        {
            GameManager.Instance.isDialogueButtonIsOn = true;
            Button.SetActive(true);
            isNearPlayer = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.Instance.isDialogueButtonIsOn = false;
            Button.SetActive(false);
            isNearPlayer = false;
        }
    }
}
