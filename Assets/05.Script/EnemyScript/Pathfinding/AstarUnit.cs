using UnityEngine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

public class AstarUnit : MonoBehaviour
{


    public Transform target;
    public IEnumerator followPath;
    float speed = 3;
    long frameCount = 0;
    Vector3[] path;
    Vector3 previousStartNode;
    int targetIndex;


    void Start()
    {
        //pathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        path = new Vector3[0];
    }
    private void FixedUpdate()
    {
        frameCount++;
        //if (frameCount % 16 == 0)
            //pathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        Debug.Log(path.Length);
        previousStartNode = Vector3.zero;
        if (path.Length > 0 && targetIndex < path.Length)
        {
            previousStartNode = path[targetIndex];
        }
        if (pathSuccessful)
        {
            if (newPath.Length > 0)
            {
                if (Vector3.Distance(previousStartNode, newPath[0]) <= 2.0f)
                    return;
            }
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Debug.Log(path.Length);
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    previousStartNode = Vector3.zero;
                    path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

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
}


