using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text npcName;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private NPCUI ui;

    private NPC npc;

    private void OnEnable()
    {
        npcName.text = npc.ToString();
        //statsText.text = 
    }

    public void SetNPC(NPC npc)
    {
        this.npc = npc;
    }
}
