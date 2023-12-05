using UnityEngine;
using NaughtyCharacter;
using TMPro;


[RequireComponent(typeof(PlayerInput))]

[RequireComponent(typeof(ManageWeapon))]
public class Combat : MonoBehaviour
{
    private const string attackTriggerName = "Attack";
    private const string holdTriggerName = "Hold";
    private const string throwTriggerName = "Throw";

    
    private PlayerInput _playerInput;
    private ManageWeapon _manageWeapon;
    public Animator _animator;
    private bool lastHoldBool = false;
    private bool lastBashBool = false;
    public bool AttackInProgress { get; private set; } = false;
    public bool HoldInProgress { get; private set; } = false;
    public bool ThrowInProgress { get; private set; } = false;
    public bool ThrowTrigger { get; private set; } = false;

    public bool BashHold { get; private set; } = false;

    public bool BashInProgress { get; private set; } = false;
    public bool BashTrigger { get; private set; } = false;
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _manageWeapon = GetComponent<ManageWeapon>();   
        HoldInProgress = false;
    }
    private void Update()
    {

        if (_playerInput.AttackInput && !AttackInProgress)
        {
            AnimAttack();
        }
        else if (_playerInput.SpecialAttackInput && 
            _manageWeapon.IsHaveWeapon && 
            !HoldInProgress && 
            !ThrowInProgress &&
            !BashHold)        // 처음 던지기 눌렀을 때
        {
            HoldInProgress = true;
            lastHoldBool = true;
            AnimHoldStart();
        }
        else if (_playerInput.BashInput && !HoldInProgress && !BashHold)
        {
            BashHold = true;
            lastBashBool = true;
        }

        CheckBashHold();

        CheckThrowProgress();

        CheckThrowEnd();                    // 던지기 동작 끝나는지 체크


    }
    private void CheckThrowProgress()
    {
        if (_playerInput.IsSpecialKeyHold)      // 누르고 있는지 검사
        {
            //HoldInProgress = true;
        }
        else
        {
            HoldInProgress = false;
        }


        if (!HoldInProgress)                // 눌렀다가 뗐을 때 던짐
        {
            if (lastHoldBool)
            {
                lastHoldBool = false;
                ThrowTrigger = true;
                ThrowInProgress = true;
                AnimThrow();
            }
        }
    }

    
    private void CheckThrowEnd()
    {
        if (ThrowInProgress)
        {
            if (BashTrigger)
            {
                ThrowInProgress = false;                    //던지기 작업중 배쉬 발동시 던지기 작업 끝내기
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("RFA_Movement"))
            {
                ThrowInProgress = false;
            }
        }

    }

    private void CheckBashHold()
    {
        if (!_playerInput.IsBashKeyHold)      // 누르고 있는지 검사
        {
            BashHold = false;
        }

        if (!BashHold)                // 눌렀다가 뗐을 때 발동
        {
            if (lastBashBool)
            {
                lastBashBool = false;
                ChangeBoolBashTrigger();
            }
        }
    }
    private void SetAttackStart()
    {
        AttackInProgress = true;
    }

    private void SetAttackEnd()
    {
        AttackInProgress = false;
    }

    public void ChangeBoolThrowTrigger()
    {
        ThrowTrigger = !ThrowTrigger;
    }

    public void ChangeBoolBashTrigger()
    {
        BashTrigger = !BashTrigger;
    }

    public void ChangeBoolBashInProgress()
    {
        BashInProgress = !BashInProgress;           //BahsinProgress 꺼질때만 대부분 사용함.
    }

    private void AnimAttack()
    {
        _animator.SetTrigger(attackTriggerName);
    }

    private void AnimHoldStart()
    {
        _animator.SetTrigger(holdTriggerName);
    }

    private void AnimThrow()
    {
        _animator.SetTrigger(throwTriggerName);
    }
}
