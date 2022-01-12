using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState
{
    public enum eState
    {
        PATROL, PURSUE, ATTACK
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

    protected HeroState nextState;

    float detectDist = 10.0f;
    float attackDist = 7.0f;

    public HeroState(GameObject obj, Animator anim, Transform targetTrm)
    {
        myObj = obj;
        myAnim = anim;
        playerTrm = targetTrm;

        curEvent = eEvent.ENTER;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    public HeroState Process()
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

public class HeroPatrol : HeroState
{
    public HeroPatrol(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
    {
        stateName = eState.PATROL;
    }

    public override void Enter()
    {
        myAnim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // ���� ���̸� ����
/*        if (CanSeePlayer())
        {
            nextState = new Patrol(myObj, myAgent, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }*/
    }

    public override void Exit()
    {
        myAnim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class HeroPursue : HeroState
{
    public HeroPursue(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
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

        // ���� ��������� ����, ���ٸ� ��Ʈ��
/*        if (CanAttackPlayer())
        {
            nextState = new Attack(myObj, myAgent, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }
        else if (!CanSeePlayer())
        {
            nextState = new Patrol(myObj, myAgent, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }*/
    }

    public override void Exit()
    {
        myAnim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class HeroAttack : HeroState
{
    public HeroAttack(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
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
        // Ÿ���õ� ������ ��������� üũ �� �ٽ� Patrol�� ���ư��� ��
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
