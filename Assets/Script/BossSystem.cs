using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class BossSystem : MonoBehaviour
{
    public static BossSystem Instance;
    // 기본 속성
    [SerializeField] private int bossHp; // 보스 HP
    private int maxBossHp; // 최대 보스 Hp
    private Rigidbody2D rig2D; // 보스 물리
    private Animator bossAnim; // 보스 애니메이션
    private BoxCollider2D boxCollider;
    [SerializeField] private int bossLevel = 1; // 보스 레벨
    [SerializeField] private Transform target;
    public int speedX; // rush 최대속도
    int DIRECTION;
    public bool isTurret; // 터렛이 생존해 있는가
    public int turretCount = 0; // 터렛이 몇개인가
    [SerializeField] private BossTurretSystem turretSystem;

    [Header("BossProfile")]
    public GameObject bossProfile; // 보스 체력바
    public Slider hpBar; //HP바
    public TextMeshProUGUI hpText; // HP바 잔량 표시



    [Header("Boss Bullet List")]
    [SerializeField] private GameObject[] bossBullet; // 일반 총알 Prefab 배열
    [SerializeField] private GameObject[] bossTurret; // 보스가 소환할 잡몹


    [Header("Boss Attack")]
    [SerializeField] Transform[] spinbulletPos; // 회전 보스 총알 날라갈 위치
    [SerializeField] Transform[] missilebulletPos; // 유도탄 보스 총알 날라갈 위치
    [SerializeField] Transform[] turretPos; // 터렛 소환될 위치
    public float attackCoolTime; // 공격 쿨타임
    private float attackCurTime; // 현재 공격 타임
    bool isAttack;
    public int bossDamage;

    [Header("BossPattern")]
    [SerializeField] private int patternLevel; // 패턴 레벨
    public float patternCoolTime; // 패턴 쿨타임
    public float patternWaitTime;
    public float roopPattern;
    public bool isPattern;
    bool isWall = false; // 돌진패턴에 쓰는 것

    [Header("PatternNote")]
    private static readonly int NONE = 0;
    private static readonly int SPINBULLET = 1;
    private static readonly int MISSILE = 2;
    private static readonly int RUSH = 3;

    private WaitForSeconds endOfPatternWait;
    private WaitForSeconds waitOfPatternWait;



    void Start()
    {
        Instance = this;
        rig2D = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        endOfPatternWait = new WaitForSeconds(patternCoolTime); // 패턴 대기시간 : 다음 패턴을 실행할때의 대기시간
        waitOfPatternWait = new WaitForSeconds(patternWaitTime); // 공격 대기시간 : 자세를 취하고 공격하는 대기시간
        maxBossHp = bossHp;
        StartCoroutine(RotateBullet());
        hpBar.value = (float)bossHp / maxBossHp;
    }
    void Update()
    {
        isBossRoom();
        BossLevel();
        HandleHp();
    }

    void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, (float)bossHp / maxBossHp, Time.deltaTime * 10);
        hpText.text = bossHp + "/" + maxBossHp;
    }
    public void getDamage()
    {
        bossHp -= PlayerController.Instance.gameData.damage;
        return;
    }
    void BossBullet()
    {
        if (attackCurTime <= 0)
        {
            switch (patternLevel)
            {
                case 1:
                    for (int i = 0; i < spinbulletPos.Length; i++)
                    {
                        Instantiate(bossBullet[0], spinbulletPos[i].position, spinbulletPos[i].rotation);
                    }
                    break;
                case 2:
                    for (int i = 0; i < missilebulletPos.Length; i++)
                    {
                        Instantiate(bossBullet[1], missilebulletPos[i].position, missilebulletPos[i].rotation);
                    }
                    break;
            }
            attackCurTime = attackCoolTime;
        }
        attackCurTime -= Time.deltaTime;
    }
    void Idle()
    {
        bossAnim.SetInteger("PatternLevel", 0);
    }
    void BossLevel()
    {
        if (bossHp <= maxBossHp / 2 && bossHp > maxBossHp / 3)
        {
            bossLevel = 2;
            bossAnim.SetBool("isCritical", true);
            Invoke("BossPattern", 2);
        }
        else if (bossHp <= maxBossHp / 3)
        {
            bossLevel = 3;
            bossAnim.SetBool("isCritical", true);
            Invoke("BossPattern", 2);
        }

    }
    void lookPlayer()
    {
        DIRECTION = (target.position.x < transform.position.x ? -1 : 1); //player와 자신(보스)의 x좌표를 비교해서 적당한 상수를 DIRECTION에 저장한다.
        float scale = transform.localScale.z;
        transform.localScale = new Vector3(DIRECTION * -1 * scale, scale, scale); //DIRECTION변수를 이용해서 player쪽을 바라보도록한다.
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
        else if (collision.gameObject.tag == "Wall")
        {
            isWall = true;
        }
    }

    IEnumerator roopOutPattern()
    {
        yield return new WaitForSeconds(roopPattern);
        isPattern = false;
    }

    void BossPattern()
    {
        if (bossLevel == 1)
        {
            patternLevel = Random.Range(1, 4);
            switch (patternLevel)
            {
                case 1:
                    StartCoroutine(RotateBullet());
                    break;
                case 2:
                    StartCoroutine(MissileBullet());
                    break;
                case 3:
                    StartCoroutine(Rush());
                    break;
            }
        }
        else if (bossLevel == 2)
        {
            patternLevel = Random.Range(1, 2);
            switch (patternLevel)
            {
                case 1:
                    StartCoroutine(Turret());
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if (bossLevel == 3)
        {

        }


    }

    IEnumerator RotateBullet() // 총알이 회전하면서 날라가는 패턴
    {
        patternLevel = 1;
        bossAnim.SetInteger("PatternLevel", 1);
        StartCoroutine(roopOutPattern());
        isPattern = true;
        while (isPattern)
        {
            yield return new WaitForSeconds(0.01f);
            foreach (Transform bullet1 in spinbulletPos)
            {
                bullet1.eulerAngles = new Vector3(0f, 0f, bullet1.eulerAngles.z + 1f);
                BossBullet();
            }
        }
        Idle();
        yield return endOfPatternWait;
        BossPattern();
    }
    IEnumerator MissileBullet()
    {
        bossAnim.SetInteger("PatternLevel", 2);
        StartCoroutine(roopOutPattern());
        isPattern = true;
        while (isPattern)
        {
            yield return new WaitForSeconds(0.01f);
            foreach (Transform bullet in missilebulletPos)
            {
                Vector3 direction = (target.position - bullet.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                BossBullet();
            }
        }
        Idle();
        yield return endOfPatternWait;
        BossPattern();
    }
    IEnumerator Rush()
    {
        bossAnim.SetInteger("PatternLevel", 3);
        lookPlayer();
        yield return new WaitForSeconds(1f);
        isWall = false;
        while (!isWall)
        {
            yield return new WaitForSeconds(0.1f);
            if (speedX > rig2D.velocity.x)
            {
                rig2D.AddForce(transform.right * DIRECTION * 1000);
            }
        }
        Idle();
        lookPlayer();
        yield return endOfPatternWait;
        BossPattern();
    }
    IEnumerator Turret()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform turret in turretPos)
        {
            if (isTurret == false) Instantiate(bossTurret[0], turret.position, turret.rotation);
            turretCount++;
            if (turretCount >= 3)
            {
                isTurret = true;
            }

        }
        yield return endOfPatternWait;
        BossPattern();

    }
    void isBossRoom()
    {
        if (PlayerController.Instance.isBossRoom == true)
        {
            bossProfile.SetActive(true);
        }
    }
}
