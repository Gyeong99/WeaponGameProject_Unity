using System.Threading;
using UnityEngine;

// ������ ��ü�� �����Ǿ ������ �̸��� Ŭ������ ����� �� �ְԲ� ���ӽ����̽��� �����Ѵ�.
namespace MonsterOwnedStates
{
    public class Monster_Idle : EntityState <Monster> {
        public override void Enter(Monster entity)
        {
            entity.StopAllCoroutines();
            entity.CurrentState = eMonsterStates.IDLE;
            entity.SetAnimatorBool("Idle");
            entity.PrintText("IdleStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            if (entity.IsDetectPlayer)
            {
                entity.ChangeState(eMonsterStates.MOVE);
            }
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public  override void Exit(Monster entity)
        {
            entity.PrintText("This monster is outta in Idle State");
        }
    }

    public class Monster_Move : EntityState <Monster>{
        public override void Enter(Monster entity)
        {
            entity.SetAnimatorBool("WalkForward");
            entity.CurrentState = eMonsterStates.MOVE;
            entity.PrintText("MoveStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            if (entity.IsDetectPlayer)
            {
                entity.PrintText("Detected Player");
                entity.ChasePlayer();
                if (Vector3.Distance(entity.transform.position , entity.TargetTr.position) <= 5.0f )
                {
                    entity.ChangeState(eMonsterStates.BATTLE);
                    entity.StartBattleThink();
                }
            }
            else
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }    
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            entity.PrintText("MoveStateOut");
            entity.ExitChase();
        }
    }
    public class Monster_Battle : EntityState<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.BATTLE;
            entity.SetAnimatorTrigger("Idle");
            entity.PrintText("BattleStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 5.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Jump : EntityState <Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.JUMP;
            entity.SetAnimatorTrigger("Jump");

            entity.PrintText("JumpStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 5.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Attack : EntityState <Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.ATTACK;
            entity.SetAnimatorTrigger("Attack1");
            entity.PrintText("AttackStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }
    public class Monster_SpecialAttack : EntityState <Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.SPECIALATTACK;
            entity.SetAnimatorTrigger("Attack5");

            entity.PrintText("monsterSpecialAttackEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 6.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
           
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Skill : EntityState <Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.SKILL;
            entity.SetAnimatorTrigger("Buff");

            entity.PrintText("MonsterSkillStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.TargetTr.position) >= 6.0f)
            {
                entity.ChangeState(eMonsterStates.IDLE);
            }
         
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }

    public class Monster_Die : EntityState <Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.CurrentState = eMonsterStates.DIE;
            entity.SetAnimatorBool("Death");

            entity.PrintText("DieStateEnter");
        }
        //������Ʈ//
        public override void Execute(Monster entity)
        {
            //entity.PrintText("This monster is idle State");
        }
        //�ش� ���� ����� 1ȸ ȣ��//
        public override void Exit(Monster entity)
        {
            //entity.PrintText("This monster is outta in Move State");
        }
    }
}


