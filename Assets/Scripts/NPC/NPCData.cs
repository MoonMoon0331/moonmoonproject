using UnityEngine;
using System.Collections.Generic;
using Ink.Runtime;

[System.Serializable]
public class NPCData
{
    public int npcID; // NPC ID
    public string npcName; // NPC名稱
    public TextAsset _inkAssets; // NPC的對話內容

    [Header("NPC Sprite")]
    public Sprite npcSprite; // NPC的圖片
    public Sprite npcSpriteAngry; // 生氣的圖片
    public Sprite npcSpriteSad; // 難過的圖片
    public Sprite npcSpriteHappy; // 高興的圖片
    public Sprite npcSpriteSurprised; // 驚訝的圖片
    public Sprite npcSpriteThinking; // 思考的圖片

    [Header("NPC Portrait")]
    public Sprite npcPortrait; // NPC的頭像
    public Sprite npcPortraitAngry; // 生氣的頭像
    public Sprite npcPortraitSad; // 難過的頭像
    public Sprite npcPortraitHappy; // 高興的頭像
    public Sprite npcPortraitSurprised; // 驚訝的頭像
    public Sprite npcPortraitThinking; // 思考的頭像
}
