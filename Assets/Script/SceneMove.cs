using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void StartScenes()
    {
        SceneManager.LoadScene("GameScenes");
        SoundManager.Instance.BgmPlay("∏ 1 πË∞Ê¿Ω");
        return;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}