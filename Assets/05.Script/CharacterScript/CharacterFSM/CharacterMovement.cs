using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float gravity = 10.0f;
    [SerializeField]
    private float jumpSpeed = 4.0f;
    [SerializeField]
    private float MaxSpeed = 10.0f;

    private Transform _modelTr;
    private KeyInput _keyInput;
    private CharacterController _characterController;
    private Animator _animator;
    private Transform _cameraTr;
    private Character _character;

    private Vector3 playerMoveDir = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private float xSpeed = 0.0f;
    private float ySpeed = 0.0f;
    

    public CharacterMovement SetUp(Character entity)
    {
        _character = entity;
        _animator = entity.OwnedAnimator;
        _keyInput = entity.KeyInput;
        _cameraTr = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _characterController = entity.CharacterController;
        _modelTr = entity.ModelTr;

        return this;
    }

    public void Updated()
    {
        if (_character.CurrentState != Character.eCharacterStates.BASHPROGRESS)
            MoveCharacter();
    }
   
    private void MoveCharacter()
    {
        xSpeed = 0.0f;
        ySpeed = 0.0f;
        if (_character.CurrentState == Character.eCharacterStates.IDLE ||
            _character.CurrentState == Character.eCharacterStates.MOVE){
            xSpeed = _keyInput.MovementInput.x;
            ySpeed = _keyInput.MovementInput.y;
        }
        bool grounded = _characterController.isGrounded;


        CharacterRotate_ThroughCamera();
        if (grounded)
        {
            moveDirection = new Vector3(xSpeed, 0, ySpeed);
            moveDirection = _character.transform.TransformDirection(moveDirection);
            moveDirection *= MaxSpeed;
            if (Mathf.Abs(xSpeed) > 0.2 || Mathf.Abs(ySpeed) > 0.2)
                RotateModel();
            if (_keyInput.JumpInput)
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        _characterController.Move(moveDirection * Time.deltaTime);
        _animator.SetFloat("InputX", xSpeed);
        _animator.SetFloat("InputY", ySpeed);
        _animator.SetBool("IsInAir", !grounded);
    }
    public void CharacterRotate_ThroughCamera()
    {
        playerMoveDir = _character.transform.position - _cameraTr.position;
        playerMoveDir = playerMoveDir.normalized;
        playerMoveDir.y = 0f;
        _character.transform.forward = playerMoveDir;
    }
    public void RotateModel()
    {
        _modelTr.rotation = Quaternion.Lerp
            (_modelTr.rotation, Quaternion.LookRotation(moveDirection),
            Time.deltaTime * 10.0f);
    }
}
