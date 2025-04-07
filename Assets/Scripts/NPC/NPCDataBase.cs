using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDataBase", menuName = "Data/NPCDataBase")]
public class NPCDataBase : ScriptableObject
{
    public List<NPCData> npcList = new List<NPCData>(); // NPC清單

    public NPCData GetNPCData(int npcID)
    {
        foreach (var npc in npcList)
        {
            if (npc.npcID == npcID) // 如果找到對應的NPCID
            {
                return npc;
            }
        }
        return null; // 如果找不到，返回null
    }
}
