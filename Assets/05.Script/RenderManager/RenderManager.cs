using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    private LineRenderer line;
    private List<LineRenderer> lineList;
    private bool isInfoSend = false;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInfoSend)
        {
            Debug.Log("Succesed");
            StartCoroutine(this.ShowLineRender());
        }
    }

    public void SendLineRenderInfo(Vector3 startPoint , Vector3 endPoint) 
    {
        if(isInfoSend == false)
        {
            isInfoSend = !isInfoSend;
        }
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);
    }

    public void DeleteUsedLine()
    {

    }
    IEnumerator ShowLineRender()
    {
        line.enabled = true;
        yield return new WaitForSeconds(0.1f);        
        line.enabled = false;
        isInfoSend = false;
    }
}
