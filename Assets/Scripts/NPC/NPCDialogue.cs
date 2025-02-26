using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject Button;
    private bool isNearPlayer = false;

    public TextAsset inkAssets;

    private void Start()
    {
        Button.SetActive(false);
    }

    private void Update()
    {
        if(InputManager.Instance.GetInteractInput() && isNearPlayer)
        {
            DialogueManager.Instance.StartDialogue(inkAssets);
        }
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
