using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGGame : MonoBehaviour
{
    public MGEnemyWave _gEnemyWaveManager;
    public MGSkill _gSkillManager;
    // public MGStage _gStageManager;
    // public MGMinion _gMinionManager;
    // public MGHero.MGHero _gHeroManager;

    List<CONEntity> heroConList = new List<CONEntity>();

    public float healthAmountMax = 100;

    private float curHealthMount;

    public Action<float> UpdateToewrHealth;
    public Action OnGameOver;

    void Awake()
    {
        GameSceneClass.gMGGame = this;

        // GameSceneClass._gColManager = new MGUCCollider2D();

        Transform spawns = GameObject.FindGameObjectWithTag("EnemySpawnpoint").transform;
        _gEnemyWaveManager = new MGEnemyWave(spawns);
        _gSkillManager = new MGSkill();
        // _gStageManager = new MGStage();
        // _gMinionManager = new MGMinion();
        // _gHeroManager = new MGHero.MGHero();

        // Global._gameStat = eGameStatus.Playing;

        GameObject.Instantiate(Global.prefabsDic[ePrefabs.MainCamera]);

        heroConList.Clear();
    }
    private void Start()
    {
        curHealthMount = healthAmountMax;
    }

    void OnEnable()
    {
        // GamePlayData.init();
        // MGGameStatistics.instance.initData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CONEntity heroCon = GameSceneClass.gMGPool.CreateObj(ePrefabs.HeroSword, UnityEngine.Random.insideUnitCircle);
            heroConList.Add(heroCon);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (heroConList.Count > 0)
            {
                heroConList[heroConList.Count - 1].SetActive(false);
                heroConList.RemoveAt(heroConList.Count - 1);
            }

        }


        if (Global._gameStat == eGameStatus.Playing)
        {
            _gEnemyWaveManager.UpdateDefence();
            _gSkillManager.UpdateSkill();
        }

        //if (Global._gameStat == eGameStatus.Playing)
        //{
        //    if (Global._gameMode == eGameMode.Collect)
        //    {
        //        _gStageManager.UpdateCollect();
        //        _gMinionManager.UpdateCollect();
        //    }
        //    else if (Global._gameMode == eGameMode.Adventure)
        //    {
        //        _gStageManager.UpdateAdventure();
        //        _gMinionManager.UpdateAdventure();
        //        _gHeroManager.UpdateAdventure();
        //    }
        //}
    }

    public void Damage(float damage)
    {
        curHealthMount -= damage;
        UpdateToewrHealth?.Invoke(curHealthMount);
        if(curHealthMount<=0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        OnGameOver();
    }

    void LateUpdate()
    {
        // GameSceneClass._gColManager.LateUpdate();
    }
}
