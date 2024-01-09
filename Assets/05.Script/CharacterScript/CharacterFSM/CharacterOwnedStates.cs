using Retro.ThirdPersonCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Character;
using static UnityEngine.EventSystems.EventTrigger;

namespace CharacterOwnedStates
{
    public class Idle : EntityState<Character>
    {
        private KeyInput _keyInput;
        private Character _character;
        private ManageWeapon _manageWeapon;
        public override void SetUp(Character entity)
        {
            _character = entity;
            _keyInput = entity.KeyInput;
            _manageWeapon = entity.ManageWepon;
        }
        public override void Enter(Character entity)
        {
            entity.PrintText("Idle State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {
            CheckKeyInput();
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Idle State Out");
        }

        private void CheckKeyInput()
        {
            if (Input.anyKeyDown)
            {
                foreach (var dic in _keyInput.KeyCharacterDictionary)
                {
                    if (Input.GetKeyDown(dic.Key))
                    {
                        dic.Value(this._character);
                    }
                }
            }
            if (_keyInput.AttackInput)
            {
                _character.ChangeState(Character.eCharacterStates.ATTACK);
            }
            if (_keyInput.ThrowInput && _manageWeapon.IsHaveWeapon)
            {
                _character.ChangeState(Character.eCharacterStates.THROW);
            }
            if (_keyInput.MovementInput.magnitude > 0.01f || _keyInput.JumpInput)
            {
                _character.ChangeState(Character.eCharacterStates.MOVE);
            }
        }
    }
    public class Move : EntityState<Character>
    { 
        private KeyInput _keyInput;
        private CharacterController _characterController;
        private Character _character;
        private ManageWeapon _manageWeapon;

        public override void SetUp(Character entity)
        {
            _character = entity;
            _keyInput = entity.KeyInput;
            _characterController = entity.CharacterController;
            _manageWeapon = entity.ManageWepon;
        }
        public override void Enter(Character entity)
        {
            entity.PrintText("Move State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {
            CheckKeyInput();
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Move State Out");
        }
        private void CheckKeyInput()
        {
            if (Input.anyKeyDown)
            {
                foreach (var dic in _keyInput.KeyCharacterDictionary)
                {
                    if (Input.GetKeyDown(dic.Key) && dic.Key == KeyCode.LeftShift)
                    {
                        dic.Value(this._character);
                    }
                }
            }
            if (_characterController.isGrounded)
            {
                if (_keyInput.AttackInput)
                {
                    _character.ChangeState(Character.eCharacterStates.ATTACK);
                }
                if (_keyInput.ThrowInput && _manageWeapon.IsHaveWeapon)
                {
                    _character.ChangeState(Character.eCharacterStates.THROW);
                }
                if (_characterController.velocity.magnitude < 0.1f)
                {
                    _character.ChangeState(Character.eCharacterStates.IDLE);
                }
            }
        }
      
    }

    public class Attack : EntityState<Character>
    {
        private const string attackTriggerName = "Attack";
        private Animator _animator;
        
        int waitFrame = 0;
        public override void SetUp(Character entity)
        {
            _animator = entity.OwnedAnimator;
        }
        public override void Enter(Character entity)
        {
            _animator.SetTrigger(attackTriggerName);
            entity.PrintText("Attack State Enter");
            waitFrame = 0;
        }
        //업데이트//
        public override void Execute(Character entity)
        {
            if (waitFrame >= 10)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("RFA_Attack"))
                {
                    entity.PrintText("Escape Attack State");
                    entity.ChangeState(Character.eCharacterStates.IDLE);
                }
            }
            waitFrame++;
        }

        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Attack State Out");
        }
    }
    public class Throw : EntityState<Character>
    {
        const string throwTriggerName = "Throw";
        const string holdTriggerName = "Hold";
        private Animator _animator;
        private KeyInput _keyInput;
        private ManageWeapon _manageWeapon;
        private FirePosMovement _firePosMovement;
        private bool _throwInProgress;
        public override void SetUp(Character entity)
        {
            _animator = entity.OwnedAnimator;
            _keyInput = entity.KeyInput;
            _manageWeapon = entity.ManageWepon;
            _firePosMovement = entity.FirePosMovement.SetUp();
            _throwInProgress = false;
        }
        public override void Enter(Character entity)
        {
            _throwInProgress = false;
            _animator.SetTrigger(holdTriggerName);
            entity.PrintText("Throw State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {
            if (!_keyInput.ThrowInput && !_throwInProgress)
            {
                _firePosMovement.RotationUpdated();
                _manageWeapon.OnThrow();
                _animator.SetTrigger(throwTriggerName);
                _throwInProgress = !_throwInProgress;

            }
            if (_throwInProgress)
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("RFA_Movement"))
                {
                    entity.ChangeState(Character.eCharacterStates.IDLE);
                }
            }
            if (!_throwInProgress && _keyInput.AttackInput)
            {
                entity.ChangeState(Character.eCharacterStates.ATTACK);
            }
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Throw State Out");
        }

        
    }
    public class Bash : EntityState<Character>
    {
        private WeaponDetectCollider _weaponDetectCollider;
        private KeyInput _keyInput;
        private BashController _bashController;
        private Vector3 bashTargetVector = Vector3.zero;

        public override void SetUp(Character entity)
        {
            _bashController = entity.BashController;
            _keyInput = entity.KeyInput;
            _weaponDetectCollider = entity.WeaponDetectCollider;
        }
        public override void Enter(Character entity)
        {
            entity.PrintText("Bash State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {
            _weaponDetectCollider.Updated();
            if (!_weaponDetectCollider.IsActive)
            {
                SetOnBashCollider();
            }
            if (_weaponDetectCollider.GetColliderRadius() < _weaponDetectCollider.ColliderMaxRadius)
            {
                ExpandBashCollider();
            }
            if (!_keyInput.IsBashKeyPressed)
            {
                if (_weaponDetectCollider.IsFindBashWeapon)
                {
                    SetBashVector(entity);
                    SetPlayerVitality(entity);
                    entity.ChangeState(Character.eCharacterStates.BASHPROGRESS);
                    return;
                }
                entity.ChangeState(Character.eCharacterStates.MOVE);
            }
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            SetOffBashCollider();
            entity.PrintText("Bash State Out");
        }
        private void SetOnBashCollider()
        {
            _weaponDetectCollider.SetOnDetectCollider();
            _weaponDetectCollider.PlaySound();
        }
        private void SetOffBashCollider()
        {
            _weaponDetectCollider.SetOffDetectCollider();
        }
        private void ExpandBashCollider()
        {
            if (_weaponDetectCollider.IsActive)
            {
                _weaponDetectCollider.ExpandRadius();
            }
        }

        private void SetPlayerVitality(Character entity)
        {
            float targetDistance = Vector3.Distance(entity.transform.position, bashTargetVector);
            entity.VitalityPoint = entity.VitalityPoint - (100 * (targetDistance / _weaponDetectCollider.ColliderDefaultRadius));
        }

        private void SetBashVector(Character entity)
        {
            if (_weaponDetectCollider.IsFindBashWeapon)
            {
                bashTargetVector = _weaponDetectCollider.BashWeaponVector;
            }
            else
            {
                bashTargetVector = entity.transform.position;
            }
            _bashController.SetBashVector(bashTargetVector);
        }
    }
    public class BashProgress : EntityState<Character>
    {
        private BashController _bashController;
        private Vector3 bashWeaponVector = Vector3.zero;
        private Vector3 bashAfterMoveDir = Vector3.zero;
        private float _bashSpeed = 10.0f;
        private float _bashDistance = 0.0f;

        public override void SetUp(Character entity)
        {
            _bashController = entity.BashController;
            _bashSpeed = _bashController.BashSpeed;
        }
        public override void Enter(Character entity)
        {
            entity.PrintText("BashProgress State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {
            BashMovement(entity);
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("BashProgress State Out");
        }
        private void BashMovement(Character entity)
        {

            bashWeaponVector = _bashController.GetBashVector();
            bashAfterMoveDir = bashWeaponVector - entity.transform.position;
            bashAfterMoveDir = Vector3.Normalize(bashAfterMoveDir);
            _bashDistance = Vector3.Distance(entity.transform.position, bashWeaponVector);
            UnityEngine.Debug.Log(bashWeaponVector);
            if (_bashDistance > 0.1f)
            {
                
                entity.transform.position = Vector3.Lerp(entity.transform.position, bashWeaponVector, _bashSpeed * Time.deltaTime);
            }
            else
            {
                entity.ChangeState(Character.eCharacterStates.MOVE);
                ///
                ///   moveDirection = 5 * bashAfterMoveDir;
                ///   moveDirection.y = -0.1f;
            }
        }
    }
    public class Skill : EntityState<Character>
    {
        public override void Enter(Character entity)
        {
            entity.PrintText("Skill State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {

        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Skill State Out");
        }
    }
    public class Dash : EntityState<Character>
    {
        public override void Enter(Character entity)
        {
            entity.PrintText("Dash State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {

        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Dash State Out");
        }
    }
    public class Damaged : EntityState<Character>
    {
        public override void Enter(Character entity)
        {
            entity.PrintText("Damaged State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {

        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Damaged State Out");
        }
    }
    public class Die : EntityState<Character>
    {
        public override void Enter(Character entity)
        {
            entity.PrintText("Die State Enter");
        }
        //업데이트//
        public override void Execute(Character entity)
        {

        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Character entity)
        {
            entity.PrintText("Die State Out");
        }
    }
}

