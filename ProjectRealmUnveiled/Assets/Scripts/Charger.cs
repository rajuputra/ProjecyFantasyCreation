using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{

    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerDetectionRadius; // Adjust this based on your needs
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] Transform checkPlayer;
    float timer;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Charger_Idle);
        rb.gravityScale = 12f;
    }

    protected override void Update()
    {
        base.Update();
        base.Update();
        if (!Player.Instance.aState.alive)
        {
            ChangeState(EnemyStates.Charger_Idle);
        }
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPlayer.position, playerDetectionRadius);
    }
    protected override void UpdateEnemyState()
    {
        if (health <= 0)
        {
            
            Death(0.05f);
        }

        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Charger_Idle:
                MoveCharger();

                RaycastHit2D groundCheck = Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround);
                RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround);

                Debug.DrawRay(transform.position + _ledgeCheckStart, Vector2.down * ledgeCheckY, Color.green);
                Debug.DrawRay(transform.position, _wallCheckDir * ledgeCheckX, Color.blue);
                Debug.DrawRay(transform.position + _ledgeCheckStart, _wallCheckDir * ledgeCheckX * 4, Color.red);

                if (!groundCheck || wallCheck)
                {
                    FlipCharger();
                }

                Collider2D playerCollider = Physics2D.OverlapCircle(checkPlayer.position, playerDetectionRadius, LayerMask.GetMask("Player"));
                if (playerCollider != null)
                {
                    ChangeState(EnemyStates.Charger_Surprissed);
                }

                break;

            case EnemyStates.Charger_Surprissed:
                rb.velocity = new Vector2(0, jumpForce);
                ChangeState(EnemyStates.Charger_Charge);
                break;

            case EnemyStates.Charger_Charge:
                timer += Time.deltaTime;

                if (timer < chargeDuration)
                {
                    ChargeTowardsPlayer();
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                }
                break;
        }
    }

    public override void EnemyGetsHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyGetsHit(_damageDone, _hitDirection, _hitForce);

        if (health > 0)
        {
            anim.SetTrigger("GetHit");
        }
        else
        {
            anim.SetTrigger("Death");
        }
    }
    /*protected override void UpdateEnemyState()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            Death(0.05f);

        }

        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Charger_Idle:
                // Move Charger freely in the idle state
                MoveCharger();

                RaycastHit2D groundCheck = Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround);
                RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround);

                Debug.DrawRay(transform.position + _ledgeCheckStart, Vector2.down * ledgeCheckY, Color.green);
                Debug.DrawRay(transform.position, _wallCheckDir * ledgeCheckX, Color.blue);
                Debug.DrawRay(transform.position + _ledgeCheckStart, _wallCheckDir * ledgeCheckX * 4, Color.red);


                if (!groundCheck || wallCheck)
                {
                    FlipCharger();
                }

                RaycastHit2D playerCheck = Physics2D.Raycast(transform.position + _ledgeCheckStart, _wallCheckDir, ledgeCheckX * 4);
                if (playerCheck.collider != null && playerCheck.collider.gameObject.CompareTag("Player"))
                {
                    ChangeState(EnemyStates.Charger_Surprissed);
                }

                break;

            case EnemyStates.Charger_Surprissed:
                rb.velocity = new Vector2(0, jumpForce);
                ChangeState(EnemyStates.Charger_Charge);
                break;

            case EnemyStates.Charger_Charge:
                timer += Time.deltaTime;

                if (timer < chargeDuration)
                {
                    ChargeTowardsPlayer();
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                }
                break;
        }
    }*/



    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Charge", currenEnemyState == EnemyStates.Charger_Charge);
    }

    private void MoveCharger()
    {
        // Implement Charger's movement logic here
        float moveDirection = transform.localScale.x; // 1 for right, -1 for left
        rb.velocity = new Vector2(speed * moveDirection, rb.velocity.y);
    }

    private void FlipCharger()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private void ChargeTowardsPlayer()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, whatIsGround))
        {
            float chargeDirection = transform.localScale.x; // 1 for right, -1 for left
            rb.velocity = new Vector2(speed * chargeSpeedMultiplier * chargeDirection, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


}
