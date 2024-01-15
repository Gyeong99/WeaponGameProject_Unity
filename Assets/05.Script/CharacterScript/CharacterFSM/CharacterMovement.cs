using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float _gravity = 10.0f;
    [SerializeField]
    private float _jumpSpeed = 4.0f;
    [SerializeField]
    private float _MaxSpeed = 10.0f;

    private Transform _modelTr;
    private KeyInput _keyInput;
    private CharacterController _characterController;
    private Animator _animator;
    private Transform _cameraTr;
    private Character _character;

    private Vector3 playerMoveDir = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    

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
        var xSpeed = 0.0f;
        var ySpeed = 0.0f;
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
            moveDirection *= _MaxSpeed;
            if (Mathf.Abs(xSpeed) > 0.2 || Mathf.Abs(ySpeed) > 0.2)
                RotateModel();
            if (_keyInput.JumpInput)
                moveDirection.y = _jumpSpeed;
        }

        moveDirection.y -= _gravity * Time.deltaTime;
        _characterController.Move(moveDirection * Time.deltaTime);
        _animator.SetFloat("InputX", xSpeed);
        _animator.SetFloat("InputY", ySpeed);
        _animator.SetBool("IsInAir", !grounded);
    }
    private void CharacterRotate_ThroughCamera()
    {
        playerMoveDir = _character.transform.position - _cameraTr.position;
        playerMoveDir = playerMoveDir.normalized;
        playerMoveDir.y = 0f;
        _character.transform.forward = playerMoveDir;
    }
    private void RotateModel()
    {
        _modelTr.rotation = Quaternion.Lerp
            (_modelTr.rotation, Quaternion.LookRotation(moveDirection),
            Time.deltaTime * 10.0f);
    }
}
