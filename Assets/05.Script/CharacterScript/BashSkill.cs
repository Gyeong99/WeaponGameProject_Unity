using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(Combat))]
    [RequireComponent(typeof(CharacterStatus))]
    public class BashSkill : MonoBehaviour
    {
        [SerializeField] private GameObject WeaponDetectCollider;
        [SerializeField] private float bashAccSpeed = 0.0f;
        private Combat _combat;
        private CharacterController _characterController;
        private CharacterStatus _characterStatus;
        private WeaponDetectCollider _WeaponDetectCollider;
        private bool isBash = false;
        private Vector3 bashTargetVector = Vector3.zero;
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _combat = GetComponent<Combat>();
            _characterStatus = GetComponent<CharacterStatus>();
            _WeaponDetectCollider = WeaponDetectCollider.GetComponent<WeaponDetectCollider>();
        }

        private void FixedUpdate()
        {
            if (_combat.BashHold)
            {
                if (!_WeaponDetectCollider.IsActive)
                {
                    SetOnBashCollider();
                }
                if (WeaponDetectCollider.GetComponent<SphereCollider>().radius < _WeaponDetectCollider.ColliderMaxRadius)
                {
                    ExpandBashCollider();
                }
            }
            
            if (_combat.BashTrigger)
            {
                if (_WeaponDetectCollider.IsFindBashWeapon)
                {
                    SetBashVector();
                    SetPlayerVitality();
                    _combat.ChangeBoolBashInProgress();
                }
                SetOffBashTrigger();
                SetOffBashCollider();
            }
        }



        private void SetOnBashCollider()
        {
            _WeaponDetectCollider.SetOnDetectCollider();
            _WeaponDetectCollider.PlaySound();
        }
        private void SetOffBashCollider()
        {
            _WeaponDetectCollider.SetOffDetectCollider();
        }
        private void ExpandBashCollider()
        {
            if (_WeaponDetectCollider.IsActive)
            {
                _WeaponDetectCollider.ExpandRadius();
            }
        }

        private void SetPlayerVitality()
        {
            _characterStatus.ReduceVitalityOnTriggerBash(bashTargetVector , _WeaponDetectCollider.ColliderDefaultRadius);
        }
        private void SetOffBashTrigger()
        {
            _combat.ChangeBoolBashTrigger();
        }
  

        private void SetBashVector()
        {
            if (_WeaponDetectCollider.IsFindBashWeapon)
            {
                bashTargetVector = _WeaponDetectCollider.BashWeaponVector;
            }
            else
            {
                bashTargetVector = transform.position;
            }
               
        }
        public Vector3 GetBashVector()
        {
            if (_WeaponDetectCollider.IsFindBashWeapon)
            {
                return bashTargetVector;
            }
            else
                return transform.position;
        }
    }
}


