using NaughtyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class FirePosMovement : MonoBehaviour
    {
        [SerializeField ]private SpringArm _springArm;
        
        
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = _springArm.transform.rotation;
        }
    }


