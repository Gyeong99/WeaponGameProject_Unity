using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private string[] arrayMonsters;
    [SerializeField]
    private GameObject monsterPrefab;
    private List<MonsterBaseEntity> Monsterentitys;
    private Vector3[] arrayMonsterSpawnPoints;
    private void Awake()
    {
        Monsterentitys = new List<MonsterBaseEntity>();
        arrayMonsterSpawnPoints = GetComponentInChildren<MonsterSpawnPoint>().GetArraySpawnPoint();
        for (int i = 0; i < arrayMonsters.Length; i++)
        {
            GameObject clone = Instantiate(monsterPrefab);
            Monster monsterEntity = clone.GetComponent<Monster>();
            monsterEntity.SetUp(arrayMonsters[i]);
            Monsterentitys.Add(monsterEntity);
            if (arrayMonsterSpawnPoints.Length > i)
            {
                Debug.Log(arrayMonsterSpawnPoints.Length);
                clone.transform.position = arrayMonsterSpawnPoints[i];
            }  
            else
                monsterEntity.PrintText("Error : monsterSpawnPoint is less than monsterCount");
        }
    }
    private void Update()
    {
        for (int i = 0; i < Monsterentitys.Count; i++)
        {
            Monsterentitys[i].Updated();
        }
    }

}
