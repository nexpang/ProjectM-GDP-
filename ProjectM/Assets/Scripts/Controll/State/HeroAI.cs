using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAI : MonoBehaviour
{
    // 스테이트 머신을 사용할(필요한) 유닛에게 부착시킬것이다

    Animator myAnim;
    HeroState curState;

    private void Start()
    {
        myAnim = this.GetComponent<Animator>();
        curState = new HeroPatrol(this.gameObject, myAnim);
    }

    private void Update()
    {
        curState = this.curState.Process();
    }
}
