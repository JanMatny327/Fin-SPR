using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("GameData")]
    public GameData gameData; // GameData 받아옴

    [Header("Player")]
    public Animator animator;
    public Slider hpBar; //HP바
    GameObject hpText; // HP바 잔량 표시
    public Rigidbody2D rig2D; // 플레이어 리지디바디
    public BoxCollider2D box2D; // 플레이어 박스 콜라이더
    public SpriteRenderer skin; // 플레이어 스프라이트
    public string nowStage; // 현재 스테이지 체크
    public LayerMask groundLayer; // 감지할 레이어
    public Transform groundCheck;
    public GameObject playerdown;
    public bool playerCheck; //플레이어 활성화 비활성화 체크

    [Header("PlayerDash")]
    public GameObject coolDownText; // 대쉬 쿨타임 표시
    float coolDown = 0f; // 대쉬 발동 딜레이
    float dashTime = 0.05f; // 대쉬 발동 시간
    float dashSpeed = 100; // 대쉬 속도
    float maxDashSpeed;
    float currentTime; // 대쉬 현재 발동 시간
    public Ghost ghost; // 대쉬 분신 클래스

    [Header("PlayerJump")]
    public bool isGround; // 땅 체크
    public int jumpCount = 2; // 이단 점프 체크

    [Header("PlayerMove")]
    private bool m_FacingRight = true; // 플레이어 좌우반전 체크
    float move; // 플레이어 좌우반전 체크

    [Header("PlayerBullet")]
    public GameObject bullet; // 총알 프리팹
    public Transform idlePos; // 가만히 있을때 발사 위치
    public Transform runPos; // 움직일때 발사 위치
    public float maxShootCoolTime; // 총알 발사 딜레이 저장 공간
    public float shootCoolTime; // 총알 발사 딜레이
    private float shootCurTime; // 현재 총알 발사 시간

    [Header("PlayerSkill")]
    public GameObject skillCoolTimeText;
    public bool isParring;
    private bool isPariringMove;
    public float skillCoolTime;
    private float skillCurTime;


    [Header("TalkSystem")]
    public TalkManager talkManager; // 대화창 클래스
    GameObject scanObject; // 내가 보고 있는 NPC 또는 사물
    Vector2 dirVec; // 내가 보고 있는 방향



    [Header("PlayerDie")]
    public Image DieImg; // 죽을 때 이미지
    public Color dieImgColor; // 죽을 때 표시 컬러
    public Animator diePanel; // 죽을 때 등장하는 패널
    public TMP_Text dieMessage; // 죽을 때 등장하는 메세지
    string dieText = "You Die..."; // 띄우는 메세지
    public GameObject ProFile; // ProFile을 가져옴
    public GameObject replayBtn; // 다시 시작 버튼
    public bool dieState; // 죽었는지 상태 체크

    [Header("isBossRoom?")]
    public bool isBossRoom;
    public GameObject boss;
    public BoxCollider2D bossRoomWall;
    public GameObject bossCutScene;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        hpBar.value = (float)gameData.hp / gameData.maxHP;
        this.hpText = GameObject.Find("HpText");
        gameData.playerSpeed = 8.0f;
        gameData.hp = 100;
        gameData.jumpPower = 35;
        maxShootCoolTime = shootCoolTime;
        maxDashSpeed = dashSpeed;
    }

    void Update()
    {
        ExpCheck();
        //Debug.Log(gameData.exp + "/ " + gameData.levelUpExpBox[gameData.level]);
        if (Input.GetKeyDown(KeyCode.LeftShift)) gameData.exp += 20;

        // 플레이어가 움직일 때 나는 효과음
        //if (move != 0) SoundManager.Instance.SfxPlay("발걸음 소리");

        DieImg.color = dieImgColor;
        StartCoroutine(playerStateCheck());

        HandleHp();

        if (coolDown > 1)
        {
            coolDown -= Time.deltaTime;
            this.coolDownText.GetComponent<TextMeshProUGUI>().text = "" + (int)coolDown + "";
        }
        else
        {
            coolDown = 1;
            coolDownText.SetActive(false);
        }

        // DashCoolTimeCheak
        if (currentTime > 0)
        {
            PlayerDash();
            currentTime -= Time.deltaTime;
        }
        else
        {
            playerMove();
            ghost.makeGhost = false;
        }

        if (coolDown == 1 && Input.GetKeyDown(KeyCode.L))
        {
            currentTime = dashTime;
            coolDown = 5.1f;
        }

        PlayerJump();
        playerTalk();
        rayObject();
        PlayerShoot();
        PlayerDrop();

        //스프라이트 좌우 반전
        if (move > 0 && !m_FacingRight)
        {
            Filp();
        }
        else if (move < 0 && m_FacingRight)
        {
            Filp();
        }

        // K Skill(Parring)
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (skillCurTime <= 1)
            {
                StartCoroutine(FastShot());
                skillCurTime = skillCoolTime;
            }
        }
        skillCurTime -= Time.deltaTime;
        if (skillCurTime > 1)
        {
            this.skillCoolTimeText.GetComponent<TextMeshProUGUI>().text = " " + (int)skillCurTime + " ";
        }
        else
        {
            skillCoolTimeText.SetActive(false);
        }

        if (skillCurTime <= 7)
        {
            this.animator.SetBool("isParring", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            Trap.Instance.TrapHit(1.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HealingItem")
        {
            Destroy(collision.gameObject);
            gameData.hp += 10;
        }
    }

    bool IsGround() // 땅 체크
    {
        int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Platform"));
        var ray = Physics2D.Raycast(groundCheck.position, Vector2.right, 1f, layerMask);
        return ray.collider != null;
    }

    IEnumerator playerStateCheck()
    {
        if (gameData.hp <= 0)
        {
            this.gameData.playerSpeed = 0f;
            this.gameData.jumpPower = 0;
            this.dieState = true;
            diePanel.SetTrigger("PanelOpen");
            ProFile.SetActive(false);
            dieImgColor.a = Mathf.MoveTowards(dieImgColor.a, 150f, 15f * Time.deltaTime);
            gameData.hp = 100;
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(TextFade(dieText));
            yield return new WaitForSeconds(2f);
            replayBtn.SetActive(true);
        }
    }

    IEnumerator TextFade(string dieText)
    {
        foreach (char letter in dieText.ToCharArray())
        {
            dieMessage.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void PlayerDash()
    {
        if (this.talkManager.isAction || this.dieState)
        {
            return;
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                coolDownText.SetActive(true);
                this.animator.SetInteger("PlayerRun", +1);
                transform.Translate(Vector2.left * dashSpeed * Time.deltaTime, Space.World);
                ghost.makeGhost = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                this.animator.SetInteger("PlayerRun", +1);
                transform.Translate(Vector2.right * dashSpeed * Time.deltaTime, Space.World);
                coolDownText.SetActive(true);
                ghost.makeGhost = true;

            }
            else
            {
                this.animator.SetInteger("PlayerRun", 0);
            }
        }
    }
    void playerMove()
    {
        move = 0;
        if (this.talkManager.isAction || this.dieState || isPariringMove)
        {
            return;
        }
        else
        {
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");
            if (Input.GetKeyUp(KeyCode.J))
            {
                this.animator.SetInteger("RunShoot", 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.J))
                {
                    this.animator.SetInteger("RunShoot", 1);
                }
                this.animator.SetInteger("PlayerRun", 1);
                transform.Translate(Vector2.left * gameData.playerSpeed * Time.smoothDeltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.J))
                {
                    this.animator.SetInteger("RunShoot", 1);
                }
                this.animator.SetInteger("PlayerRun", 1);
                transform.Translate(Vector2.right * gameData.playerSpeed * Time.smoothDeltaTime, Space.World);
            }
            else
            {
                this.animator.SetInteger("PlayerRun", 0);
            }
        }
    }

    public void takeDamage(int damage)
    {
        if (isParring)
            return;
        else if (BulletSystem.Instance.whobulletType == BulletSystem.WhoBulletType.enemyBullet)
        {
            gameData.hp -= damage;
            if (BulletSystem.Instance.abilityBulletType == BulletSystem.AbilityBulletType.slow)
            { gameData.playerSpeed = 4f; Invoke("UnSlow", 1.5f); }
        }
        else if (BulletSystem.Instance.whobulletType == BulletSystem.WhoBulletType.bossBullet)
        {
            gameData.hp -= GetComponent<BossSystem>().bossDamage;
            if (BulletSystem.Instance.abilityBulletType == BulletSystem.AbilityBulletType.slow)
            { gameData.playerSpeed /= 4f; Invoke("UnSlow", 1.5f); }
        }
    }

    void UnSlow()
    {
        this.gameData.playerSpeed = 8f;
    }

    void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, (float)gameData.hp / (float)gameData.maxHP, Time.deltaTime * 10);
        this.hpText.GetComponent<TextMeshProUGUI>().text = gameData.hp + " / " + gameData.maxHP;
    }

    void PlayerJump() // 플레이어 점프
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.animator.SetBool("isJumping", true);
            if (IsGround() || jumpCount > 1)
            {
                rig2D.velocity = new Vector2(rig2D.velocity.x, 0);
                rig2D.AddForce(Vector2.up * gameData.jumpPower, ForceMode2D.Impulse);
                jumpCount--;
            }
        }
        if (IsGround())
        {
            jumpCount = 2;
            this.animator.SetBool("isJumping", false);
        }

    }
    void playerTalk()
    {
        if (Input.GetKeyDown(KeyCode.F) && scanObject != null)
        {
            talkManager.Action(scanObject);
        }
    }
    void rayObject()
    {
        if (talkManager.isAction == true)
        {
            return;
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                dirVec = Vector2.right;
                move = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dirVec = Vector2.left;
                move = -1;
            }
            Debug.DrawRay(rig2D.position, dirVec * 0.7f, new Color(0, 1, 0));
            Debug.DrawRay(groundCheck.position, Vector2.right * 1f, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rig2D.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

            if (rayHit.collider != null)
            {
                scanObject = rayHit.collider.gameObject;
            }
            else
            {
                scanObject = null;
            }
        }
    }
    public void PlayerShoot()
    {
        if (this.talkManager.isAction || this.dieState || isPariringMove)
        {
            return;
        }
        else
        {
            if (Input.GetKey(KeyCode.J))
            {
                if (shootCurTime <= 0)
                {
                    if (move != 0)
                    {
                        this.animator.SetBool("isShoot", false);
                        this.animator.SetInteger("RunShoot", 1);
                        Instantiate(bullet, runPos.position, transform.rotation);
                        //SoundManager.Instance.SfxPlay("플레이어 총 사운드");
                    }
                    else
                    {
                        this.animator.SetBool("isShoot", true);
                        this.animator.SetInteger("RunShoot", 0);
                        Instantiate(bullet, idlePos.position, transform.rotation);
                        //SoundManager.Instance.SfxPlay("플레이어 총 사운드");
                    }
                    shootCurTime = shootCoolTime;
                }
            }
            else
            {
                this.animator.SetBool("isShoot", false);
            }
            shootCurTime -= Time.deltaTime;
        }
    }

    void PlayerDrop()
    {
        if (gameObject.transform.position.y < -13)
        {
            gameObject.transform.position = new Vector3(83, -4, 0);
            gameData.hp -= 10;
        }
    }
    public void Replay()
    {
        dieImgColor = Color.black;
        replayBtn.SetActive(false);
        Invoke("ReplaySetting", 2.5f);
    }

    public void ReplaySetting()
    {
        SceneManager.LoadScene("GameScenes");
        this.gameData.playerSpeed = 8.0f;
        this.gameData.jumpPower = 1800;
        this.gameData.hp = 100;
        this.dieState = false;
        dieImgColor.a = Mathf.MoveTowards(dieImgColor.a, 0f, -1f * Time.deltaTime);
    }
    void Filp()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, -180f, 0f);
    }
    IEnumerator FastShot()
    {
        skillCoolTimeText.SetActive(true);
        this.animator.SetBool("isParring", true);
        shootCoolTime = 0.1f;
        yield return new WaitForSeconds(3f);
        shootCoolTime = maxShootCoolTime;
    }

    private void ExpCheck()
    {
        if (gameData.level != gameData.maxLevelValue)
        {
            if (gameData.exp >= gameData.levelUpExpBox[gameData.level])
            {
                LevelUp();
                Debug.Log(gameData.level);
            }
        }
    }

    private void LevelUp()
    {
        gameData.level += 1;
        gameData.statPoint += 1;
        gameData.exp = 0;
        GameManager.instance.LevelUpBroad();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBossRoom == true)
        {
            return;
        }
        else if (collision.gameObject.name == "BossGate-1")
        {
            SoundManager.Instance.BgmPlay("보스방 배경음");
        }
        if (collision.gameObject.tag == "BossRoomGateWall")
        {
            StartCoroutine(BossCutScene());
            isBossRoom = true;
            bossRoomWall.enabled = true;
        }
    }

    IEnumerator BossCutScene() // 보스 컷씬을 위해 필요
    {
        bossCutScene.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        Destroy(bossCutScene);
        boss.SetActive(true);
    }
}