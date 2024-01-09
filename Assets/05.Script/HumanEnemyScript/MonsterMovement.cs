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
        
        // 몬스터가 Idle 상태일 때 두리번 거리게 하는 코드
        // 50% 확률로 좌/우로 돌아 보기
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        // 회전 속도 설정
        float lookSpeed = Random.Range(25f, 40f);

        // IdleNormal 재생 시간 동안 돌아보기
        for (float i = 0; i < 1.0f; i += Time.deltaTime)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + (dir) * Time.deltaTime * lookSpeed, 0f);
            yield return null;
        }
    }

    IEnumerator CHASE()
    {
        
        // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면
        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            // StateMachine 을 공격으로 변경
            ChangeState(State.ATTACK);
        }
        // 목표와의 거리가 멀어진 경우
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);
            ChangeState(State.IDLE);
            AnimIdle();
            yield return null;
            // StateMachine 을 대기로 변경
            
        }
        else
        {
            AnimWalkFwd();
            // WalkFWD 애니메이션의 한 사이클 동안 대기
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
        // 목표와의 거리가 멀어진 경우
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
            // WalkFWD 애니메이션의 한 사이클 동안 대기
            //yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 2f);
        }
    }

    IEnumerator ATTACK()
    {
        // 거리가 멀어지면
        if (_isHaveWeapon && nmAgent.remainingDistance > nmAgent.stoppingDistance)
        {
            // StateMachine을 추적으로 변경
            ChangeState(State.CHASE);
        }
        else
        {
            // 공격 animation 의 두 배만큼 대기
            // 이 대기 시간을 이용해 공격 간격을 조절할 수 있음.
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
            // NavMeshAgent의 목표를 Player 로 설정
            // StateMachine을 추적으로 변경
            
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
        // target 이 null 이 아니면 target 을 계속 추적
        nmAgent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {
        Debug.Log(state);
        if (_isHaveWeapon)
        {
            timeSpan += Time.deltaTime;
            if (timeSpan > checkTime)  // 경과 시간이 특정 시간이 보다 커졋을 경우
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