using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSystem : MonoBehaviour
{
    // NPC Number은 1000부터 시작
    // 물건과의 상호작용은 100부터 시작
    public Dictionary<int, string[]> talkData;
    public Dictionary<int, Sprite> profileData;

    public  Sprite[] profileArr;

    private void Awake()
    {
       talkData = new Dictionary<int, string[]>();
        profileData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    private void GenerateData()
    {
        talkData.Add(1000, new string[] { "안녕 나는 먼지나는것을 싫어해:0", "점프를 뛰지말거라...:0" });
        talkData.Add(2000, new string[] { "달이 아름다운 밤이군..:1", "그대의 눈동자에 건배~:1", "그래서 말인데:1","나랑 같이 데이트 가지 않을래?:1" });

        profileData.Add(1000, profileArr[0]);
        profileData.Add(2000 + 1, profileArr[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if(talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }
    public Sprite GetProfile(int id, int profileIndex)
    {
        return profileData[id + profileIndex];
    }
}
