using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // ������Ʈ �ӽ��� �����(�ʿ���) ���ֿ��� ������ų���̴�

    Animator myAnim;
    EnemyState curState;
    Transform targetCastle;

    private void Start()
    {
        myAnim = this.GetComponent<Animator>();
        curState = new EnemyPursue(this.gameObject, myAnim, targetCastle);
    }

    private void Update()
    {
        curState = this.curState.Process();
    }
}
