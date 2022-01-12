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
    protected GameObject targetEnemy;
    protected SpriteRenderer sr;

    protected HeroState nextState;

    float detectDist = 15.0f;
    float attackDist = 4.0f;
    protected float speed = 3.0f;

    public HeroState(GameObject obj, Animator anim, GameObject targetEnemy)
    {
        myObj = obj;
        myAnim = anim;
        this.targetEnemy = targetEnemy;
        sr = myObj.GetComponent<SpriteRenderer>();

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
        Collider2D[] hit = Physics2D.OverlapBoxAll(myObj.transform.position, new Vector2(detectDist, 1), 0, 1 << LayerMask.NameToLayer("Enemy"));

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

    public bool IsDetectEnemy_AttackRange(out GameObject closestEnemy)
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

public class HeroPatrol : HeroState
{
    Vector3[] heroWaypointPos;
    int curIndex = 0;

    public HeroPatrol(GameObject obj, Animator anim, GameObject targetEnemy) : base(obj, anim, targetEnemy)
    {
        stateName = eState.PATROL;
        speed = 3f;
    }

    public override void Enter()
    {
        GameObject[] heroWaypoint = GameObject.FindGameObjectsWithTag("HeroWaypoint");
        heroWaypointPos = new Vector3[heroWaypoint.Length];

        for(int i =0; i<heroWaypoint.Length;i++)
        {
            heroWaypointPos[i] = new Vector3(heroWaypoint[i].transform.position.x, myObj.transform.position.y, heroWaypoint[i].transform.position.z);
        }

        Debug.Log(heroWaypoint.Length);
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
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class HeroPursue : HeroState
{
    public HeroPursue(GameObject obj, Animator anim, GameObject targetEnemy) : base(obj, anim, targetEnemy)
    {
        stateName = eState.PURSUE;
        speed = 5f;
        this.targetEnemy = targetEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        myObj.transform.position = Vector3.MoveTowards(myObj.transform.position, targetEnemy.transform.position, speed * Time.deltaTime);

        float flipDir = myObj.transform.position.x - targetEnemy.transform.position.x;
        sr.flipX = (flipDir > 0);

        if (IsDetectEnemy_AttackRange(out GameObject enemy))
        {
            nextState = new HeroAttack(myObj, myAnim, enemy);
            curEvent = eEvent.EXIT;
        }
        else if (!IsDetectEnemy(out GameObject enemy2))
        {
            nextState = new HeroPatrol(myObj, myAnim, null);
            curEvent = eEvent.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class HeroAttack : HeroState
{
    public HeroAttack(GameObject obj, Animator anim, GameObject targetEnemy) : base(obj, anim, targetEnemy)
    {
        stateName = eState.ATTACK;
        speed = 5f;
        this.targetEnemy = targetEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Vector3 dir = myObj.transform.position - targetEnemy.transform.position;

        if (dir.magnitude >= 1)
        {
            myObj.transform.position = Vector3.MoveTowards(myObj.transform.position, targetEnemy.transform.position, speed * Time.deltaTime);
        }

        float flipDir = myObj.transform.position.x - targetEnemy.transform.position.x;
        sr.flipX = (flipDir > 0);

        if (!IsDetectEnemy(out GameObject enemy2))
        {
            nextState = new HeroPatrol(myObj, myAnim, null);
            curEvent = eEvent.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
