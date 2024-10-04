using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BGM 사운드 관리")]
    public AudioClip[] BgmClips;
    public AudioSource BgmPlayer;

    [Header("SFX 사운드 관리")]
    public AudioClip[] SfxClips;
    public AudioSource SfxPlayer;

    private void Awake()
    {
        if (Instance != null)
        { Destroy(this.gameObject); }
        else { DontDestroyOnLoad(this.gameObject); Instance = this; }
    }

    public void BgmPlay(string type)
    {
        switch (type)
        {
            case "첫 로딩 화면 사운드":
                BgmPlayer.clip = BgmClips[0];
                BgmPlayer.Play(); break;
            case "타이틀 사운드":
                BgmPlayer.clip = BgmClips[1];
                BgmPlayer.Play(); break;
            case "맵1 배경음":
                BgmPlayer.clip = BgmClips[2];
                BgmPlayer.Play(); break;
            case "보스방 배경음":
                BgmPlayer.clip = BgmClips[3];
                BgmPlayer.Play(); break;
            default:
                Debug.LogError("찾을 수 없는 배경음"); break;
        }
    }

    public void SfxPlay(string type)
    {
        switch (type)
        {
            case "플레이어 총 사운드":
                SfxPlayer.PlayOneShot(SfxClips[0]); break;
            case "발걸음 소리":
                SfxPlayer.clip = SfxClips[1];
                SfxPlayer.Play(); break;
            default:
                Debug.LogError("찾을 수 없는 효과음"); break;
        }
    }
}

