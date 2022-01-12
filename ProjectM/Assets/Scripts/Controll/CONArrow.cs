using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONArrow : CONEntity
{
    private CONEnemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    public static CONArrow Create(Vector3 pos, CONEnemy enemy)
    {
        //Transform arrowPrefab = Resources.Load<Transform>("arrowPrefab");
        //Transform arrowTrm = Instantiate(arrowPrefab, pos, Quaternion.identity);

        Transform arrowTrm = GameSceneClass.gMGPool.CreateObj(ePrefabs.Arrow, pos).transform;

        CONArrow arrowProjectile = arrowTrm.GetComponent<CONArrow>();

        arrowProjectile.SetTarget(enemy);

        return arrowProjectile;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        timeToDie = 2f;
    }

    public override void Update()
    {
        base.Update();

        Vector3 moveDir;
        if (targetEnemy != null && !targetEnemy.IsActive())
        {
            targetEnemy = null;
        }

        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }

        float moveSpeed = 20f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;


        float radians = Mathf.Atan2(moveDir.y, moveDir.x);
        float degree = Mathf.Rad2Deg * radians;

        transform.eulerAngles = new Vector3(0, 0, degree);

        timeToDie -= Time.deltaTime;

        if (timeToDie < 0f)
        {
            SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CONEnemy enemy = collision.GetComponent<CONEnemy>();
        if (enemy != null)
        {
            enemy.GetComponent<HealthSystem>().Damage(10);

            SetActive(false);
        }
    }

    private void SetTarget(CONEnemy enemy)
    {
        targetEnemy = enemy;
    }
}
