using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    /*[SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    float timer;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Charger_Idle);
        rb.gravityScale = 12f;
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

                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround)
                    || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }

                RaycastHit2D _hit = Physics2D.Raycast(transform.position + _ledgeCheckStart, _wallCheckDir, ledgeCheckX * 10);
                if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Player"))
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

                if(timer < chargeDuration)
                {
                    if(Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, whatIsGround))
                    {
                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(speed * chargeSpeedMultiplier, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed * chargeSpeedMultiplier, rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                }


                break;
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        if(GetCurrentEnemyState == EnemyStates.Charger_Idle)
        {
            anim.speed = 1;
        }

        if (GetCurrentEnemyState == EnemyStates.Charger_Charge)
        {
            anim.speed = chargeSpeedMultiplier;
        }
    }*/

    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    float timer;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Charger_Idle);
        rb.gravityScale = 12f;
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
                // Move Charger freely in the idle state
                MoveCharger();

                RaycastHit2D groundCheck = Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround);
                RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround);

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
    }

    protected override void ChangeCurrentAnimation()
    {
        if (GetCurrentEnemyState == EnemyStates.Charger_Idle)
        {
            anim.speed = 1;
        }

        if (GetCurrentEnemyState == EnemyStates.Charger_Charge)
        {
            anim.speed = chargeSpeedMultiplier;
        }
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
