using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDataBase", menuName = "Data/NPCDataBase")]
public class NPCDataBase : ScriptableObject
{
    public List<NPCData> npcList = new List<NPCData>(); // NPC清單
}
