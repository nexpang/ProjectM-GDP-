using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONArcher : CONEntity
{
    [SerializeField]
    private float shootTimerMax;

    private float shootTimer;

    private CONEnemy targetEnemy;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.1f;

    private Vector3 arrowSpawnPos;
    private Animator anim;


    public override void Awake()
    {
        base.Awake();
        arrowSpawnPos = transform.Find("arrowSpawnPos").position;
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        HandleTargetting();

        HandleShooting();
    }

    private void HandleTargetting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer += shootTimerMax;
            if (targetEnemy != null)
            {
                anim.SetTrigger("doAttack");
                CONArrow.Create(arrowSpawnPos, targetEnemy);
            }
            else
            {
                anim.SetTrigger("doStop");
            }
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 20;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            CONEnemy enemy = collider.GetComponent<CONEnemy>();
            if (enemy != null && enemy.IsActive())
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
