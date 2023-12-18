using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GruzMother : MonoBehaviour
{
    [Header("Idel")]
    [SerializeField] float idelMovementSpeed;
    [SerializeField] Vector2 idelMovementDirection;

    [Header("AttackUpNDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;

    [Header("Other")]
    [SerializeField] Transform goundCheckUp;
    [SerializeField] Transform goundCheckDown;
    [SerializeField] Transform goundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform playerCheck;
    private const float playerCheckRadius = 1.0f;
    [SerializeField] LayerMask playerLayer;
    private Animator anim;
    private bool invincible = false;
    public BossThemeSong bossThemeSong;

    


    private AudioSource audioSource;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;

    [SerializeField] private float health;
    [SerializeField] private float recoilLength;
    [SerializeField] private float recoilFactor;
    [SerializeField] private bool isRecoiling = false;

    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] protected GameObject PurpleBlood;

    [SerializeField] AudioClip slamSound;
    private SpriteRenderer sr;

    public Pintu pintu;
    public PintuKeluar pintuKeluar;

    public bool startAttack;
    private bool alreadyAttack = false;
    void Start()
    {
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        player = Player.Instance.transform;
        audioSource = GetComponent<AudioSource>();
        bossThemeSong = FindObjectOfType<BossThemeSong>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pintu = FindObjectOfType<Pintu>();
        pintuKeluar = FindObjectOfType<PintuKeluar>();

        pintu.Unactive();
        pintuKeluar.Active();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(goundCheckUp.position, groundCheckRadius, groundLayer); 
        isTouchingDown = Physics2D.OverlapCircle(goundCheckDown.position, groundCheckRadius, groundLayer); 
        isTouchingWall = Physics2D.OverlapCircle(goundCheckWall.position, groundCheckRadius, groundLayer);
        if (health <= 0)
        {
            CameraShake.instance.ShakeCamera(3f, 2f);
            pintu.Unactive();
            pintuKeluar.Unactive();
            bossThemeSong.PlayDefaultMusic();
            Death(3f);
        }

        startAttack = PlayerDetected();

       
        if(!alreadyAttack)
        {
            AttackStart();
        }

        FlashWhileInvincible();

        if(Player.Instance.health <= 0)
        {
            pintu.Unactive();
        }
        
    }

    void AttackStart()
    {
        if(startAttack)
        {
            alreadyAttack = true;
            enemyAnim.SetTrigger("GetAttack");
            bossThemeSong.PlayBossMusic();
            pintu.Active();
            
        }
    }

    bool PlayerDetected()
    {
        return Physics2D.OverlapCircle(playerCheck.position, playerCheckRadius, playerLayer);
    }
    void RandomStatePicker()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randomState == 1)
        {
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }

   public void IdelState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    } 
   public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            audioSource.PlayOneShot(slamSound);
            CameraShake.instance.ShakeCamera(2f, 0.3f);
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            audioSource.PlayOneShot(slamSound);
            CameraShake.instance.ShakeCamera(2f, 0.3f);
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
            playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }
        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }
        

        if (isTouchingWall || isTouchingDown)
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            audioSource.PlayOneShot(slamSound);
            CameraShake.instance.ShakeCamera(2f, 0.3f);
            enemyRB.velocity = Vector2.zero;
            hasPlayerPositon = false;
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(goundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckWall.position, groundCheckRadius);
    }

    public void EnemyGetsHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        /*anim.Play("BossGotHit");*/
        StartCoroutine(StopTakingDamage());

        if (!isRecoiling)
        {
            GameObject _purpleBlood = Instantiate(PurpleBlood, transform.position, Quaternion.identity);
            Destroy(_purpleBlood, 5.5f);
            enemyRB.velocity = _hitForce * recoilFactor * _hitDirection;
        }
        
    }

    IEnumerator KenaHit()
    {
        sr.enabled = false;
        yield return new WaitForSeconds(0.1f);
        sr.enabled = true;
    }

    IEnumerator StopTakingDamage()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }

    void FlashWhileInvincible()
    {
        sr.material.color = invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * 5, 1.0f)) : Color.white;
    }

    void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Player") && !Player.Instance.aState.invincible && !Player.Instance.aState.invincible && health > 0)
        {
            CameraShake.instance.ShakeCamera(1.2f, 0.2f);
            Attack();
            if (Player.Instance.aState.alive)
            {
                Player.Instance.HitStopTime(0, 5, 0.5f);
            }

        }
    }

    private void Attack()
    {
        Player.Instance.TakeDamage(damage);
    }

    private void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);
    }
}
