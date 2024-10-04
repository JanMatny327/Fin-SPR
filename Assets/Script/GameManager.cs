using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class GameData
{
    [Header("���� ����")]
    public int level = 1; // �÷��̾� ����
    public int[] levelUpExpBox = { 50, 55, 60, 65, 70, 75, 80, 85, 90, 95,
                                   105, 115, 125, 135, 145, 155, 165, 175, 195, 205,
                                   220, 235, 250, 265, 280, 295, 310, 325, 340, 355,
                                   375, 395, 415, 435, 455, 475, 495, 515, 535, 555,
                                   580, 605, 630, 655, 680, 705, 730, 755, 780, 805 }; // �÷��̾ �������� �ϱ� ���� ����ġ �ʿ䷮�� ���� �迭
    public int exp = 0; // �÷��̾� ����ġ
    public int statPoint = 0; // ���� ����Ʈ
    public int maxLevelValue = 50;

    [Header("Stats")]
    public int gold; // �÷��̾� ���
    public int maxHP = 100; // �÷��̾� �ִ� ü��
    public int hp; // �÷��̾� ���� ü��
    public int damage; // �÷��̾� ������
    public float criticalPercent; // ũ��Ƽ�� Ȯ��
    public float criticalDamage; // ũ��Ƽ�� ������
    public float playerSpeed; // �÷��̾� �̵��ӵ�
    public float jumpPower; // ������
    public bool isJump = false;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData gameData;
    public GameObject Panel;
    public GameObject player;

    [Header("���� �ؽ�Ʈ")]
    public TMP_Text gameBroadText;
    private bool isMessage = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        SettingUI();
        StartCoroutine(BroadCastManager());
    }

    [ContextMenu("Save Data")]
    void SaveData()
    {
        string data = JsonUtility.ToJson(gameData);
        string path = Path.Combine(Application.dataPath, "GameData.json");
        File.WriteAllText(path, data);
    }

    [ContextMenu("Load Data")]
    void LoadData()
    {
        SceneManager.LoadScene("GameScenes");

        string path = Path.Combine(Application.dataPath, "GameData.json");
        if (!File.Exists(path))
        {
            SaveData();
        }
        string data = File.ReadAllText(path);

        gameData = JsonUtility.FromJson<GameData>(data);
    }

    public void SettingUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerController.Instance.dieState)
        {
            if (!Panel.activeSelf)
            {
                Panel.SetActive(true);
                player.SetActive(false);
            }
            else
            {
                Panel.SetActive(false);
                player.SetActive(true);
            }
        }
    }

    public void LevelUpBroad()
    {
        gameBroadText.text = PlayerController.Instance.gameData.level + "������ �Ǿ����ϴ�!";
        gameBroadText.color = Color.green;
        isMessage = true;
    }

    private IEnumerator BroadCastManager()
    {
        while (isMessage)
        {
            yield return new WaitForSeconds(0.55f);
            gameBroadText.text = "";
            isMessage = false;
        }
    }
}

