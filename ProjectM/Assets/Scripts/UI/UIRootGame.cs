using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIRootGame : MonoBehaviour
{
    public RectTransform waveBar;
    public Text waveText;
    private MGEnemyWave enemyWave;

    public RectTransform manaBar;
    public Text manaText;
    private MGSkill skill;

    public Button Skill1;
    public Image skill1_cool;
    private const float skill1_coolTime = 5;
    private float skill1_curCoolTime = skill1_coolTime;

    public Button Skill2;
    public Image skill2_cool;
    private const float skill2_coolTime = 10;
    private float skill2_curCoolTime = skill2_coolTime;

    public Button skipTimeButton;
    public Text skipTimeText;
    private int skipIndex = 0;

    public Button pauseButton;
    public CanvasGroup pausePanel;
    private float saveTimeScale = 1;

    public Button resumeButton;
    public Button leaveButton;

    void Awake()
    {
        GameSceneClass.gUiRootGame = this;
    }

    private void Start()
    {
        enemyWave = GameSceneClass.gMGGame._gEnemyWaveManager;
        skill = GameSceneClass.gMGGame._gSkillManager;

        enemyWave.OnWaveWait += (amount) =>
        {
            float f = Mathf.Clamp(amount/ 7.0f, 0, 1);
            waveBar.localScale = new Vector2(f, waveBar.localScale.y);
        };
        enemyWave.OnWaveNumberChanged += (waveNum) =>
        {
            waveText.text = $"WAVE {waveNum}";
        };

        skill.OnManaChanged += (mana) =>
        {
            float f = Mathf.Clamp(mana / 20.0f, 0, 1);
            manaBar.localScale = new Vector2(f, waveBar.localScale.y);

            manaText.text = $"{mana} / 20";
        };

        Skill1.onClick.AddListener(() =>
        {
            skill.ManaReduce(8);
            skill.Skill1Active();
        });

        Skill2.onClick.AddListener(() =>
        {
            skill.ManaReduce(10);
            skill.Skill2Active();
        });

        skipTimeButton.onClick.AddListener(SkipTime);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        leaveButton.onClick.AddListener(Leave);
    }

    private void SkipTime()
    {
        skipIndex++;
        if (skipIndex == 0)
        {
            skipTimeText.text = "x1";
            Time.timeScale = 1;
        }
        else if (skipIndex == 1)
        {
            skipTimeText.text = "x2";
            Time.timeScale = 2;
        }
        else if (skipIndex == 2)
        {
            skipTimeText.text = "x3";
            Time.timeScale = 3;
        }
        else if (skipIndex == 3)
        {
            skipIndex = 0;
            skipTimeText.text = "x1";
            Time.timeScale = 1;
        }
    }

    private void Pause()
    {
        saveTimeScale = Time.timeScale;
        Time.timeScale = 0;

        pausePanel.DOFade(1, 0.5f).SetUpdate(true);
        pausePanel.blocksRaycasts = true;
        pausePanel.interactable = true;
    }

    private void Resume()
    {
        Time.timeScale = saveTimeScale;

        pausePanel.DOFade(0, 0.5f).SetUpdate(true);
        pausePanel.blocksRaycasts = false;
        pausePanel.interactable = false;
    }

    private void Leave()
    {
        MGScene.Instance.ChangeScene(eSceneName.Title);
    }
}
