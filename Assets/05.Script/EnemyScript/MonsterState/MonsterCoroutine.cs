using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterCoroutine : MonoBehaviour
{
    private float m_speed = 15;
    private long m_frameCount = 0;
    private Vector3[] path;
    private Vector3 previousStartNode;
    private int m_targetIndex;
    private Monster monster;
    private IEnumerator followPath;

    private PathRequestManager pathRequestManager;
    private void Awake()
    {
        pathRequestManager = new PathRequestManager();
        monster = GetComponent<Monster>();
        path = new Vector3[0];
    }

    public void ChasePlayer(Vector3 targetPos)
    {
        m_frameCount++;
        if (m_frameCount % 16 == 0)
        {
            pathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        previousStartNode = Vector3.zero;
        if (path.Length > 0 && m_targetIndex < path.Length)
        {
            previousStartNode = path[0];
        }
        if (pathSuccessful)
        {
            if (newPath.Length > 0 && path.Length > 0)
            {
                if (Vector3.Distance(previousStartNode, newPath[0]) <= 4.0f)
                {
                    Debug.Log(path.Length);
                    //return;
                }
            }
            path = newPath;
            m_targetIndex = 0;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    public void ExitChase()
    {
        previousStartNode = Vector3.zero;
        path = new Vector3[0];
    }

    public void StartBattleThink()
    {
        StopAllCoroutines();
        StopCoroutine("BattleThink");
        StartCoroutine("BattleThink");
    }

    IEnumerator FollowPath()
    {
        //if (path.Length == 0)
        //yield break;
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            Debug.Log(monster.CurrentState);
            Debug.Log(eMonsterStates.MOVE);
            if (monster.CurrentState != eMonsterStates.MOVE)
            {
                yield break;
            }
            if (transform.position == currentWaypoint)
            {
                m_targetIndex++;
                if (m_targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[m_targetIndex];
            }
            Debug.Log(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, m_speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(currentWaypoint - transform.position, Vector3.up);
            yield return new WaitForSeconds(0.01f);

        }
    }
    IEnumerator BattleThink()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (monster.CurrentState == eMonsterStates.IDLE)
            {
                StopCoroutine("BattleThink");
                yield break;
            }
            int ranAction = Random.Range(0, 100);
            if (ranAction <= 25)
            {
                monster.ChangeState(eMonsterStates.ATTACK);
                yield return new WaitForSeconds(2.5f);
            }
            else if (ranAction <= 50)
            {
                monster.ChangeState(eMonsterStates.SPECIALATTACK);
                yield return new WaitForSeconds(2.5f);
            }
            else if (ranAction <= 75)
            {
                monster.ChangeState(eMonsterStates.JUMP);
                yield return new WaitForSeconds(2.5f);
            }
            else if (ranAction <= 100)
            {
                monster.ChangeState(eMonsterStates.SKILL);
                yield return new WaitForSeconds(5.0f);
            }
        }



    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = m_targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == m_targetIndex)
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
}
