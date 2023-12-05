using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combat))]
public class CharacterStatus : MonoBehaviour
{
    private Combat _combat;
    private float _playerVitality = 0.0f;
    private float _playerHp = 0.0f;
    private float targetDistance = 0.0f;
    public float Vitality { get => _playerVitality;  }
    public float Hp { get => _playerHp; }

    private void Start()
    {
        _combat = GetComponent<Combat>();
        _playerVitality = 100.0f;
        _playerHp = 100.0f;
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (_combat.BashHold)
        {
            ReduceVitalityOnHoldBash();
        }
        else
        {
            RecoverVitality();
        }
        RecoverHp();
    }

    private void RecoverVitality()
    {
        if (_playerVitality < 100.0f)
        {
            _playerVitality += 0.5f;
        }
        
    }

    private void RecoverHp()
    {
        if (_playerHp < 100.0f)
        {
            _playerHp += 0.1f;
        }
    }

    private void ReduceVitalityOnHoldBash()
    {
        _playerVitality -= 0.1f;
    }

    public void ReduceVitalityOnTriggerBash(Vector3 targetVector , float detectColliderMaxRadius)
    {
        targetDistance = Vector3.Distance(transform.position, targetVector);
        _playerVitality = _playerVitality - (100 * (targetDistance / detectColliderMaxRadius));
        Debug.Log(transform.position + " , " + targetVector + " , " + _playerVitality);
    }

    public void ChangeVitality(float vitalityStatus)
    {
        _playerVitality = vitalityStatus;
    }

    public void ChangeHp(float hpStatus)
    {
        _playerHp = hpStatus;
    }
}
