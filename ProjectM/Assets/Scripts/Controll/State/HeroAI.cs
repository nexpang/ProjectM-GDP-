using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAI : MonoBehaviour
{
    // ������Ʈ �ӽ��� �����(�ʿ���) ���ֿ��� ������ų���̴�

    Animator myAnim;
    HeroState curState;
    GameObject targetEnemy;

    private void Start()
    {
        myAnim = this.GetComponent<Animator>();
        curState = new HeroPatrol(this.gameObject, myAnim, targetEnemy);
    }

    private void Update()
    {
        curState = this.curState.Process();
    }
}
