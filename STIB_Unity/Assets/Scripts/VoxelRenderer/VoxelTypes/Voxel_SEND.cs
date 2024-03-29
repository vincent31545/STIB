using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_SEND: Voxel {

    public Voxel_SEND(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("Created SEND BLOCK");
    }

    public override void Initialize() {
        base.Initialize();
        SendSignal(true);
        Debug.Log("Sending Signal");
    }

  
    public override Color GetVoxelColor() => Color.black;

    public override void UpdateSignal() {}
}
