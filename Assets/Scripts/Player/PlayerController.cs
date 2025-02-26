using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 1f;

   private Vector2 movement;
   private Rigidbody2D rb;

   private Animator myAnimator;
   private SpriteRenderer mySpriteRenderer;

   public LayerMask interactablesLayer;
   public PlayerControls playerControls;

   private void Awake() 
   {
      // playerControls = new PlayerControls();
      rb = GetComponent<Rigidbody2D>();
      myAnimator = GetComponent<Animator>();
      mySpriteRenderer = GetComponent<SpriteRenderer>();
   }

   private void OnEnable() 
   {
      // playerControls.Enable();
   }

   private void Update() 
   {
      PlayerInput();
   }

   private void FixedUpdate() 
   {
      AdjustPlayerFacingDirection();
      Move();
   }

   private void PlayerInput() 
   {
      // if(GameManager.Instance.isTalk){return;}
      movement = InputManager.Instance.GetMovementInput();
   }

   private void Move() 
   {
      rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
   }

   private void AdjustPlayerFacingDirection()
   {
      if (movement == Vector2.zero) 
      {myAnimator.SetBool("isWalk", false);return; }
      else
      {
          myAnimator.SetBool("isWalk", true);
         if (Mathf.Abs(movement.x) >= MathF.Abs(movement.y))
         {
            if (movement.x < 0)
            {myAnimator.SetTrigger("walkLeft");}
            else
            {myAnimator.SetTrigger("walkRight");}
         }
         else
         {
            if (movement.y < 0)
            {myAnimator.SetTrigger("walkDown");}
            else
            {myAnimator.SetTrigger("walkUp");}
         }
      }
        
   }
}
