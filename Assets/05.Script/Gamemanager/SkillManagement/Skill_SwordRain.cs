using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SwordRain : SkillState
{
    public GameObject SpawnObject;
    
    public void OnStartState()
    {
        Debug.Log("successed using SwordRain FixedUpdateState");
        
        OnFixedUpdateState();
    }

    public void OnUpdateState()
    {
        Debug.Log("successed using UpdateState");
    }

    public void OnFixedUpdateState()
    {
        
        Debug.Log("successed using SwordRain FixedUpdateState");
    }

    public void OnExitState()
    {
        
        Debug.Log("Skill_SwordRainState Exit");
    }

   
}
