using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGEnemyWave : MonoBehaviour
{
    public Action<int> OnWaveNumberChanged;
    public Action<float> OnEnemySpawnAmountChanged;

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
    //[SerializeField] private Transform spawnPositionTransform;
    private Vector3 spawnPosition;


    private void Awake()
    {
        spawnPosition = transform.Find("enemySpawnPos").position;
    }

    private void Start()
    {
        // �ּ� ���� ���� �� ����
        state = eWaveState.WaitingToSpawnNextWave;

        // ���� ���� ���� �� 3�� ���Ŀ� ��ȯ ����
        nextWaveSpawnTimer = 3f;
    }

    public void UpdateDefence()
    {
        switch (state)
        {
            case eWaveState.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
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
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .4f);

                        // ���� �� ���� �� remainingEnemySpawnAmount �ϳ��� ����
                        CONEnemy.Create(spawnPosition);
                        remainingEnemySpawnAmount--;
                        OnEnemySpawnAmountChanged?.Invoke(nextEnemySpawnTimer);

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
        return spawnPosition;
    }
}
