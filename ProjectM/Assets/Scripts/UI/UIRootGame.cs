using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRootGame : MonoBehaviour
{
    [SerializeField]
    private Image testImage;

    public RectTransform waveBar;
    public Text waveText;

    private MGEnemyWave enemyWave;

    void Awake()
    {
        GameSceneClass.gUiRootGame = this;
    }

    private void Start()
    {
        enemyWave = GameSceneClass.gMGGame._gEnemyWaveManager;
        enemyWave.OnWaveWait += (amount) =>
        {
            float f = Mathf.Clamp(amount/ 7.0f, 0, 1);
            waveBar.localScale = new Vector2(f, waveBar.localScale.y);
        };
        enemyWave.OnWaveNumberChanged += (waveNum) =>
        {
            waveText.text = $"WAVE {waveNum}";
        };
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<string> keyList = new List<string>(Global.spritesDic.Keys);
            int randomIdx = Random.Range(0, keyList.Count - 1);
            
            testImage.sprite = Global.spritesDic[keyList[randomIdx]];
            testImage.SetNativeSize();
        }
    }

    public void TestFunc()
    {
        print("call UIRootGame");
    }
}
