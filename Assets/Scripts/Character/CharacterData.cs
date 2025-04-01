using UnityEngine;
using System.Collections.Generic;
using Ink.Runtime;

[System.Serializable]
public class CharacterData
{
    public int characterID;               // 角色ID
    public string characterName;          // 角色名稱
    public string characterInformation;   // 角色資訊，例如背景描述
    public TextAsset conversationInkAsset; // 角色對話內容

    [Header("Character Sprite")]
    public Sprite characterSprite;        // 角色圖片
    public Sprite characterSpriteAngry;   // 生氣的圖片
    public Sprite characterSpriteHappy;   // 高興的圖片
    public Sprite characterSpriteSad;     // 難過的圖片
    public Sprite characterSpriteSurprised; // 驚訝的圖片
    public Sprite characterSpriteThinking; // 思考的圖片

    [Header("Character Portrait")]
    public Sprite characterPortrait;        // 角色頭像
    public Sprite characterPortraitAngry;   // 生氣的頭像
    public Sprite characterPortraitHappy;   // 高興的頭像
    public Sprite characterPortraitSad;     // 難過的頭像
    public Sprite characterPortraitSurprised; // 驚訝的頭像
    public Sprite characterPortraitThinking;  // 思考的頭像
    [Header("Character Action")]
    public Sprite characterAction;        // 角色行動圖片
}

// 角色動態資料
[System.Serializable]
public class CharacterRuntimeData
{
    public int characterID;
    public enum CharacterRuntimeState { Locked, Available, Busy }
    public int characterProgress; // 例如完成度或其他進度參數
    public CharacterRuntimeState state = CharacterRuntimeState.Locked;
}

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Data/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    public List<CharacterData> characterList = new List<CharacterData>(); // 角色清單
}

[CreateAssetMenu(fileName = "CharacterRuntimeDatabase", menuName = "Data/CharacterRuntimeDatabase")]
public class CharacterRuntimeDatabase : ScriptableObject
{
    public List<CharacterRuntimeData> characterRuntimeList = new List<CharacterRuntimeData>(); // 角色動態狀態清單
}
