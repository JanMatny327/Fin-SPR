using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Stat : MonoBehaviour
{
    [Header("스텟별 증가량 관리")]
    [SerializeField] public int hpPlus;
    [SerializeField] public int damagePlus;
    [SerializeField] public float speedPlus;
    

    [Header("스텟으로 증가된 총 능력치")] // 스텟으로 인해 증가한 수치를 변수에 할당
    [SerializeField] private int hpPlusValue = 0;
    [SerializeField] private int damagePlusValue = 0;
    [SerializeField] private float speedPlusValue = 0;

    [Header("텍스트 관리")]
    [SerializeField] TMP_Text BroadCast;
    [SerializeField] TMP_Text[] plusValueTexts;
    [SerializeField] TMP_Text playerInfo;
    [SerializeField] TMP_Text goldText;
    private bool isMessage = false;

    private void Update()
    {
        StartCoroutine(BroadCastManager());
        PlayerInfoUpdateUI();
    }

    public void HpStatPlus()
    {
        if (PlayerController.Instance.gameData.statPoint > 0)
        {
            PlayerController.Instance.gameData.maxHP += hpPlus;
            PlayerController.Instance.gameData.hp += hpPlus;
            hpPlusValue += hpPlus; 
            PlayerController.Instance.gameData.statPoint -= 1;
            plusValueTexts[0].text = "체력 증가량 : " + hpPlusValue;
            Debug.Log("HP : " + PlayerController.Instance.gameData.hp);
        }
        else { Debug.LogError("스텟 포인트가 부족합니다. "); BroadCast.text = "스텟 포인트가 부족합니다."; BroadCast.color = Color.red; isMessage = true; }
    }

    public void DamageStatPlus()
    {
        if (PlayerController.Instance.gameData.statPoint > 0)
        {
            PlayerController.Instance.gameData.damage += damagePlus;
            damagePlusValue += damagePlus;
            PlayerController.Instance.gameData.statPoint -= 1;
            plusValueTexts[1].text = "공격력 증가량 : " + damagePlusValue;
        }
        else { Debug.LogError("스텟 포인트가 부족합니다. "); BroadCast.text = "스텟 포인트가 부족합니다."; BroadCast.color = Color.red; isMessage = true; }
    }

    public void SpeedStatPlus()
    {
        if (PlayerController.Instance.gameData.statPoint > 0)
        {
            PlayerController.Instance.gameData.playerSpeed += speedPlus;
            speedPlusValue += speedPlus;
            PlayerController.Instance.gameData.statPoint -= 1;
            plusValueTexts[2].text = "이속 증가량 : " + speedPlusValue;
        }
        else { Debug.LogError("스텟 포인트가 부족합니다. "); BroadCast.text = "스텟 포인트가 부족합니다."; BroadCast.color = Color.red; isMessage = true; }
    } 

    private IEnumerator BroadCastManager()
    {
        while (isMessage)
        {
            yield return new WaitForSeconds(0.55f);
            BroadCast.text = "";
            isMessage = false;
        }
    }

    private void PlayerInfoUpdateUI()
    {
        playerInfo.text = "공격력 : " + PlayerController.Instance.gameData.damage +
        "\n" +
        "\n최대 체력 : " + PlayerController.Instance.gameData.maxHP +
        "\n" +
        "\n현재 체력 : " + PlayerController.Instance.gameData.hp +
        "\n" +
        "\n이동속도 : " + PlayerController.Instance.gameData.playerSpeed +
        "\n" +
        "\n" +
        "\n" +
        "\n스텟 포인트 : " + PlayerController.Instance.gameData.statPoint;

        goldText.text = "" + PlayerController.Instance.gameData.gold;
    }
}
