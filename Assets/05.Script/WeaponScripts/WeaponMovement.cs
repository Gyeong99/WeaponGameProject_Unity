using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
    public class WeaponMovement : MonoBehaviour
    {
        private Weapon _weapon;
        public float throwSpeed = 500.0f;
    public float rotateSpeed = 1000.0f;
        void Start()
        {
            _weapon = GetComponent<Weapon>();

        }

        
        void Update()
        {
            if (_weapon.isThrown)
            {
                GetComponent<Rigidbody>().AddForce(transform.forward * throwSpeed);
            GetComponent<Rigidbody>().AddTorque(transform.up * rotateSpeed * 10, ForceMode.Force);
            transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
                _weapon.ChangeBoolThrown();
            }
        }


    }



