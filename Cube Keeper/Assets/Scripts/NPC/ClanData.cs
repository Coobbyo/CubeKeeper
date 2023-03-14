using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Clan Data")]
public class ClanData : ScriptableObject
{
    public string id;
    public string clanName;
    public Color color;
}
