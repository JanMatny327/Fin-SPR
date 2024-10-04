using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletSystem : MonoBehaviour
{
    public static BulletSystem Instance;

    [Header("Bullet Type")]
    public float bulletSpeed; // 총알이 날아가는 속도
    public int bulletDamage; // 총알 공격력

    [SerializeField] Rigidbody2D rb; // 총알의 강체
    public WhoBulletType whobulletType; // 총알을 누가 쐈는지 정의
    public AbilityBulletType abilityBulletType; // 총알의 능력 정의
    public GameObject target;
    public Animator t_Animator;

    public enum WhoBulletType // 어떤 애가 쐈는가
    {
        playerBullet,
        enemyBullet,
        bossBullet
    }
    public enum AbilityBulletType // 총알의 능력
    {
        nomalBullet,
        missile,
        slow
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke("DestroyBullet", 3.7f);
        if (whobulletType == WhoBulletType.playerBullet)
            rb.velocity = transform.right * bulletSpeed;
        if (whobulletType == WhoBulletType.enemyBullet)
        {
            Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            rb.velocity = directionToPlayer * bulletSpeed;
        }
        if(whobulletType == WhoBulletType.bossBullet) rb.velocity = transform.right * bulletSpeed;
    }
    private void Update()
    {
        if (whobulletType == WhoBulletType.playerBullet)
        {
            this.bulletDamage = PlayerController.Instance.gameData.damage;
        }
    }


    void DestroyBullet()
    {
        Destroy(this.gameObject);
        return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (whobulletType == WhoBulletType.playerBullet && other.CompareTag("Enemy")) // 총알에 닿은 대상에 태그가 "Enemy"일 경우
        {
            other.GetComponent<EnemySystem>().getDamage(PlayerController.Instance.gameData.damage);
            DestroyBullet();
            return;
        }

        if (whobulletType == WhoBulletType.playerBullet && other.CompareTag("boss"))
        {
            other.GetComponent<BossSystem>().getDamage(PlayerController.Instance.gameData.damage);
            DestroyBullet();
            return;
        }

        if (whobulletType == WhoBulletType.enemyBullet && other.gameObject.tag == "Player")
        { PlayerController.Instance.takeDamage(bulletDamage); DestroyBullet(); return; }

        if (whobulletType == WhoBulletType.bossBullet && other.gameObject.tag == "Player")
        {
            PlayerController.Instance.takeDamage(this.bulletDamage);
            DestroyBullet();
            if (abilityBulletType == AbilityBulletType.missile)
            { PlayerController.Instance.takeDamage(bulletDamage); DestroyBullet(); return; }
        }

        if (other.CompareTag("Ground") || other.gameObject.tag == "Wall")
        {
            DestroyBullet();
            return;
        }
    }
}


