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

    protected HeroState nextState;

    float detectDist = 10.0f;
    float attackDist = 7.0f;
    protected float speed = 3.0f;

    public HeroState(GameObject obj, Animator anim)
    {
        myObj = obj;
        myAnim = anim;

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

    public bool IsDetectEnemy(out GameObject closestEnemy)
    {
        closestEnemy = null;
        float distance = Mathf.Infinity;
        Collider2D[] hit = Physics2D.OverlapCircleAll(myObj.transform.position, 5, 1 << LayerMask.NameToLayer("Enemy"));

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

public class HeroPatrol : HeroState
{
    Vector3[] heroWaypointPos;
    int curIndex = 0;

    SpriteRenderer sr;

    public HeroPatrol(GameObject obj, Animator anim) : base(obj, anim)
    {
        stateName = eState.PATROL;
    }

    public override void Enter()
    {
        myAnim.SetTrigger("isRunning");
        GameObject[] heroWaypoint = GameObject.FindGameObjectsWithTag("HeroWaypoint");
        heroWaypointPos = new Vector3[heroWaypoint.Length];

        for(int i =0; i<heroWaypoint.Length;i++)
        {
            heroWaypointPos[i] = new Vector3(heroWaypoint[i].transform.position.x, myObj.transform.position.y, heroWaypoint[i].transform.position.z);
        }

        Debug.Log(heroWaypoint.Length);

        sr = myObj.GetComponent<SpriteRenderer>();
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        myObj.transform.position = Vector3.MoveTowards(myObj.transform.position, heroWaypointPos[curIndex], speed * Time.deltaTime);

        Vector3 dir = myObj.transform.position - heroWaypointPos[curIndex];
        if (dir.magnitude <= 1)
        {
            if(curIndex == 0)
            {
                curIndex = 1;
                sr.flipX = true;
            }
            else
            {
                curIndex = 0;
                sr.flipX = false;
            }
        }

        if (IsDetectEnemy(out GameObject enemy))
        {
            nextState = new HeroPursue(myObj, myAnim, enemy);
            curEvent = eEvent.EXIT;
        }

        // 적이 보이면 추적
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
    public HeroPursue(GameObject obj, Animator anim, GameObject targetEnemy) : base(obj, anim)
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

        // 적과 가까워지면 공격, 없다면 패트롤
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
    public HeroAttack(GameObject obj, Animator anim) : base(obj, anim)
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
        // 타겟팅된 유닛이 사라졌는지 체크 후 다시 Patrol로 돌아가야 함
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
