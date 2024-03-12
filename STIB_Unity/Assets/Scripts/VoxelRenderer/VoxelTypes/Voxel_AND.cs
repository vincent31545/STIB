using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_AND : Voxel {

    public Voxel_AND(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("AND");
    }
  
    public override Color GetVoxelColor() => Color.black;

    public override void UpdateSignal() {
        bool[] inSignal = new bool[6];
        int counter = 0;
        for (int i = 0; i < 6; i++) {
            if (adjacent[i] == null) continue;
            // Invert signal position
            // ie if block on the right outgoing then block on left incoming
            if (i%2 == 1)
                inSignal[i] = adjacent[i].signals[i-1];
                if (adjacent[i].signals[i-1]) counter++;
            else 
                inSignal[i] = adjacent[i].signals[i];
                if (adjacent[i].signals[i]) counter++;
        }
        if (counter < 2) {
            for (int i = 0; i < 6; i++) {
                this.signals[i] = false;
            }
        }  
        else {
            for (int i = 0; i < 6; i++) {
                this.signals[i] = !inSignal[i];
            }
        }
    }
}
