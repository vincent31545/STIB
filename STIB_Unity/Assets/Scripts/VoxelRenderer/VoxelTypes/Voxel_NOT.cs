using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_NOT : Voxel {

    public Voxel_NOT(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("NOT");
    }

    public override Color GetVoxelColor() => Color.green;
}
