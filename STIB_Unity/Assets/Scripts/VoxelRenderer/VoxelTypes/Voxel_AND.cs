using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_AND : Voxel {

    public Voxel_AND(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("AND");
    }

    public override Color GetVoxelColor() => Color.black;
}
