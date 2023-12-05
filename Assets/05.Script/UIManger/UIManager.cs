using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        Debug.Log("button Clicked");
        SceneManager.LoadScene("scLevel1");
    }
}
