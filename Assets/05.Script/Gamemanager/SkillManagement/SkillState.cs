using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SkillState
{
    public void OnStartState();

    public abstract void OnUpdateState();
    public abstract void OnFixedUpdateState();

    public abstract void OnExitState();
};
