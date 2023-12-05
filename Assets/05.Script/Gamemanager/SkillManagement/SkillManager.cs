using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject spawnedSword;
    private enum State {Idle, SwordRain };
    private Skill skill = null;
    private GameObject playerObject = null;
    private PlayerInput _playerInput;
    private Transform playerTransform = null;
    private Vector3 spawnPoint;
    private int spawnPerFrame = 0;
    private void Start()
    {
        skill = new Skill(new Skill_Idle());
        skill.OnStartState();
        playerObject = GameObject.FindWithTag("Player");
        _playerInput = playerObject.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_playerInput.Skill01Input)
        {
            spawnPerFrame = 0;
            skill.SetSkillState(new Skill_SwordRain());
        }
    }

    private void FixedUpdate()
    {
        
        switch (skill.GetSkillState(this.skill))
        {
            case Skill_Idle:
                {
                    //Debug.Log("Idle");
                    break;
                }
            case Skill_SwordRain:
                {
                    if (spawnPerFrame >= 100)
                    {
                        skill.SetSkillState(new Skill_Idle());
                    }
                    if (spawnPerFrame % 6 == 0)
                    {
                        SpawnSword();
                    }
                    break;
                }
            default: break;
        }
        spawnPerFrame++;
    }

    private void SpawnSword()
    {
        playerTransform = playerObject.transform;
        
        Vector3 pos =  playerTransform.position + (Random.insideUnitSphere * 10f);

        pos.y = playerTransform.position.y + 6.0f;
        //높이는 0으로

        // 그 자리에 오브젝트를 생성시킵니다.
        GameObject ob = Instantiate(spawnedSword, pos, Quaternion.identity);
        ob.GetComponentInChildren<Weapon>().ChangeBoolSpawned();
        ob.transform.Rotate(Vector3.right * 180);
        Destroy(ob, 5f);
        Debug.Log("SwordRainActivated");
        
    }
}
