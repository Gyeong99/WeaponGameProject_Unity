using Retro.ThirdPersonCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Character : CharacterBase
{
    [SerializeField]
    private float _vitalityPoint = 100.0f;
    public enum eCharacterStates
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        THROW,
        BASH,
        BASHPROGRESS,
        SKILL,
        DASH,
        DAMAGED,
        DIE,
    }
    private EntityState<Character> _currentState;
    private EntityState<Character>[] characterStates;
    private CharacterController _characterController;
    private eCharacterStates stateNum;

    private Animator _ownedAnimator;
    private KeyInput _keyInput;
    private CombatInput _combatInput;
    private ManageWeapon _manageWeapon;
    private BashController _bashController;
    private FirePosMovement _firePosMovement;
    private WeaponDetectCollider _weaponDetectCollider;
    private Transform _springArmTransform;
    private CharacterMovement _characterMovement;
    
    public eCharacterStates CurrentState
    {
        set => stateNum = value;
        get => stateNum;
    }
    public float VitalityPoint
    {
        set => _vitalityPoint = value;
        get => _vitalityPoint;
    }

    public CharacterController CharacterController
    {
        get => _characterController;
    }
    public Animator OwnedAnimator
    {
        get => _ownedAnimator;
    }
    public KeyInput KeyInput
    {
        get => _keyInput;
    }
    public ManageWeapon ManageWepon
    {
        get => _manageWeapon;
    }
    public BashController BashController
    {
        get => _bashController;
    }
    public WeaponDetectCollider WeaponDetectCollider { get => _weaponDetectCollider; }
    public Transform SpringArmTransform {
        get => _springArmTransform;
    }
    public FirePosMovement FirePosMovement { get => _firePosMovement; }
    public Transform ModelTr;

    public override void SetUp(string name)
    {
        base.SetUp(name);
        gameObject.name = $"_Player_{name}";
        _springArmTransform = GameObject.FindGameObjectWithTag("SpringArm").GetComponent<Transform>();
        _ownedAnimator = ModelTr.GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _keyInput = FindObjectOfType<KeyInput>();
        _combatInput = GetComponent<CombatInput>();
        _manageWeapon = GetComponent<ManageWeapon>();
        _weaponDetectCollider = GetComponentInChildren<WeaponDetectCollider>().SetUp(this);
        _bashController = GetComponent<BashController>().SetUp(this);
        _firePosMovement = GetComponentInChildren<FirePosMovement>();
        _characterMovement = GetComponent<CharacterMovement>().SetUp(this);
        
        
        StateSetUp();

        ChangeState(eCharacterStates.IDLE);

        PrintText("new Character is born");
        if (_ownedAnimator == null)
        {
            PrintText($"Error : This Entity doesn't have Animator");
        }
    }

    public void StateSetUp()
    {
        int stateCount = Enum.GetNames(typeof(eCharacterStates)).Length;
        characterStates = new EntityState<Character>[stateCount];
        characterStates[(int)eCharacterStates.IDLE] = new CharacterOwnedStates.Idle();
        characterStates[(int)eCharacterStates.MOVE] = new CharacterOwnedStates.Move();
        characterStates[(int)eCharacterStates.ATTACK] = new CharacterOwnedStates.Attack();
        characterStates[(int)eCharacterStates.THROW] = new CharacterOwnedStates.Throw();
        characterStates[(int)eCharacterStates.BASH] = new CharacterOwnedStates.Bash();
        characterStates[(int)eCharacterStates.BASHPROGRESS] = new CharacterOwnedStates.BashProgress();
        characterStates[(int)eCharacterStates.SKILL] = new CharacterOwnedStates.Skill();
        characterStates[(int)eCharacterStates.DASH] = new CharacterOwnedStates.Dash();
        characterStates[(int)eCharacterStates.DAMAGED] = new CharacterOwnedStates.Damaged();
        characterStates[(int)eCharacterStates.DIE] = new CharacterOwnedStates.Die();

        for (int index = 0; index < characterStates.Length; index++)
        {
            characterStates[index].SetUp(this);
        }
    }
    public void ChangeState(eCharacterStates newCharacterState)
    {
        if (characterStates[(int)newCharacterState] == null)
        {
            return;
        }
        
        if (_currentState != null)
            _currentState.Exit(this);
        _currentState = characterStates[(int)newCharacterState];
        CurrentState = newCharacterState;
        _currentState.Enter(this);
    }

    public override void Updated()
    {
        if (_currentState != null)
        {
            _currentState.Execute(this);
        }
        _combatInput.Updated();
        _characterMovement.Updated();
    }
    public override void FixedUpdated()
    {
        UpdateCharacterStatus();
    }

    private void UpdateCharacterStatus()
    {
        UpdateVitality();
    }
    private void UpdateVitality()
    {
        switch (CurrentState) {
            case eCharacterStates.BASH:
                ReduceVitalityOnHoldBash(); break;
            default:
                RecoverVitality(); break;
        }
    }
    private void RecoverVitality()
    {
        if (_vitalityPoint < 100.0f)
        {
            _vitalityPoint += 0.5f;
        }
    }
    private void ReduceVitalityOnHoldBash()
    {
        _vitalityPoint -= 0.1f;
    }
}
