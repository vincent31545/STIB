using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_WIRE: Voxel {

    public Voxel_WIRE(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("CREATION: WIRE block");
        SendSignal(false);
    }
  
    public override void UpdateSignal() {
        
        // Checking all inputs
        for (int i = 0; i < 6; i++) {
            if (adjacent[i] == null || i == forward) continue;
            // Invert signal position
            // ie if block on the right outgoing then block on left incoming 
            if (adjacent[i].signals[ (i%2 == 1) ? i : (i-1)]) {
                SendSignal(true);
                return;
            }
        }
        SendSignal(false);
    }

    public override Color GetVoxelColor() => Color.black;
}

