using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Voxel_WIRE: Voxel {

    public Voxel_WIRE(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("Created WIRE BLOCK");
    }

    public override void Initialize() {
        base.Initialize();
        SendSignal(false);
    }
  
    public override void UpdateSignal() {
        
        // Checking all inputs
        
        for (int i = 0; i < 6; i++) {
            if (adjacent[i] == null || i == forward) continue;
            // Invert signal position
            // ie if block on the right outgoing then block on left incoming 
            if (adjacent[i].signals[ (i%2 == 1) ? (i-1) : (i+1)]) {
                SendSignal(true);
                return;
            }
        }
        

        // int direction = (forward%2 == 1) ? (forward - 1) : (forward + 1);
        // if (adjacent[direction] == null) {
        //     SendSignal(false);
        //     return;
        // }
        // // x, -x, y, -y, z, -z
        // if (adjacent[direction])

        // This is Daniel previous code that just checks the current direction
        // if (adjacent[direction].signals[forward]) {
        //     SendSignal(true);
        //     return;
        // } 
        SendSignal(false);
    }

    public override Color GetVoxelColor() => Color.black;
}

