using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public float charPerSeconds;
    public GameObject EndCursor; 
    string targetMsg;
    TextMeshProUGUI msgText;
    int index;


    private void Awake()
    {
        msgText = GetComponent<TextMeshProUGUI>();
    }
    public void SetMsg(string msg)
    {
        targetMsg = msg;
        EffectStart();
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false); 
        Invoke("EffectIng", 1 / charPerSeconds);
    }
    void EffectIng()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();    
            return;
        }
        msgText.text += targetMsg[index];
        index++;

        Invoke("EffectIng", 1 / charPerSeconds);
    }
    void EffectEnd()
    {
        EndCursor.SetActive(true);
    }
}
