using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBehaviour : MonoBehaviour
{
    private Structure structure;
    public Structure Structure
    {
        get { return structure; }
        set
        {
            structure = value;
            foreach (MeshRenderer mesh in displayMeshes)
            {
                mesh.material.color = structure.clan.GetColor();
            }
        }
    }

    [SerializeField] private MeshRenderer[] displayMeshes;
}
