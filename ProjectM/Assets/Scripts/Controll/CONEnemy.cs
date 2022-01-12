using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONEnemy : CONEntity
{
    // ������Ʈ �ӽ��� �����(�ʿ���) ���ֿ��� ������ų���̴�

    Animator myAnim;
    EnemyState curState;
    Transform targetCastle;
    public static CONEnemy Create(Vector3 pos)
    {
        Transform enemyTrm = GameSceneClass.gMGPool.CreateObj(ePrefabs.Enemy, pos).transform;

        return enemyTrm.GetComponent<CONEnemy>();
    }

    public override void Start()
    {
        base.Start();
        targetCastle = GameObject.FindGameObjectWithTag("Castlepoint").transform;
        myAnim = this.GetComponent<Animator>();
        curState = new EnemyPursue(this.gameObject, myAnim, targetCastle);
    }

    public override void Update()
    {
        base.Update();
        curState = this.curState.Process();
    }
}
