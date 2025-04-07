using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRuntimeDataBase : ScriptableObject
{
    public List<NPCRuntimeData> npcRuntimeList = new List<NPCRuntimeData>(); // NPC狀態清單

    public NPCRuntimeData GetNPCData(int npcID)
    {
        foreach (var npc in npcRuntimeList)
        {
            if (npc.npcID == npcID) // 如果找到對應的NPCID
            {
                return npc;
            }
        }
        return null; // 如果找不到，返回null
    }
}
