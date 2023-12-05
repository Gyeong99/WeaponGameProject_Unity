using Retro.ThirdPersonCharacter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    [SerializeField] private Material[] _weaponMat = new Material[4];
    [SerializeField] private float pickingDepth = 0.0f;
    [SerializeField] private bool _isSpawnedBySkill = false;
    [SerializeField] private float spawnFallSpeed = 0.1f;
    
    private Vector3 pickingVector = Vector3.zero;
    private WeaponCollider _weaponCollider;
    private Renderer weaponColor;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private bool _isSpawnMovement = false;
    private bool _isThrown = false;
    private bool _isCanBash = false;
    private bool _isTriggerBash = false;
    
    
    public bool iSpawnedBySkill { get => _isSpawnedBySkill; }
    public bool isThrown { get => _isThrown; }
    public bool isTriggerBash { get => _isTriggerBash; }
    public bool isCanBash { get => _isCanBash; }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _weaponCollider = GetComponentInChildren<WeaponCollider>();
        weaponColor = GetComponent<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _isTriggerBash = false;
    }

    private void FixedUpdate()
    {
        CheckSpawnMovementEnd();
        CheckWeaponBashStatus();
        ChangeWeaponColor();
    }

    private void CheckSpawnMovementEnd()
    {
        if (_isSpawnedBySkill)
        {
            if (_isCanBash)
            {
                transform.Translate(new Vector3(0.0f, -spawnFallSpeed * 0.1f, 0.0f), Space.World);  
            }
            else
            {
                transform.Translate(new Vector3(0.0f, -spawnFallSpeed, 0.0f), Space.World);
                spawnFallSpeed += 0.005f;                 // 가속도 운동
            }
        }
        
        if (_isSpawnMovement)
        {
            if (transform.position.y >= pickingDepth + 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position , pickingVector , 0.3f);
            }
            else
            {
                _isSpawnMovement = !_isSpawnMovement;
            }
        }
    }
    private void ChangeWeaponColor()
    {
        if (_isTriggerBash)
        {
            weaponColor.material = _weaponMat[3];
        }           
        else if (_isCanBash)
            weaponColor.material = _weaponMat[1];
        else
            weaponColor.material = _weaponMat[0];
    }

    private void CheckWeaponBashStatus()
    {
        if (_isTriggerBash)
        {
            SetOnTriggerBashBool();
        }
        else if (_isCanBash != _weaponCollider.IsCanBash)
        {
            if (_weaponCollider.IsCanBash)
            {
                SetOnBashBool();
            }
            else
            {
                SetOffBashBool();
                SetOffTriggerBashBool();
            }
        }
        else
        {
            SetOffTriggerBashBool();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor" && _isSpawnedBySkill)
        {
            pickingDepth = transform.position.y - pickingDepth;
            pickingVector = new Vector3(transform.position.x, pickingDepth, transform.position.z);
            _isSpawnMovement = !_isSpawnMovement;
            _isSpawnedBySkill = false;
            _audioSource.Play();
        }
        else if (other.tag == "Player" || other.tag == "Monster" )
        {
            if (_isSpawnedBySkill)
            {
                _isSpawnedBySkill = false;
                Destroy(this.gameObject);
            }


        }
    }

 
    public void DestroyWeapon()
    {
        if (_weaponCollider.IsCanPickUp)
        {
            Destroy(gameObject);
        }
    }

    private void SetOnBashBool()
    {
        _isCanBash = true;
    }


    private void SetOffBashBool()
    {
        _isCanBash= false;
    }

    private void SetOnTriggerBashBool()
    {
        _isTriggerBash = true;
    }

    private void SetOffTriggerBashBool()
    {
        _isTriggerBash = false;
    }

    public void ChangeTriggerBashBool(bool changeBool)
    {
        if (changeBool)
        {
            if (_isCanBash)
            {
                SetOnTriggerBashBool();
            }
        }
        else
        {
            SetOffTriggerBashBool();
        }
    }

    public void ChangeBoolThrown()
    {
        _isThrown = !_isThrown;
    }

    public void ChangeBoolSpawned()
    {
        _isSpawnedBySkill = true;
    }
}



