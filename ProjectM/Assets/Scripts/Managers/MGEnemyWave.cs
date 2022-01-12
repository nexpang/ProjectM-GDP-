using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MGEnemyWave : MonoBehaviour
{
    public Action<int> OnWaveNumberChanged;
    public Action<float> OnWaveWait;

    // 1 ���̺� �⺻
    private enum eWaveState
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    private eWaveState state;


    // 2 ���̺� �ѹ��� ���� ���� ������
    private int waveNumber; // ���̺� ���ڴ� �ð��� �� ���� ������
    private float nextWaveSpawnTimer;   // ���� ���̺������ ���ð�
    private float nextEnemySpawnTimer;  // �� ������ ���ÿ� �������� ������ �����ϱ� ���� Ÿ�̸�
    private int remainingEnemySpawnAmount;  // �� ���̺꿡 ����� ���� ������ ������?


    // 3 ���� ������ ����Ʈ�� ����
    private Transform spawnPositionTransform;
    private List<Vector3> spawnPositionList;


    public MGEnemyWave(Transform spwans)
    {
        spawnPositionList = new List<Vector3>();
        this.spawnPositionTransform = spwans;
        if (spawnPositionList == null)
        {
            print("tq");
        }
        else
        {
            for (int i = 0; i < spawnPositionTransform.childCount; i++)
            {
                spawnPositionList.Add(spawnPositionTransform.GetChild(i).position);
            }
        }

        //spawnPositionList.Add(new Vector3(26f, -1.54f, 0f));
        //spawnPositionList.Add(new Vector3(26f, -3.93f, 0f));

        // �ּ� ���� ���� �� ����
        state = eWaveState.WaitingToSpawnNextWave;

        // ���� ���� ���� �� 3�� ���Ŀ� ��ȯ ����
        nextWaveSpawnTimer = 3f;
    }

    //private void Awake()
    //{
    //    spawnPositionTransform = transform.Find("enemySpawnPos");
    //    for (int i = 0; i < spawnPositionTransform.childCount; i++)
    //    {
    //        spawnPositionList.Add(spawnPositionTransform.GetChild(i));
    //    }
    //}

    //private void Start()
    //{
    //    // �ּ� ���� ���� �� ����
    //    state = eWaveState.WaitingToSpawnNextWave;

    //    // ���� ���� ���� �� 3�� ���Ŀ� ��ȯ ����
    //    nextWaveSpawnTimer = 3f;
    //}

    public void UpdateDefence()
    {
        switch (state)
        {
            case eWaveState.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                OnWaveWait?.Invoke(nextWaveSpawnTimer);
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;
            case eWaveState.SpawningWave:
                /*
                    ���� ������ ���� ���ڰ� 0�� �� ������ ��ȯ��
                    nextEnemySpawnTimer �� ������ �Ź� 200ms�� �������� �����ν� ��ġ�� �����ϴ°� ������ �� ����
                */
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0.5f, 0.7f);

                        // ���� �� ���� �� remainingEnemySpawnAmount �ϳ��� ����
                        CONEnemy.Create(GetSpawnPosition());
                        remainingEnemySpawnAmount--;

                        // ���� ������ ���� ��� �����ߴٸ� ���ο� ������ġ�� �������� �ް� �ٽ� ���� �����·�...
                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = eWaveState.WaitingToSpawnNextWave;

                            nextWaveSpawnTimer = 7f;   // �̷������� �ܺν�Ʈ�� ����
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        // ���̺� ���ڰ� �þ���� �����ϴ� ���� ���ڷ� ���� �÷���
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;     // �̷������� �ܺν�Ʈ�� ����

        state = eWaveState.SpawningWave;
        waveNumber++;

        OnWaveNumberChanged?.Invoke(waveNumber);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        int randIdx = Random.Range(0, spawnPositionList.Count);
        return spawnPositionList[randIdx];
    }
}
