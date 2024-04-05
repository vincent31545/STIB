using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_LED: Voxel {

    public Voxel_LED(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {

    }

    public override void Initialize() {
        base.Initialize();
    }

    private bool ledON = false;
  
    public override Color GetVoxelColor() => ledON ? WorldManager.instance.voxelRenderer.ledOnColor : Color.grey;

    public override void UpdateSignal() {
       for (int i = 0; i < 6; i++) {
            if (adjacent[i] == null) continue;
            // Invert signal position
            // ie if block on the right outgoing then block on left incoming 
            if (adjacent[i].signals[ (i%2 == 1) ? (i-1) : (i+1)]) {
                if (!ledON) {
                    ledON = true;
                    WorldManager.instance.voxelRenderer.RefreshColorData();
                }
                return;
            }
        } 
        if (ledON) {
            ledON = false;
            WorldManager.instance.voxelRenderer.RefreshColorData();
        }
    }
}

