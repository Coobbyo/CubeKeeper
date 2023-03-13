using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBuilder
{
    public List<Structure> structures { get; private set; }

    public ClanBuilder()
    {
        structures = new List<Structure>();
    }

    public StructureData nextStructureToBuild()
    {
        return BuildManager.Instance.GetStructure(BuildManager.Build.ClanHall);
    }
}
