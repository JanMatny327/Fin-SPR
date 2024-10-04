using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{  
    public NPCSystem npcSystem;
    public Animator talkPanel;
    public TypeEffect talkText;
    public GameObject nameText;
    public GameObject profile;
    public GameObject scanObject;
    public Image profileImg;
    public Sprite prevProfile; // 과거 스프라이트 저장
    public Animator profileAnim;
    public bool isAction = false;
    public int talkIndex;


    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        talkPanel.SetBool("isShow", isAction);
        profile.SetActive(!isAction);
    }
    void Talk(int id, bool isNpc)
    {
        string talkData = npcSystem.GetTalk(id, talkIndex);

        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }
        
        if (isNpc)
        {
            talkText.SetMsg(talkData.Split(':')[0]);
            
            // 스프라이트 프로필
            profileImg.sprite = npcSystem.GetProfile( id, int.Parse(talkData.Split(':')[1]));
            this.nameText.GetComponent<TextMeshProUGUI>().text = scanObject.name;
            profileImg.color = new Color(1, 1, 1, 1);

            // 스프라이트 애니메이션
            profileAnim.SetTrigger("isEffect");
            prevProfile = profileImg.sprite;
        }
        else
        {
            talkText.SetMsg(talkData);
            profileImg. color = new Color(1, 1, 1, 0);
        }
        isAction = true;
        talkIndex++;
        
    }
}
