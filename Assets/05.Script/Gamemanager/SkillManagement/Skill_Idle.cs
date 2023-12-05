using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Idle : SkillState
{
    public void OnStartState()
    {
        Debug.Log("successed using SkillState Action");
    }

    public void OnUpdateState()
    {

    }

     public void OnFixedUpdateState()
    {
        
        //Debug.Log("successed using Skill_Idle FixedUpdateState");
    }

    public void OnExitState()
    {

    }
}
