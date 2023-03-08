using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClanDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text clanName;
    [SerializeField] private TMP_Text clanSize;
    [SerializeField] private TMP_Text numFriends;
    [SerializeField] private TMP_Text numEnemies;

    [SerializeField] private NPCListUI memberList;
    [SerializeField] private ClanListUI friendList;
    [SerializeField] private ClanListUI enemyList;

    [SerializeField] private NPCUI ui;

    private NPCClan clan;

    private void OnEnable()
    {
        clanName.text = clan.ToString();
        clanSize.text = clan.GetClanSize().ToString();
        numFriends.text = clan.GetNumFriends().ToString();
        numEnemies.text = clan.GetNumEnemies().ToString();

        memberList.Propigate(clan.GetMembers());
        friendList.Propigate(clan.GetFriends());
        enemyList.Propigate(clan.GetEnemies());
    }

    public void SetClan(NPCClan clan)
    {
        this.clan = clan;
    }
}
