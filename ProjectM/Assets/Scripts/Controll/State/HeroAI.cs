using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAI : MonoBehaviour
{
    // ������Ʈ �ӽ��� �����(�ʿ���) ���ֿ��� ������ų���̴�

    Animator myAnim;

    public Transform playerTrm;

    HeroState curState;

    private void Start()
    {
        myAnim = this.GetComponent<Animator>();
        curState = new HeroPatrol(this.gameObject, myAnim, playerTrm);
    }

    private void Update()
    {
        curState = this.curState.Process();
    }
}
