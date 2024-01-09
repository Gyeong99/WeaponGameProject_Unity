using MonsterOwnedStates;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public enum eMonsterStates { IDLE = 0, MOVE, BATTLE , JUMP, ATTACK, SPECIALATTACK, SKILL, DIE };
public class Monster : MonsterBaseEntity
{
    private int hp;
    private int damage;
    private bool isDetectPlayer;
    
    private EntityState <Monster>[] monsterStates;
    private EntityState <Monster> currentState;
    private MonsterCoroutine monsterCoroutine;
    private Animator animator;
    private Transform targetTr;

    private eMonsterStates stateNum;
    
    private string currentAnimationName = "Idle";
    public int Hp
    {
        set => hp = 100;
        get => hp;
    }

    public int Damage
    {
        set => damage = 10;

        get => damage;
    }

    public bool IsDetectPlayer
    {
        set => isDetectPlayer = value;
        get => isDetectPlayer;
    }

    public Transform TargetTr
    {
        set => targetTr = value;
        get => targetTr;
    }


    public eMonsterStates CurrentState
    {
        set => stateNum = value;
        get => stateNum;
    }

  
    public override void SetUp(string name)
    {
        base.SetUp(name);
        gameObject.name = $"{ID:D2}_BossBear_{name}";
        animator = GetComponentInChildren<Animator>();
        monsterCoroutine = GetComponent<MonsterCoroutine>();

        
        monsterStates = new EntityState<Monster>[8];
        monsterStates[(int)eMonsterStates.IDLE] = new MonsterOwnedStates.Monster_Idle();
        monsterStates[(int)eMonsterStates.MOVE] = new MonsterOwnedStates.Monster_Move();
        monsterStates[(int)eMonsterStates.BATTLE] = new MonsterOwnedStates.Monster_Battle();
        monsterStates[(int)eMonsterStates.JUMP] = new MonsterOwnedStates.Monster_Jump();
        monsterStates[(int)eMonsterStates.ATTACK] = new MonsterOwnedStates.Monster_Attack();
        monsterStates[(int)eMonsterStates.SPECIALATTACK] = new MonsterOwnedStates.Monster_SpecialAttack();
        monsterStates[(int)eMonsterStates.SKILL] = new MonsterOwnedStates.Monster_Skill();
        monsterStates[(int)eMonsterStates.DIE] = new MonsterOwnedStates.Monster_Die();

        ChangeState(eMonsterStates.IDLE);

        hp = 100;

        PrintText("new BossBear is born");
        if (animator == null)
        {
            PrintText($"Error : This Monster doesn't have Animator");
        }
    }

    public void ChangeState(eMonsterStates newMonsterState)
    {
        if (monsterStates[(int)newMonsterState] == null) return;
        if (currentState != null)
            currentState.Exit(this);
        currentState = monsterStates[(int)newMonsterState];
        currentState.Enter(this);
    }

    public void ChasePlayer()
    {
       monsterCoroutine.ChasePlayer(targetTr.position); 
    }

    public void ExitChase()
    {
        monsterCoroutine.ExitChase();
    }

    public void StartBattleThink()
    {
        monsterCoroutine.StartBattleThink();
    }

    public void SetAnimatorBool(string name)
    {
        animator.SetBool(currentAnimationName, false);
        animator.SetBool(name, true);
        currentAnimationName = name;
    }
    public void SetAnimatorTrigger(string name)
    {
        animator.SetBool(currentAnimationName, false);
        animator.SetTrigger(name);
    }
   

    public override void Updated()
    {
        if (currentState != null)
        {
            currentState.Execute(this);

        }
    }
}
