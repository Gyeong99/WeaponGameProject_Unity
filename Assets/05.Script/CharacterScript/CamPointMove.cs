using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPointMove : MonoBehaviour
{
    [SerializeField] private Transform playerTr;
    private Vector3 camPointPos = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camPointPos = playerTr.position;
        camPointPos.y += 1.5f;
        transform.position = camPointPos;
    }
}
