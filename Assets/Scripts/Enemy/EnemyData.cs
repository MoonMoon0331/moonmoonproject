using UnityEngine;
using System.Collections.Generic;
using Ink.Runtime;

[System.Serializable]
public class EnemyData
{
    public int enemyID; // 詐騙對象ID
    public string enemyName;   // 詐騙對象名稱
    public string enemyCallNumber; // 詐騙對象的電話號碼
    public string enemyInformation; // 詐騙對象的資訊
    public TextAsset _inkAssets; // 詐騙對象的對話內容
    public int enemyDifficulty; // 詐騙對象的難度

    [Header("Enemy Sprite")]
    public Sprite enemySprite; // 詐騙對象的圖片
    public Sprite enemySpriteAngry; // 生氣的圖片
    public Sprite enemySpriteSad; // 難過的圖片
    public Sprite enemySpriteHappy; // 高興的圖片
    public Sprite enemySpriteSurprised; // 驚訝的圖片
    public Sprite enemySpriteThinking; // 思考的圖片

    [Header("Enemy Portrait")]
    public Sprite enemyPortrait; // 詐騙對象的頭像
    public Sprite enemyPortraitAngry; // 生氣的頭像
    public Sprite enemyPortraitSad; // 難過的頭像
    public Sprite enemyPortraitHappy; // 高興的頭像
    public Sprite enemyPortraitSurprised; // 驚訝的頭像
    public Sprite enemyPortraitThinking; // 思考的頭像
}



[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Data/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemyList = new List<EnemyData>(); // 詐騙對象清單
}

[CreateAssetMenu(fileName = "EnemyRuntimeDatabase", menuName = "Data/EnemyRuntimeDatabase")]
public class EnemyRuntimeDatabase : ScriptableObject
{
    public List<EnemyRuntimeData> enemyRuntimeList = new List<EnemyRuntimeData>(); // 詐騙對象狀態清單
}