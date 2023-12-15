using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;
    float timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Fly_Idle);
    }

    protected override void Update()
    {
        base.Update();
        
        if (!Player.Instance.aState.alive)
        {
            ChangeState(EnemyStates.Fly_Idle);
        }
    }

    protected override void UpdateEnemyState()
    {
        float _dist = Vector2.Distance(transform.position, Player.Instance.transform.position);
        switch(GetCurrentEnemyState)
        {
            case EnemyStates.Fly_Idle:
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Fly_Chase);
                }
                break;

            case EnemyStates.Fly_Chase:
                rb.MovePosition(Vector2.MoveTowards(transform.position, Player.Instance.transform.position, Time.deltaTime * speed));

                FlipFly();
                break;

            case EnemyStates.Fly_Stunned:
                timer += Time.deltaTime;

                if(timer > stunDuration)
                {
                    ChangeState(EnemyStates.Fly_Idle);
                    timer = 0;
                }

                break;


            case EnemyStates.Fly_Death:
                Death(Random.Range(5, 10));

                break;
        }
    }

    public override void EnemyGetsHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyGetsHit(_damageDone, _hitDirection, _hitForce);

        if(health > 0)
        {
            ChangeState(EnemyStates.Fly_Stunned);
        }
        else
        {
            ChangeState(EnemyStates.Fly_Death);
        }
    }

    protected override void Death(float _destroyTime)
    {
        rb.gravityScale = 12;
        base.Death(_destroyTime);
    }
    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Fly_Idle);

        anim.SetBool("Chase", GetCurrentEnemyState == EnemyStates.Fly_Chase);

        anim.SetBool("Stunned", GetCurrentEnemyState == EnemyStates.Fly_Stunned);

        if(GetCurrentEnemyState == EnemyStates.Fly_Death)
        {
            anim.SetTrigger("Death");
        }
    }

    void FlipFly()
    {
        sr.flipX = Player.Instance.transform.position.x > transform.position.x;
    }
}
