using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONEnemy : CONEntity
{
    // ������Ʈ �ӽ��� �����(�ʿ���) ���ֿ��� ������ų���̴�

    Animator myAnim;
    EnemyState curState;
    Transform targetCastle;
    HealthSystem healthSystem;

    public static CONEnemy Create(Vector3 pos)
    {
        Transform enemyTrm = GameSceneClass.gMGPool.CreateObj(ePrefabs.Enemy, pos).transform;
        CONEnemy enemy = enemyTrm.GetComponent<CONEnemy>();
        //enemy.healthSystem = enemyTrm.GetComponent<HealthSystem>();
        //enemy.healthSystem.Revive();
        //enemy.healthSystem.Damage(0);
        //enemy.curState = new EnemyPursue(enemy.gameObject, enemy.myAnim, enemy.targetCastle);

        return enemy;
    }
    public override void Start()
    {
        base.Start();
        targetCastle = GameObject.FindGameObjectWithTag("Castlepoint").transform;
        myAnim = this.GetComponent<Animator>();

        curState = new EnemyPursue(this.gameObject, myAnim, targetCastle);
    }
    public override void OnEnable()
    {
        base.Start();
        healthSystem = myObj.GetComponent<HealthSystem>();
        healthSystem.Revive();
        healthSystem.Damage(0);

        curState = new EnemyPursue(this.gameObject, myAnim, targetCastle);
    }

    public override void Update()
    {
        base.Update();
        curState = this.curState.Process();
    }
}
