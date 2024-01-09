using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Vector3[] arraySpawnPoint;
    private int spawnPointCount;
    void Awake()
    {
        spawnPointCount = transform.childCount;
        arraySpawnPoint = new Vector3[spawnPointCount];
        for (int i = 0; i < arraySpawnPoint.Length; i++)
        {
            arraySpawnPoint[i] = transform.GetChild(i).position;
        }
    }

    public Vector3[] GetArraySpawnPoint()
    {
        return arraySpawnPoint;
    }
   
}
