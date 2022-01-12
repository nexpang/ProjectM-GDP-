using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public enum eState
    {
        PURSUE, ATTACK, DEAD
    }

    public enum eEvent
    {
        ENTER, UPDATE, EXIT
    }

    public eState stateName;

    protected eEvent curEvent;

    protected GameObject myObj;
    protected Animator myAnim;
    protected Transform playerTrm;

    protected EnemyState nextState;

    float attackDist = 7.0f;

    public EnemyState(GameObject obj, Animator anim, Transform targetTrm)
    {
        myObj = obj;
        myAnim = anim;
        playerTrm = targetTrm;

        curEvent = eEvent.ENTER;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    public EnemyState Process()
    {
        if (curEvent == eEvent.ENTER) Enter();
        if (curEvent == eEvent.UPDATE) Update();
        if (curEvent == eEvent.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}

public class EnemyPursue : EnemyState
{
    public EnemyPursue(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
    {
        stateName = eState.PURSUE;
    }

    public override void Enter()
    {
        myAnim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // 성 공격이되면 공격
/*        if (CanAttackPlayer())
        {
            nextState = new Attack(myObj, myAgent, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }*/
    }

    public override void Exit()
    {
        myAnim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class EnemyAttack : EnemyState
{
    public EnemyAttack(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
    {
        stateName = eState.ATTACK;
    }

    public override void Enter()
    {
        myAnim.SetTrigger("isShooting");
        base.Enter();
    }

    public override void Update()
    {
        // 공격 범위 벗어나면 다시 추적
/*        if (!CanAttackPlayer())
        {
            nextState = new Idle(myObj, myAgent, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }*/
    }

    public override void Exit()
    {
        myAnim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class EnemyDead : EnemyState
{
    public EnemyDead(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
    {
        stateName = eState.DEAD;
    }

    public override void Enter()
    {
        myAnim.SetTrigger("isShooting");
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        myAnim.ResetTrigger("isRunning");
        base.Exit();
    }
}