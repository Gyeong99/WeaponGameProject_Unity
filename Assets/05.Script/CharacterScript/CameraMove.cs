using NaughtyCharacter;
using Retro.ThirdPersonCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform springArmTr;
    [SerializeField]
    private Transform _targetTr;
    private Vector3 camPointPos = Vector3.zero;
    private SpringArm springArm;
    private bool isAiming;
    private KeyInput keyInput;
    private Vector2 mouseInput;
    private Vector3 camAngle;
    float x = 0.0f;

    [SerializeField] private float _aimCameraDistance = 3;
    [SerializeField] private float _regularCameraDistance = 1f;

    void Start()
    {
        keyInput = FindObjectOfType<KeyInput>();
        springArm = springArmTr.GetComponent<SpringArm>();
        _targetTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if (_targetTr != null)
        {
            CamRotateMouse();
            camPointPos = _targetTr.position;
            camPointPos.y += 1.5f;
            transform.position = camPointPos;
            if (keyInput.ChangeCameraModeInput) SwitchAimZoom();
        }
        else
        {
            _targetTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }
    public void SetCameraTargetTr(Transform newTargetTr)
    {
        _targetTr = newTargetTr;
    }
    private void SwitchAimZoom()
    {
        isAiming = !isAiming;
        OnStateChanged();
    }
    private void OnStateChanged()
    {
        if (isAiming)
        {
            springArm.TargetLength = _aimCameraDistance;
        }
        else
        {
            springArm.TargetLength = _regularCameraDistance;
        }
    }

    private void CamRotateMouse()
    {
        mouseInput = keyInput.MouseInput;
        camAngle = springArmTr.rotation.eulerAngles;
        x = camAngle.x - mouseInput.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        springArmTr.rotation = Quaternion.Euler(x, camAngle.y + mouseInput.x, camAngle.z);
    }
}


