using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Voxel_SPLIT: Voxel {

    public Voxel_SPLIT(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {

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
                for (int j = 0; j < signals.Length; j++) {
                    signals[j] = false;
                }
                signals[forward] = true;
                signals[(forward%2 == 1) ? (i-1) : (i+1)] = true;
                // Gives the red circle texture
                WorldManager.instance.voxelRenderer.SetVoxData(new Vector4(1, 0, 0, 0), WorldManager.GetVoxelIndex(this));
                return;
            }
        }
        
        // if (adjacent[direction].signals[forward]) {
        //     SendSignal(true);
        //     return;
        // } 
        for (int i = 0; i < signals.Length; i++) {
            signals[i] = false;
        }
        WorldManager.instance.voxelRenderer.SetVoxData(new Vector4(0, 0, 0, 0), WorldManager.GetVoxelIndex(this));
    }

    public override Color GetVoxelColor() => Color.black;
}

