using NaughtyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetectCollider : MonoBehaviour
{
    [SerializeField] private Transform _springArmTr;
    private SphereCollider _sphereCollider;
    private GameObject _colliderMesh;
    private Character _character;

    private Vector3 dirVector = Vector3.zero;
    private Vector3 hitVector = Vector3.zero;
    private Vector3 _bashWeaponVector = Vector3.zero;

    private float _colliderMaxRadius = 20.0f;
    private float _colliderDefaultRadius = 20.0f;

    private Color _rayColor = Color.red;
   
    private AudioSource _audioSource;
    private bool _isFindBashWeapon = false;
    private bool _isActive = false;

    public Vector3 BashWeaponVector { get => _bashWeaponVector; }
    public bool IsFindBashWeapon { get => _isFindBashWeapon; }

    public bool IsActive { get => _isActive; }

    public float ColliderMaxRadius { get => _colliderMaxRadius; }
    public float ColliderDefaultRadius { get => _colliderDefaultRadius; }
    private int layerMaskWeapon = 0;
   
    public WeaponDetectCollider SetUp(Character entity)
    {
        _character = entity;
        _springArmTr = _character.SpringArmTransform;
        layerMaskWeapon = LayerMask.GetMask("Weapon");
        _sphereCollider = GetComponent<SphereCollider>();
        _colliderMesh = transform.GetChild(0).gameObject;
        _audioSource = GetComponent<AudioSource>();
        _isActive = false;
        ChangeActive(_isActive);
        return this;
    }
    public void Updated()
    {
        dirVector = _springArmTr.forward;
        dirVector = Vector3.Normalize(dirVector);
        ChangeRadiusAsVitality();
        FindBashWeapon();
    }

    private void ChangeActive(bool bActive)
    {
        gameObject.SetActive(bActive);
    }
    private void ChangeRadiusAsVitality()
    {
        _colliderMaxRadius = _colliderDefaultRadius * _character.VitalityPoint * 0.01f;
    }


    private void FindBashWeapon()
    {
        if (Physics.SphereCast(transform.position, 0.3f, dirVector, out RaycastHit hit, _sphereCollider.radius + 1.0f, layerMaskWeapon))
        {
            if (hit.transform.gameObject.tag == "Weapon" && hit.transform.GetComponent<Weapon>().isCanBash)
            {
                _isFindBashWeapon = true;
                _bashWeaponVector = hit.transform.position;
                hit.transform.GetComponent<Weapon>().ChangeTriggerBashBool(true);
            }
        }
        else
        {
            _isFindBashWeapon = false;
            _bashWeaponVector = Vector3.zero;
        }
    }

    public void ExpandRadius()
    {
        if (_isActive)
        {
            _sphereCollider.radius += 0.1f;
            _colliderMesh.transform.localScale = Vector3.one * (_sphereCollider.radius * 2);
        }
    }

    public void SetOnDetectCollider()
    {
        _isActive = true;
        ChangeActive(_isActive);
    }
    public void SetOffDetectCollider()
    {
        _sphereCollider.radius = 1.0f;
        _isActive = false;
        ChangeActive(_isActive);
    }
    public float GetColliderRadius()
    {
        return _sphereCollider.radius;
    }

   
    public void PlaySound()
    {
        _audioSource.Play();
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;
        float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);


        if (_isActive)
        {
            // 함수 파라미터 : 현재 위치, Sphere의 크기(x,y,z 중 가장 큰 값이 크기가 됨), Ray의 방향, RaycastHit 결과, Sphere의 회전값, SphereCast를 진행할 거리
            if (Physics.SphereCast(transform.position, 0.01f, dirVector, out RaycastHit hit, _sphereCollider.radius + 5.0f, layerMaskWeapon))
            {
                if (hit.transform.gameObject.tag == "Weapon")
                {
                    // Hit된 지점까지 ray를 그려준다.
                    Gizmos.DrawRay(transform.position, transform.forward * hit.distance);


                    // Hit된 지점에 Sphere를 그려준다.
                    Gizmos.DrawWireSphere(transform.position + hitVector, sphereScale / 2.0f);
                }
                else
                {
                    Gizmos.DrawRay(transform.position, dirVector * _sphereCollider.radius);
                }
            }
            else
            {
                // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
                Gizmos.DrawRay(transform.position, dirVector * _sphereCollider.radius);
            }
        }
        

    }

  

}