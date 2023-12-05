using System.Threading;
using UnityEngine;

// 동일한 객체가 생성되어도 동일한 이름의 클래스를 사용할 수 있게끔 네임스페이스를 구분한다.
namespace MonsterOwnedStates
{
    public class Monster_Idle : MonsterState {
        public override void Enter(Monster entity)
        {
            entity.StopAllCoroutines();
            entity.CurrentState = eMonsterStates.IDLE;
            entity.SetAnimatorBool("Idle");
            entity.PrintText("IdleStateEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            if (entity.IsDetectPlayer)
            {
                entity.ChangeState(eMonsterStates.MOVE);
            }
        }
        //해당 상태 종료시 1회 호출//
        public  override void Exit(Monster entity)
        {
            entity.PrintText("This monster is outta in Idle State");
        }
    }

    public class Monster_Move : MonsterState {
        public override void Enter(Monster entity)
        {
            entity.SetAnimatorBool("WalkForward");

            entity.PrintText("MoveStateEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            if (entity.IsDetectPlayer)
            {
                entity.PrintText("Detected Player");
                entity.ChasePlayer();
                if (Vector3.Distance(entity.transform.position , entity.TargetTr.position) <= 5.0f )
                {
                    entity.ChangeState(eMonsterStates.ATTACK);
                    entity.BattleThinkSet(true);
                }
            }
            else
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }


            
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Monster entity)
        {
            entity.PrintText("MoveStateOut");
        }
    }

    public class Monster_Jump : MonsterState
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.JUMP;
            entity.SetAnimatorTrigger("Jump");

            entity.PrintText("JumpStateEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 5.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Attack : MonsterState
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.ATTACK;
            entity.SetAnimatorTrigger("Attack1");
            entity.PrintText("AttackStateEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 5.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }
    public class Monster_SpecialAttack : MonsterState
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.SPECIALATTACK;
            entity.SetAnimatorTrigger("Attack5");

            entity.PrintText("monsterSpecialAttackEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 6.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
           
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Skill : MonsterState
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.SKILL;
            entity.SetAnimatorTrigger("Buff");

            entity.PrintText("MonsterSkillStateEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 6.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
         
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Die : MonsterState
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.DIE;
            entity.SetAnimatorBool("Death");

            entity.PrintText("DieStateEnter");
        }
        //업데이트//
        public override void Execute(Monster entity)
        {
            //entity.PrintText("This monster is idle State");
        }
        //해당 상태 종료시 1회 호출//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }
}


