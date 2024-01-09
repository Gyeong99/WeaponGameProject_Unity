using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    private Dictionary<KeyCode, Action <Character>> keyCharacterDictionary;
    private Dictionary<KeyCode, Action> keyOtherDictionary;
    private Vector2 movementInput;
    private bool _jumpInput;
    private Vector2 mouseInput;
    private bool _attackInput;
    private bool _throwInput;
    private bool _isBashKeyPressed;
    private bool changeCameraModeInput;
    private bool _weaponPickUpInput;
    private bool _skillSwordRainInput;

    public Dictionary<KeyCode, Action<Character>> KeyCharacterDictionary { get => keyCharacterDictionary; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
    public bool JumpInput { get => _jumpInput; }
    public Vector2 MouseInput { get => mouseInput; }
    public bool ThrowInput { get => _throwInput; }
    public bool AttackInput { get => _attackInput; }
    public bool ChangeCameraModeInput { get => changeCameraModeInput; }

    public bool WeaponPickUpInput { get => _weaponPickUpInput; }    

    public bool IsBashKeyPressed { get => _isBashKeyPressed; }
    public bool SkillSwordRainInput { get => _skillSwordRainInput; }
    void Start()
    {
        keyCharacterDictionary = new Dictionary<KeyCode, Action <Character>>
        {
            { KeyCode.LeftShift, BashInput},
        };
    }

    void Update()
    {
        movementInput.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _throwInput = Input.GetMouseButton(1);
        _attackInput = Input.GetMouseButtonDown(0);
        _jumpInput = Input.GetButton("Jump");
        changeCameraModeInput = Input.GetKeyDown(KeyCode.F);
        _weaponPickUpInput = Input.GetKeyDown(KeyCode.Q);
        _isBashKeyPressed = CheckBashKeyPressed();
        _skillSwordRainInput = Input.GetKeyDown(KeyCode.H);
    }

    private void BashInput(Character character)
    {
        character.ChangeState(Character.eCharacterStates.BASH);
    }



    private bool CheckBashKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isBashKeyPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isBashKeyPressed = false;
        }

        return _isBashKeyPressed;
    }
}
