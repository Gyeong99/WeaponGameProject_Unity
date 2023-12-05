using MonsterOwnedStates;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public enum eMonsterStates { IDLE = 0, MOVE, JUMP, ATTACK, SPECIALATTACK, SKILL, DIE };
public class Monster : MonsterBaseEntity
{
    private int hp;
    private int damage;
    private bool isDetectPlayer;
    private float speed = 1;
    private Vector3[] path;
    private int targetIndex;
    private MonsterState[] monsterStates;
    private MonsterState currentState;
    private AstarUnit astarUnit;
    private ulong frameCount = 0;
    public Transform targetTr;
    private eMonsterStates stateNum;
    private Animator animator;
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
    public int PathCount
    {
        get => path.Length;
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
    public override void SetUp(string name)
    {
        base.SetUp(name);
        gameObject.name = $"{ID:D2}_BossBear_{name}";
        animator = GetComponentInChildren<Animator>();


        monsterStates = new MonsterState[7];
        monsterStates[(int)eMonsterStates.IDLE] = new MonsterOwnedStates.Monster_Idle();
        monsterStates[(int)eMonsterStates.MOVE] = new MonsterOwnedStates.Monster_Move();
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
        PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);

    }
    
     public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
     {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
     }
     IEnumerator FollowPath()
     {
        if (path.Length == 0)
            yield break;
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (this.currentState != monsterStates[(int)eMonsterStates.MOVE])
                yield break;
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(currentWaypoint - transform.position, Vector3.up);
            yield return null;

        }
    }
    public bool IsEndAnimation()
    {
        return false;
    } 

    public void BattleThinkSet(bool isRun)
    {
        if (isRun)
        {
            StopAllCoroutines();
            StopCoroutine("BattleThink");
            StartCoroutine("BattleThink");
        }
    }
    //Enumerator 사용하지 말고 Animator 작동 시간을 콜백으로 선언하고 하는 방법을 찾아보셈 2023.12.01 1641pm

    IEnumerator BattleThink()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (this.currentState == monsterStates[(int)eMonsterStates.IDLE])
            {
                
            }
            int ranAction = Random.Range(0, 100);
            if (ranAction <= 25)
            {
                this.ChangeState(eMonsterStates.ATTACK);
                yield return new WaitForSeconds(2.5f);
            }
            else if (ranAction <= 50)
            {
                this.ChangeState(eMonsterStates.SPECIALATTACK);
                yield return new WaitForSeconds(2.5f);
            }
            else if (ranAction <= 75)
            {
                this.ChangeState(eMonsterStates.JUMP);
                yield return new WaitForSeconds(2.5f);
            }
            else if (ranAction <= 100)
            {
                this.ChangeState(eMonsterStates.SKILL);
                yield return new WaitForSeconds(5.0f);
            }
        }

        

    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    public override void Updated()
    {
        if (currentState != null)
        {  
            currentState.Execute(this);
        }
    }



}
