using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public static EnemySystem Instance;

    [Header("Enemy Setting")]
    public int enemyHp;
    [SerializeField] int dropGold;
    [SerializeField] float speed;
    [SerializeField] bool isRun = false;
    [SerializeField] int dropExp;
    public bool enemyIsDie = false;

    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Enemy Attack Setting")]
    [SerializeField] private float attackRadius = 5f; // 공격 범위
    [SerializeField] private float attackCooldown; // 공격 쿨타임
    [SerializeField] private float cooldownResetValue; // 공격 쿨타임 초기화 밸류
    [SerializeField] private GameObject enemyBulletPrefab; // 총알 프리팹
    [SerializeField] private int numberOfBullet;
    public float spawnDistance = -2f;
    public LayerMask targetLayer;


    public GameObject hartPrefab;
    Transform enemyTransform;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public Transform player;
    public Vector2 home;

    public enum EnemyType { defaultEnemy, drone }

    private void Awake()
    {
        Instance = this;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTransform = this.transform;
        home = transform.position;
    }

    private void Start()
    {

    }
    private void Update()
    {
        EnemyAttack();

        if (PlayerController.Instance.isParring == true)
        {
            Invoke("Parring", 1f);
        }
        else
        {
            FollowPlayer();
            enemyWalkAnim();
        }
        EnemyStateCheck();


        if (this.attackCooldown > 0f)
            this.attackCooldown -= Time.deltaTime;
    }
    void EnemyStateCheck()
    {
        if (enemyHp <= 0 && !enemyIsDie)
         {
            animator.SetTrigger("enemyDeath");
            enemyIsDie = true;
            Instantiate(hartPrefab, gameObject.transform.position, gameObject.transform.rotation);
            Invoke("DestroyedEnemy", 0.37f);
            return;
        }
    }
    void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(player.position, enemyTransform.position);
        float distanceToHome = Vector2.Distance(home, enemyTransform.position);

        if (distanceToPlayer < 4f)
        {
            if (distanceToPlayer > 0.1f)
            {
                enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, player.position, Time.deltaTime * speed);
                isRun = true;
            }
        }
        else if (distanceToHome > 0.1f)
        {
            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, home, Time.deltaTime * speed);
            isRun = false;
        }
    }

    private void EnemyAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && attackCooldown <= 0)
            {
                animator.SetBool("isAttack", true);

                Shoot();

                attackCooldown = cooldownResetValue;

                StartCoroutine(ResetAttackAnimation());
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
    }
    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttack", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    public void getDamage(int damage)
    {
        this.enemyHp -= damage;
        return;
    }

    void enemyWalkAnim()
    {
        float distanceToHome = Vector2.Distance(home, enemyTransform.position);

        if (isRun) this.animator.SetBool("isWalk", true);
        else this.animator.SetBool("isWalk", false);

        if (player.position.x < enemyTransform.position.x) this.spriteRenderer.flipX = false;
        else this.spriteRenderer.flipX = true;
    }
    void Parring()
    {
        PlayerController.Instance.isParring = false;
        PlayerController.Instance.shootCoolTime = 0.3f;
    }

    void DestroyedEnemy()
    {
        PlayerController.Instance.gameData.gold += dropGold;
        PlayerController.Instance.gameData.exp += dropExp;
        Destroy(this.gameObject);
    }

    void BoomDrone()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyType == EnemyType.drone)
        {
            PlayerController.Instance.gameData.hp -= 20;
            animator.SetTrigger("Boom");
            Invoke("BoomDrone", 0.37f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ParringBox"))
        {
            PlayerController.Instance.isParring = true;
            PlayerController.Instance.shootCoolTime = 0.1f;
        }
    }
}