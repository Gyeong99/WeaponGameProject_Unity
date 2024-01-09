using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class BashController : MonoBehaviour
{
    [SerializeField]
    private float _bashSpeed = 10.0f;
    private WeaponDetectCollider _weaponDetectCollider;
    private Vector3 bashTargetVector = Vector3.zero;

    public float BashSpeed {
        get => _bashSpeed;
    }
    public BashController SetUp(Character entity)
    {
        _weaponDetectCollider = entity.WeaponDetectCollider;
        return this;
    }
    public void SetBashVector(Vector3 newBashTargetVector)
    {
        bashTargetVector = newBashTargetVector;
    }
    public Vector3 GetBashVector()
    {
        if (_weaponDetectCollider.IsFindBashWeapon)
        {
            return bashTargetVector;
        }
        else
            return transform.position;
    }
}

