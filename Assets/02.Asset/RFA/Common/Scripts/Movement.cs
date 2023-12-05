using UnityEngine;
using UnityEngine.UIElements;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Combat))]
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Animator _animator_2;
        [SerializeField] private float bashSpeed = 0.0f;
        private Animator _animator;
        private PlayerInput _playerInput;
        private Combat _combat;
        private CharacterController _characterController;
        private BashSkill _bashSkill;

        private Vector2 lastMovementInput;
        private Vector3 moveDirection = Vector3.zero;
        private Vector3 playerMoveDir = Vector3.zero;
        private Vector3 bashWeaponVector = Vector3.zero;
        private Vector3 bashAfterMoveDir = Vector3.zero;
        public Transform modelTr;
        [SerializeField] private Transform springArmTr;


        public float gravity = 10;
        public float jumpSpeed = 4; 
        public float MaxSpeed = 10;
        private float bashDistance = 0.0f;
        private float DecelerationOnStop = 0.00f;

        

        

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            _combat = GetComponent<Combat>();
            _characterController = GetComponent<CharacterController>();
            _bashSkill = GetComponent<BashSkill>();
            _animator = _animator_2;
        }

        private void Update()
        {
            if (_animator == null) return;

            if (_combat.AttackInProgress && _characterController.isGrounded)
            {   
                StopMovementOnAttack();
            }
            else if (_combat.HoldInProgress && _characterController.isGrounded)
            {
                StopMovementOnHold();
            }
            else if (_combat.ThrowInProgress && _characterController.isGrounded)
            {
                if (_combat.BashInProgress)
                {
                    BashMovement();
                }
                else
                {
                    //Debug.Log(_combat.ThrowInProgress);
                    StopMovementOnThrow();
                }
            }
            else if (_combat.BashHold && _characterController.isGrounded)
            {
                StopMovementOnBash();
            }
            else if (_combat.BashInProgress)
            {
                BashMovement();                 // 행동 순서 사이드 이펙트 주의 (배쉬 무브먼트 함수의 우선순위가 높아지면 사이드 이펙트 발생 가능성 있음.)
            }
            else
            {
                Move();
            }
            
        }
        private void Move()
        {
            var x = _playerInput.MovementInput.x;
            var y = _playerInput.MovementInput.y;
            bool grounded = _characterController.isGrounded;

            CharacterRotate_ThroughCamera();
            if (grounded)
            {
                moveDirection = new Vector3(x, 0, y);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= MaxSpeed;
                if (Mathf.Abs(x) > 0.2 || Mathf.Abs(y) > 0.2)
                    RotateModel();
                if (_playerInput.JumpInput)
                    moveDirection.y = jumpSpeed;
            }

            moveDirection.y -= gravity * Time.deltaTime;
            _characterController.Move(moveDirection * Time.deltaTime);
            _animator.SetFloat("InputX", x);
            _animator.SetFloat("InputY", y);
            _animator.SetBool("IsInAir", !grounded);
        }

        private void BashMovement()
        {
            
            bashWeaponVector = _bashSkill.GetBashVector();
            bashAfterMoveDir = bashWeaponVector - transform.position;
            bashAfterMoveDir = Vector3.Normalize(bashAfterMoveDir);
            bashDistance = Vector3.Distance(transform.position, bashWeaponVector);
            if (bashDistance > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, bashWeaponVector, bashSpeed * Time.deltaTime);
            }
            else
            {
                _combat.ChangeBoolBashInProgress();     // true -> false
                moveDirection = 5 * bashAfterMoveDir;
                moveDirection.y = -0.1f;
                Debug.Log("Bash End");
            }
        }

        private void CharacterRotate_ThroughCamera()
        {
            playerMoveDir = transform.position - springArmTr.position;
            playerMoveDir = playerMoveDir.normalized;
            playerMoveDir.y = 0f;
            transform.forward = playerMoveDir;
        }
  
        private void RotateModel()
        {
            modelTr.rotation = Quaternion.Lerp
                (modelTr.rotation, Quaternion.LookRotation(moveDirection),
                Time.deltaTime * 10.0f);
        }

        private void StopMovementOnAttack()
        {
            var temp = lastMovementInput;
            temp.x -= DecelerationOnStop;
            temp.y -= DecelerationOnStop;
            lastMovementInput = temp;

            _animator.SetFloat("InputX", lastMovementInput.x);
            _animator.SetFloat("InputY", lastMovementInput.y);
        }

        private void StopMovementOnHold()
        {
            CharacterRotate_ThroughCamera();
        }

        private void StopMovementOnThrow()
        {
            var temp = lastMovementInput;
            temp.x -= DecelerationOnStop;
            temp.y -= DecelerationOnStop;
            lastMovementInput = temp;

            _animator.SetFloat("InputX", lastMovementInput.x);
            _animator.SetFloat("InputY", lastMovementInput.y);
        }

        private void StopMovementOnBash()
        {
            CharacterRotate_ThroughCamera();
        }
    }
}