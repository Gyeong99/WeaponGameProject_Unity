using NaughtyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirePosMovement : MonoBehaviour
{
    private Transform _springArm;
    private Character _character;

    public FirePosMovement SetUp()
    {
        _character = GetComponentInParent<Character>();
        _springArm = _character.SpringArmTransform;

        return this;
    }

    public void RotationUpdated()
    {
        transform.rotation = _springArm.transform.rotation;
    }
}


