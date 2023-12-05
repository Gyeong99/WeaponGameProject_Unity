using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private GameObject monsterSword;
    private Animator _animator;
    private Transform target;
    private NavMeshAgent nmAgent;
    private const string attackTriggerName = "Attack";

    private bool _isHaveWeapon = false;
    private float timeSpan = 5.0f;
    private float checkTime = 5.0f;
    float HP = 0;
    public float lostDistance;

    public bool IsHaveWeapon { get => _isHaveWeapon; }
    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        WEAPONFOUND,
        KILLED
    }

    State state;

    // Start is called before the first frame update
    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        HP = 10;
        state = State.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (HP > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        
        // ���Ͱ� Idle ������ �� �θ��� �Ÿ��� �ϴ� �ڵ�
        // 50% Ȯ���� ��/��� ���� ����
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        // ȸ�� �ӵ� ����
        float lookSpeed = Random.Range(25f, 40f);

        // IdleNormal ��� �ð� ���� ���ƺ���
        for (float i = 0; i < 1.0f; i += Time.deltaTime)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + (dir) * Time.deltaTime * lookSpeed, 0f);
            yield return null;
        }
    }

    IEnumerator CHASE()
    {
        
        // ��ǥ������ ���� �Ÿ��� ���ߴ� �������� �۰ų� ������
        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            // StateMachine �� �������� ����
            ChangeState(State.ATTACK);
        }
        // ��ǥ���� �Ÿ��� �־��� ���
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);
            ChangeState(State.IDLE);
            AnimIdle();
            yield return null;
            // StateMachine �� ���� ����
            
        }
        else
        {
            AnimWalkFwd();
            // WalkFWD �ִϸ��̼��� �� ����Ŭ ���� ���
            //yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 2f);
        }
    }
    IEnumerator WEAPONFOUND()
    {
        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            Monster_WeaponGet();
            target = null;
            ChangeState(State.IDLE);
        }
        // ��ǥ���� �Ÿ��� �־��� ���
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);
            ChangeState(State.IDLE);
            yield return null;
        }
        else
        {
            //AnimWalkFwd();
            // WalkFWD �ִϸ��̼��� �� ����Ŭ ���� ���
            //yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 2f);
        }
    }

    IEnumerator ATTACK()
    {
        // �Ÿ��� �־�����
        if (_isHaveWeapon && nmAgent.remainingDistance > nmAgent.stoppingDistance)
        {
            // StateMachine�� �������� ����
            ChangeState(State.CHASE);
        }
        else
        {
            // ���� animation �� �� �踸ŭ ���
            // �� ��� �ð��� �̿��� ���� ������ ������ �� ����.
            //yield return new WaitForSeconds(curAnimStateInfo.length * 2f);
            AnimAttack();
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 2f);
            yield return null;
        }

    }

    IEnumerator KILLED()
    {
        yield return null;
    }

    void ChangeState(State newState)
    {
        state = newState;
    }

    private void Monster_WeaponGet()
    {
        _isHaveWeapon = !_isHaveWeapon;
        monsterSword.gameObject.SetActive(true);
        
    }

    private void Monster_WeaponLost()
    {
        _isHaveWeapon = !_isHaveWeapon;
        monsterSword.gameObject.SetActive(false);
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_isHaveWeapon && other.CompareTag("Weapon"))
        {
            target = other.transform;
            
            ChangeState(State.WEAPONFOUND);
            return;
        }
        else if (other.CompareTag("Player"))
        {
            target = other.transform;
            // NavMeshAgent�� ��ǥ�� Player �� ����
            // StateMachine�� �������� ����
            
            ChangeState(State.CHASE);
        }
        if (target != null)
        {
            nmAgent.SetDestination(target.position);
        }
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        // target �� null �� �ƴϸ� target �� ��� ����
        nmAgent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {
        Debug.Log(state);
        if (_isHaveWeapon)
        {
            timeSpan += Time.deltaTime;
            if (timeSpan > checkTime)  // ��� �ð��� Ư�� �ð��� ���� Ŀ���� ���
            {
                
                Monster_WeaponLost();
                timeSpan = 0;
            }
        }

        if(state == State.CHASE)
        {
            Debug.Log("12345");
            AnimWalkFwd();
        }
    }

    private void AnimAttack()
    {
        _animator.SetFloat("InputY", 0);
        _animator.SetTrigger(attackTriggerName);
    }
    private void AnimWalkFwd()
    {
        _animator.SetFloat("InputY", 1);
    }

    private void AnimIdle()
    {
        _animator.SetFloat("InputY", 0);
    }
}