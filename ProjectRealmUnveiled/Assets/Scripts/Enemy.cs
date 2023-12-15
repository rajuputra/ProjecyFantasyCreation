using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected GameObject PurpleBlood;

    protected float recoilTimer;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;

    protected enum EnemyStates
    {
        //Crawler Enemy
        Crawler_Idle,
        Crawler_Flip,

        //Fly Enemy
        Fly_Idle,
        Fly_Chase,
        Fly_Stunned,
        Fly_Death,

        //Charger Enemy
        Charger_Idle,
        Charger_Surprissed,
        Charger_Charge,

        //Babi
        Babi_Idle,
        Babi_Tekejut,
        Babi_Charge,

        //Shade
        Shade_Idle,
        Shade_Chase,
        Shade_Stunned,
        Shade_Death,

    }

    protected EnemyStates currenEnemyState;
    
    protected virtual EnemyStates GetCurrentEnemyState
    {
        get { return currenEnemyState; }
        set
        {
            if(currenEnemyState != value)
            {
                currenEnemyState = value;
                ChangeCurrentAnimation();
            }
        }
    }

    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (pauseMenu.isPaused) return;

        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        else
        {
            UpdateEnemyState();
        }
    }

    public virtual void EnemyGetsHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            GameObject _purpleBlood = Instantiate(PurpleBlood, transform.position, Quaternion.identity);
            Destroy(_purpleBlood, 5.5f);
            rb.velocity = _hitForce * recoilFactor * _hitDirection;
        }
    }

    protected void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Player") && !Player.Instance.aState.invincible && !Player.Instance.aState.invincible && health > 0)
        {
            CameraShake.instance.ShakeCamera(1f, 0.2f);
            Attack();
            if (Player.Instance.aState.alive)
            {
                Player.Instance.HitStopTime(0, 5, 0.5f);
            }
            
        }
    }

    protected virtual void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);
    }

    protected virtual void UpdateEnemyState()
    {

    }

    protected virtual void ChangeCurrentAnimation() { }
    protected void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;
    }
    protected virtual void Attack()
    {
        Player.Instance.TakeDamage(damage);
    }
}
