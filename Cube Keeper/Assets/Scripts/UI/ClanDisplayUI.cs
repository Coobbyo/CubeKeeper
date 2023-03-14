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
        clanSize.text = clan.Size.ToString();
        numFriends.text = clan.Friends.Count.ToString();
        numEnemies.text = clan.Enemies.Count.ToString();

        memberList.Propigate(clan.Members);
        friendList.Propigate(clan.Friends);
        enemyList.Propigate(clan.Enemies);
    }

    public void SetClan(NPCClan clan)
    {
        this.clan = clan;
    }
}
