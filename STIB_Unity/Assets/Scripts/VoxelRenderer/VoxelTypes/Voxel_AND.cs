using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_AND : Voxel {

    public Voxel_AND(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {

    }
  
    public override Color GetVoxelColor() => Color.grey;

    public override void UpdateSignal() {
        int counter = 0;
        
        for (int i = 0; i < 6; i++) {
            if (adjacent[i] == null || i == forward) continue;
            // Invert signal position
            // ie if block on the right outgoing then block on left incoming 
            if (adjacent[i].signals[ (i%2 == 1) ? (i-1) : (i+1)]) {
                counter++;
            }
        }
        
        // Set output 
        if (counter < 2) SendSignal(false);
        else SendSignal(true);
    }
}
