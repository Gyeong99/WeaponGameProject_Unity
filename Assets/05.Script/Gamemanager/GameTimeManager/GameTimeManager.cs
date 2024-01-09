using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    private Character _character;
    public float slowFactor = 0.05f;
    public float slowLength = 1f;

    private void Start()
    {
        _character = FindObjectOfType<Character>();
    }
    private void DoSlowMotion()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void Update()
    {
        if (_character.CurrentState == Character.eCharacterStates.BASH)
        {
            DoSlowMotion();
        }
        Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
