using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    private string characterName;
    public virtual void SetUp(string name)
    {
        characterName = name;
    }

    public abstract void Updated();

    public abstract void FixedUpdated();

    public void PrintText(string text)
    {
        Debug.Log($"{characterName} : {text}");
    }
    
}
