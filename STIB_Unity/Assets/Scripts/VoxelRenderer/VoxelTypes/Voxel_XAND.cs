using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_XAND : Voxel {

    public Voxel_XAND(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {

    }

    public override Color GetVoxelColor() => Color.blue;
}
