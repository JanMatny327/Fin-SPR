using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLoaded : MonoBehaviour
{
    [Header("컴포넌트")]
    public TMP_Text BroadText;
    public TMP_Text BroadText2;
    public SpriteRenderer Logo;

    [Header("타이틀 세부 설정")]
    public Color StartColor;
    public Color StartColor2;
    bool Message = false;
    bool Message2 = false;
    public bool First = true;
    public bool First2 = false;
    public bool First3 = false;
    public bool Loading = false;

    private void Awake()
    {

    }
    private void Update()
    {
        StartCoroutine(MessageSay());
        StartCoroutine(MessageFade());
        BroadText.color = StartColor;
        BroadText2.color = StartColor2;

        if (Loading)
        {
            SceneManager.LoadScene("TitleScenes");
            SoundManager.Instance.BgmPlay("타이틀 사운드");
        }
    }

    IEnumerator MessageSay()
    {
        while (First)
        {
            StartColor.a += 0.5f * Time.deltaTime;
            BroadText.text = "본 게임은 성남시 청소년 게임개발대회를 위해 제작되었습니다.";
            Message = true;
            yield return new WaitForSeconds(3.5f);
            First2 = true;
            First = false;
        }

        while (First2)
        {
            StartColor.a += 0.5f * Time.deltaTime;
            BroadText.text = "Survival Project R";
            SoundManager.Instance.BgmPlay("첫 로딩 화면 사운드");
            Message = true;
            yield return new WaitForSeconds(5.5f);
            First3 = true;
            First2 = false;
        }

        while (First3)
        {
            StartColor2.a += 0.5f * Time.deltaTime;
            BroadText2.text = "제작 : 일개미들";
            Logo.enabled = true;
            yield return new WaitForSeconds(8f);
            Logo.enabled = false;
            First3 = false;
            Loading = true;
        }
    }

    IEnumerator MessageFade()
    {
        while (Message)
        {
            yield return new WaitForSeconds(1f);
            StartColor.a -= 0.5f * Time.deltaTime;

            if (StartColor.a <= 0f)
                Message = false;
        }
    }
}
