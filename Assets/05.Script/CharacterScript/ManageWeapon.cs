using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(PlayerInput))]


public class ManageWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Transform _firePos;
    private GameObject copiedWeapon;
    private Combat _combat;
    private PlayerInput _playerInput;
    private Collider[] closeWeaponArr;
    private Weapon closeWeapon = null;
    [SerializeField]private float weaponPickUpDistance = 5.0f;
    private float weaponShortestDistance = 0.0f;
    private bool _isHaveWeapon = true;
    public bool IsHaveWeapon { get => _isHaveWeapon; }
    
    
    private void Start()
    {
        _combat = GetComponent<Combat>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void LateUpdate()
    {
        if (_combat.ThrowTrigger)
        {
            OnThrow();
            _combat.ChangeBoolThrowTrigger();
        }

        if (_playerInput.WeaponPickUpInput)
        {
            FindWeapon();
        }
    }

    private void OnThrow()
    {
        CreateWeapon();
        _isHaveWeapon = !_isHaveWeapon;
    }
    private void CreateWeapon()
    {
        copiedWeapon = Instantiate(_weapon, _firePos.position, _firePos.rotation);
        copiedWeapon.transform.rotation = _firePos.rotation;
        copiedWeapon.GetComponent<Weapon>().ChangeBoolThrown();
    }

    private void FindWeapon() 
    {
        if (_isHaveWeapon)
        {
            Debug.Log("youHaveWeapon");
            return;
        }
        closeWeaponArr = Physics.OverlapSphere(transform.position, weaponPickUpDistance , 1 << LayerMask.NameToLayer("Weapon"));
        
        Debug.Log(closeWeaponArr.Length);
        if (closeWeaponArr.Length > 0)
        {
            Debug.Log("Found Weapon");
            closeWeapon = closeWeaponArr[0].transform.GetComponent<Weapon>();
            
            weaponShortestDistance = Vector3.Distance(transform.position, closeWeapon.transform.position);
            for (int i = 0; i < closeWeaponArr.Length; i++)
            {
                if (Vector3.Distance(transform.position , closeWeaponArr[i].transform.position) < weaponShortestDistance)
                {
                    closeWeapon = closeWeaponArr[i].transform.GetComponent<Weapon>();
                    weaponShortestDistance = Vector3.Distance(transform.position, closeWeapon.transform.position);
                }
            }
        }

        if (closeWeapon != null)
        {
            Debug.Log(closeWeapon.transform.position);
            if (closeWeapon.GetComponentInChildren<WeaponCollider>().IsCanPickUp)
            {
                PickUpWeapon(closeWeapon);
                Debug.Log("PickUpWeaponSuccesfully");
            }
            else
            {
                Debug.Log("error : Weapon , BoolCanPickUp is false");
            }
        }
        else
        {
            Debug.Log("closeWeapon is NUll");
        }

    }

    private void PickUpWeapon(Weapon weapon)
    {
        _isHaveWeapon = !_isHaveWeapon;
        weapon.DestroyWeapon();
        closeWeapon = null;
        closeWeaponArr = null;
    }
}




