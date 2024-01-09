using UnityEngine;
using UnityEngine.EventSystems;

public class AimingController : MonoBehaviour
{
    
    private PlayerInput _playerInput;
    private Combat _combat;

#pragma warning disable 0649
    [SerializeField] private bool _isAiming;
    private bool isThrowing = false;
    [SerializeField] private SpringArm _springArm;
    [SerializeField] private Transform modelTr;
    private Quaternion targetRotation;

#pragma warning restore 0649

    [Header("Settings")]
    [SerializeField] private float _aimCameraDistance = 3;
    [SerializeField] private float _regularCameraDistance = 1f;

    private void Start()
    {
        
        _playerInput = GetComponent<PlayerInput>();
        _combat = GetComponent<Combat>();
        OnStateChanged();
    }

    private void Update()
    {
        if (_playerInput.ChangeCameraModeInput) SwitchAim_Zoom();

        if (_playerInput.SpecialAttackInput) SetBoolSwitchAim_Throw();


        if (isThrowing)
            OnSwitchAim_Throw();

    }


    private void SwitchAim_Zoom()
    {
        _isAiming = !_isAiming;
        OnStateChanged();
    }

    private void SetBoolSwitchAim_Throw()
    {
        isThrowing = !isThrowing;
        targetRotation = Quaternion.LookRotation(modelTr.forward);
    }
    private void OnStateChanged()
    {
        if (_isAiming)
        {
            _springArm.TargetLength = _aimCameraDistance;
        }
        else
        {
            _springArm.TargetLength = _regularCameraDistance;
        }
    }

    private void OnSwitchAim_Throw()
    {
        if (Mathf.Abs(_springArm.transform.rotation.eulerAngles.y - Quaternion.LookRotation(modelTr.forward).eulerAngles.y) < 1.0f)
        {
            modelTr.rotation = transform.rotation;
            isThrowing = false;
        }
        //_springArm.transform.rotation = Quaternion.LookRotation(modelTr.forward);
        _springArm.transform.rotation = Quaternion.Lerp(_springArm.transform.rotation,
            targetRotation,
            Time.deltaTime * 10.0f);

        modelTr.rotation = Quaternion.Lerp(modelTr.rotation,
            targetRotation,
             Time.deltaTime * 10.0f);

    }
}
