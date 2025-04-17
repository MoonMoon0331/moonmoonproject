using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private GameInputActions inputActions;
    private Vector2 movementInput;
    // private bool interactPressed;

    private void Awake()
    {
        // 確保只有一個 InputManager 存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // 初始化 Input System
        inputActions = new GameInputActions();
    }

    private void OnEnable()
    {EnablePlayerInput();}

    private void OnDisable()
    {
        if(inputActions!=null)
            DisableAllInputs();
    }

    private void OnDestroy()
{
    if (inputActions != null)
    {
        inputActions.Dispose();
    }
}

    // 切換到 Player 操作
    public void EnablePlayerInput()
    {
        Debug.Log("Enable Player Input");
        inputActions.UI.Disable();
        inputActions.Player.Enable();
        
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    // 切換到 UI 操作
    // 這裡的 UI 操作是指選單操作，例如暫停選單、物品欄等
    public void EnableUIInput()
    {
        Debug.Log("Enable UI Input");
        inputActions.Player.Disable();
        inputActions.UI.Enable();
    }

    // 取消所有操作
    public void DisableAllInputs()
    {
        inputActions.Player.Disable();
        inputActions.UI.Disable();
    }

    // 玩家移動輸入
    private void OnMove(InputAction.CallbackContext context)
    {movementInput = context.ReadValue<Vector2>();}
    public Vector2 GetMovementInput()
    {return movementInput;}

    //暫停按鈕
    public bool GetPauseInput() => inputActions.Player.Pause.WasPerformedThisFrame();
    public bool GetPauseInputInUI() => inputActions.UI.Start.WasPerformedThisFrame();

    public bool GetInteractInput()
    {
        return inputActions.Player.Interact.WasPerformedThisFrame();
    }

    // UI 操作輸入 (上下左右、確認、取消)
    public bool GetNavigateUpInput() => inputActions.UI.NavigateUp.WasPerformedThisFrame();
    public bool GetNavigateDownInput() => inputActions.UI.NavigateDown.WasPerformedThisFrame();
    public bool GetNavigateLeftInput() => inputActions.UI.NavigateLeft.WasPerformedThisFrame();
    public bool GetNavigateRightInput() => inputActions.UI.NavigateRight.WasPerformedThisFrame();
    public bool GetSubmitInput() => inputActions.UI.Submit.WasPerformedThisFrame();
    public bool GetCancelInput() => inputActions.UI.Cancel.WasPerformedThisFrame();
}
