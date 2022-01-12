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

    float attackDist = 2.0f;
    private HealthSystem healthSystem;

    public EnemyState(GameObject obj, Animator anim, Transform targetTrm)
    {
        myObj = obj;
        myAnim = anim;
        playerTrm = targetTrm;

        healthSystem = myObj.GetComponent<HealthSystem>();

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

    protected bool CanAttackTower()
    {
        Debug.Log(Mathf.Abs((-12) - myObj.transform.position.x));
        if(Mathf.Abs(myObj.transform.position.x - (-12)) < attackDist)
        {
            return true;
        }
        return false;
    }

    protected bool IsDead()
    {
        return healthSystem.isDead();
    }
}

public class EnemyPursue : EnemyState
{
    float moveSpeed;
    private Rigidbody2D rigid;

    public EnemyPursue(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
    {
        stateName = eState.PURSUE;
    }

    public override void Enter()
    {
        moveSpeed = 3f;
        rigid = myObj.GetComponent<Rigidbody2D>();
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        rigid.velocity = Vector2.left * moveSpeed;

        // 성 공격이되면 공격
        if(IsDead())
        {
            nextState = new EnemyDead(myObj, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }
        else if (CanAttackTower())
        {
            rigid.velocity = Vector2.zero;
            nextState = new EnemyAttack(myObj, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class EnemyAttack : EnemyState
{
    private float curAttackCoolDown;
    private float attackCoolDown = 1f;

    public EnemyAttack(GameObject obj, Animator anim, Transform targetTrm) : base(obj, anim, targetTrm)
    {
        stateName = eState.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        curAttackCoolDown += Time.deltaTime;
        if (curAttackCoolDown > attackCoolDown)
        {
            curAttackCoolDown -= attackCoolDown;
            myAnim.SetTrigger("doAttack");
        }
        // 공격 범위 벗어나면 다시 추적
        if (IsDead())
        {
            nextState = new EnemyDead(myObj, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }
        else if (!CanAttackTower())
        {
            nextState = new EnemyPursue(myObj, myAnim, playerTrm);
            curEvent = eEvent.EXIT;
        }
    }

    public override void Exit()
    {
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
        myObj.SetActive(false);
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
    }
}