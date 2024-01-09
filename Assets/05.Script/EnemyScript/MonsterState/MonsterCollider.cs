using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    private Monster monster;

    private void Start()
    {
        monster = GetComponentInParent<Monster>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.IsDetectPlayer = true;
            monster.TargetTr = other.transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.IsDetectPlayer = false;
            monster.TargetTr = monster.transform;
        }
    }
}
