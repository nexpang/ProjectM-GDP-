using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    // 스테이트 머신을 사용할(필요한) 유닛에게 부착시킬것이다

    Animator myAnim;
    ArcherState curState;
    GameObject targetEnemy;

    private void Start()
    {
        myAnim = this.GetComponent<Animator>();
        curState = new ArcherIdle(this.gameObject, myAnim, targetEnemy);
    }

    private void Update()
    {
        curState = this.curState.Process();
    }
}
