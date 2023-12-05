using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class WeaponCollider : MonoBehaviour
{

    [SerializeField] private float _collisionLength = 5.0f;
    [SerializeField] private float _collisionHeight = 0.0f;
    [SerializeField] private int horizontalRayCount = 0;
    [SerializeField] private int verticalRayCount = 0;
    [SerializeField] private bool gizmoActivated = false;
    private bool _bDetectCharacter = false;
    private bool _bCanPickUp = false;
    private bool _bCanBash = false;
    private bool _bBashStay = false;
    private Color _rayColor = Color.red;
    private Vector3 collisionPos = Vector3.zero;
    private Vector3 sphereCastDirVector = Vector3.zero;
    private Vector3 gizmoDirVector = Vector3.zero;
    private Vector3 hitVector = Vector3.zero;


    public bool IsCanPickUp { get => _bCanPickUp; }
    public bool IsCanBash { get => _bCanBash; }

    void Start()
    {
        _bDetectCharacter = false;
        _bCanPickUp = false;
        _bCanBash = false;
        _bBashStay = false;
        sphereCastDirVector = Vector3.down;
        gizmoDirVector = Vector3.down;
        collisionPos = transform.position;
        collisionPos.y += _collisionHeight;
    }

    // Update is called once per frame
    void Update()
    {
        GetBoolWeaponPickUp();
    }

    private void FixedUpdate()
    {
        CheckBashColliderDetect();      // Update���� ���� ���� �߻� (Trigger Stay �۵� ���� ����)
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BashCollider")
        {
            _bBashStay = true;                  //bBashStay(OnTriggerStay) �Ҹ� SideEffect ����
        }
    }

    private void GetBoolWeaponPickUp()
    {
        collisionPos = transform.position;
        collisionPos.y += _collisionHeight;
        for (int a = 0; a < verticalRayCount; a++)
        {
            for (int i = 0; i < horizontalRayCount; i++)
            {
                if (Physics.SphereCast(collisionPos, 0.01f, sphereCastDirVector, out RaycastHit hit, _collisionLength))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        _bDetectCharacter = true;
                        hitVector = hit.transform.position - collisionPos;
                    }
                }
                sphereCastDirVector = Quaternion.AngleAxis(360 / horizontalRayCount, Vector3.up) * sphereCastDirVector;
            }
            sphereCastDirVector = Quaternion.AngleAxis(180 / verticalRayCount, Vector3.right) * sphereCastDirVector;
        }

        if (_bDetectCharacter)
        {
            _bCanPickUp = true;
            _bDetectCharacter = false;
        }
        else
            _bCanPickUp = false;


        sphereCastDirVector = Vector3.down;
    }

    private void CheckBashColliderDetect()
    {
        if (_bBashStay)
        {
            _bCanBash = true;
            _bBashStay = false;
        }
        else
        {
            _bCanBash = false;
        }
    }


    void OnDrawGizmos()
    {
        if (gizmoActivated) {
            Gizmos.color = _rayColor;
            float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

            for (int a = 0; a < verticalRayCount; a++)
            {
                for (int s = 0; s < horizontalRayCount; s++)
                {

                    // �Լ� �Ķ���� : ���� ��ġ, Sphere�� ũ��(x,y,z �� ���� ū ���� ũ�Ⱑ ��), Ray�� ����, RaycastHit ���, Sphere�� ȸ����, SphereCast�� ������ �Ÿ�
                    if (Physics.SphereCast(collisionPos, 0.01f, gizmoDirVector, out RaycastHit hit, _collisionLength))
                    {
                        if (hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "BashCollider")
                        {
                            // Hit�� �������� ray�� �׷��ش�.
                            Gizmos.DrawRay(collisionPos, transform.forward * hit.distance);


                            // Hit�� ������ Sphere�� �׷��ش�.
                            Gizmos.DrawWireSphere(collisionPos + hitVector, sphereScale / 2.0f);
                        }
                    }
                    else
                    {
                        // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
                        Gizmos.DrawRay(collisionPos, gizmoDirVector * _collisionLength);
                    }
                    gizmoDirVector = Quaternion.AngleAxis(360 / horizontalRayCount, Vector3.up) * gizmoDirVector;
                }
                gizmoDirVector = Quaternion.AngleAxis(180 / verticalRayCount, Vector3.right) * gizmoDirVector;
            }
            gizmoDirVector = Vector3.down;  
        }
        

    }




}




