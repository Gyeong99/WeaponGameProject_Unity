using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private SkillState skillState;

    public Skill(SkillState skillState)
    {
        this.skillState = skillState;
    }
    
    public void SetSkillState(SkillState skillState)
    {
        this.skillState.OnExitState();
        this.skillState = skillState;
        this.skillState.OnStartState();
    }

    public SkillState GetSkillState(Skill skill)
    {
        return this.skillState;
    }

    public void OnStartState()
    {
        skillState.OnStartState();
    }

    public void OnUpdateState()
    {
        skillState.OnUpdateState();
    }

    public void OnFixedUpdateState()
    {
        skillState.OnFixedUpdateState();
    }

    public void OnExitState()
    {
        skillState.OnExitState();
    }
}
