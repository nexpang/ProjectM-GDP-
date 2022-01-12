using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONEnemy : CONEntity
{
    // 스테이트 머신을 사용할(필요한) 유닛에게 부착시킬것이다

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
