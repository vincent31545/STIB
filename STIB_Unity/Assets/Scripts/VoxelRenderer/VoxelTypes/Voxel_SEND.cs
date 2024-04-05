using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_SEND: Voxel {

    public Voxel_SEND(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {

    }

    public override void Initialize() {
        base.Initialize();
        SendSignal(true);
    }
  
    public override Color GetVoxelColor() => Color.red;

    public override void UpdateSignal() {
        SendSignal(true);
    }
}
