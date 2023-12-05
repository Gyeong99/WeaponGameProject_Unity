using NaughtyCharacter;
using Retro.ThirdPersonCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retro.ThirPersonCharacter {
    [RequireComponent(typeof(PlayerInput))]

    public class CameraMoveMouse : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private Vector2 mouseInput;
        private Vector3 camAngle;
        float x = 0.0f;

        [SerializeField] private Transform springArmTr;
        void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        
        void Update()
        {
           CamMoveMouse();
        }

        private void CamMoveMouse()
        {
            mouseInput = _playerInput.MouseInput;
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
}

