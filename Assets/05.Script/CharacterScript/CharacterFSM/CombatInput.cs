using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatInput : MonoBehaviour
{
    private KeyInput _keyInput;
    private Character _character;
    private ManageWeapon _manageWeapon;

    private void Awake()
    {
        _keyInput = FindObjectOfType<KeyInput>().GetComponent<KeyInput>();
        _character = GetComponent<Character>();
        _manageWeapon = GetComponent<ManageWeapon>();
    }

    private void CheckKeyInput()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in _keyInput.KeyCharacterDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value(_character);
                }
            }
        }
        if (_keyInput.WeaponPickUpInput)
        {
            _manageWeapon.FindWeapon();
        }
    }

    public void Updated()
    {
        CheckKeyInput();
    }
    
    
}
