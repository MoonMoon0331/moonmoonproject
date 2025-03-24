using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeatDialogue : MonoBehaviour
{
    public GameObject Button;
    private bool isNearPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.GetInteractInput() && isNearPlayer)
        {
            UIManager.Instance.OpenEnemyMenu();
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
