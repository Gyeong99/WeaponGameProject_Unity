using UnityEngine;


    public class PlayerInput : MonoBehaviour
    {
        private bool _attackInput;
        private bool _specialAttackInput;
        private bool isMousePressed;
        private Vector2 _movementInput;
        private bool _jumpInput;
        private bool _changeCameraModeInput;
        private bool _weaponPickUpInput;
        private bool _bashInput;
        private bool _skill01Input;
        private bool isBashPressed;
        private Vector2 _mouseInput;

        public bool AttackInput {get => _attackInput;}
        public bool SpecialAttackInput {get => _specialAttackInput;}
        public bool IsSpecialKeyHold { get => CheckSpecialKeyHold(); }
        public Vector2 MovementInput {get => _movementInput;}
        public bool JumpInput { get => _jumpInput; }
        public bool ChangeCameraModeInput {get => _changeCameraModeInput;}
        public bool WeaponPickUpInput { get => _weaponPickUpInput; }
        
        public bool BashInput { get => _bashInput;}

        public bool Skill01Input { get => _skill01Input;  }
        public bool IsBashKeyHold { get => CheckBashKeyHold(); }
        public Vector2 MouseInput { get => _mouseInput;}


        
        private void Update()
        {
            _attackInput = Input.GetMouseButtonDown(0);
            _specialAttackInput = Input.GetMouseButtonDown(1);
            _movementInput.Set(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            _jumpInput = Input.GetButton("Jump");
            _changeCameraModeInput = Input.GetKeyDown(KeyCode.F);
            _weaponPickUpInput = Input.GetKeyDown(KeyCode.Q);
            _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _bashInput = Input.GetKeyDown(KeyCode.LeftShift);
            _skill01Input = Input.GetKeyDown(KeyCode.H);
        }

        private bool CheckSpecialKeyHold()
        {
            if (Input.GetMouseButtonDown(1))
            {
                isMousePressed = true;
            }

            // 마우스 왼쪽 버튼이 떼졌을 때
            if (Input.GetMouseButtonUp(1))
            {
                isMousePressed = false;
            }

           return isMousePressed;
        }

        private bool CheckBashKeyHold()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isBashPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isBashPressed = false;
            }

            return isBashPressed;
        }
    }
