using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONEnemy : CONEntity
{
    public static CONEnemy Create(Vector3 pos)
    {
        Transform enemyTrm = GameSceneClass.gMGPool.CreateObj(ePrefabs.Enemy, pos).transform;

        return enemyTrm.GetComponent<CONEnemy>();
    }

    private Rigidbody2D rigidbody2D;
    private Transform targetTrm; //커맨드센터

    private HealthSystem healthSystem;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        //targetTrm = 

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += CallOnDied;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void CallOnDied()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();

        HandleTargetting();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void HandleMovement()
    {
        if (targetTrm != null)
        {
            Vector3 moveDir = (targetTrm.position - transform.position).normalized;

            float moveSpeed = 7f;
            rigidbody2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
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

    private void LookForTargets()
    {
        //float targetMaxRadius = 10;

        //Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        //foreach (Collider2D collider in collider2DArray)
        //{
        //    Building building = collider.GetComponent<Building>();
        //    if (building != null)
        //    {
        //        if (targetTrm == null)
        //        {
        //            targetTrm = building.transform;
        //        }
        //        else
        //        {
        //            if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTrm.position))
        //            {
        //                targetTrm = building.transform;
        //            }
        //        }
        //    }
        //}

        //if (targetTrm == null)
        //{
        //    targetTrm = BuilderManager.Instance.GetCCBuilding().transform;
        //}
    }
}
