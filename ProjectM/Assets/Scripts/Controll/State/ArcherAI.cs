using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    // ������Ʈ �ӽ��� �����(�ʿ���) ���ֿ��� ������ų���̴�

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
