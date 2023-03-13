using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    public StructureData data { get; private set; }
    public NPCClan clan { get; private set; }
    public Vector3 position { get; private set; }

    public Structure(StructureData referenceData)
    {
        data = referenceData;
        clan = null;
        position = Vector3.zero;
    }
    public Structure(StructureData referenceData, Vector3 location)
    {
        data = referenceData;
        clan = null;
        position = location;
    }
    public Structure(StructureData referenceData, NPCClan owner, Vector3 location)
    {
        data = referenceData;
        clan = owner;
        position = location;
    }
}
