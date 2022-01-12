using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherState
{
    public enum eState
    {
        IDLE, ATTACK
    }

    public enum eEvent
    {
        ENTER, UPDATE, EXIT
    }

    public eState stateName;

    protected eEvent curEvent;

    protected GameObject myObj;
    protected Animator myAnim;
    protected GameObject targetEnemy;

    protected ArcherState nextState;

    float attackDist = 20.0f;

    public ArcherState(GameObject obj, Animator anim, GameObject targetEnemy)
    {
        myObj = obj;
        myAnim = anim;
        this.targetEnemy = targetEnemy;

        curEvent = eEvent.ENTER;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    public ArcherState Process()
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

    public bool IsDetectEnemy(out GameObject closestEnemy)
    {
        closestEnemy = null;
        float distance = Mathf.Infinity;
        Collider2D[] hit = Physics2D.OverlapBoxAll(myObj.transform.position, new Vector2(attackDist, 1), 0, 1 << LayerMask.NameToLayer("Enemy"));

        if (hit.Length > 0) // 적이 보인다 (true)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                float dis = Vector3.Distance(myObj.transform.position, hit[i].transform.position);
                if (dis < distance)
                {
                    distance = dis;
                    closestEnemy = hit[i].gameObject;
                }
            }

            return true;
        }

        return false;
    }
}

public class ArcherIdle : ArcherState
{
    public ArcherIdle(GameObject obj, Animator anim, GameObject targetEnemy) : base(obj, anim, targetEnemy)
    {
        stateName = eState.IDLE;
    }

    public override void Enter()
    {
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

public class ArcherAttack : ArcherState
{
    public ArcherAttack(GameObject obj, Animator anim, GameObject targetEnemy) : base(obj, anim, targetEnemy)
    {
        stateName = eState.ATTACK;
        this.targetEnemy = targetEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Vector3 dir = myObj.transform.position - targetEnemy.transform.position;

        if (!IsDetectEnemy(out GameObject enemy))
        {
            nextState = new ArcherIdle(myObj, myAnim, null);
            curEvent = eEvent.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
