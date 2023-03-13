using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Structure Data")]
public class StructureData : ScriptableObject
{
    public string id;
    public string displayName;
    public ItemData[] resourceList;
    public int[] resourceAmounts;
    //public Sprite icon;
    public GameObject prefab;
}
