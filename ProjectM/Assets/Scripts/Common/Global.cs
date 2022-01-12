using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global
{
    public static Dictionary<ePrefabs, GameObject> prefabsDic;
    public static Dictionary<string, Sprite> spritesDic;

    public static Vector2 referenceResolution;
    public static Image blackPannel;
    internal static eGameStatus _gameStat;
}

public enum ePrefabs
{
    None = -1,
    MainCamera,
    HEROS = 1000,
    HeroGuard,
    HeroSword,
    Archer,
    Arrow,
    Enemy,
    MANAGERS = 2000,
    MGPool,
    MGGame,
    MGEnemyWave,
    UI = 3000,
    UIRoot,
    UIRootLoading,
    UIRootTitle,
    UIRootGame,
}

public enum eSceneName
{
    None =-1,
    Loading,
    Title,
    Game,     
}

public enum eGameStatus
{
    None = -1,
    Playing,
}

public class GameSceneClass
{
    public static MGGame gMGGame;
    public static MGPool gMGPool;
    public static UIRootGame gUiRootGame;
}
