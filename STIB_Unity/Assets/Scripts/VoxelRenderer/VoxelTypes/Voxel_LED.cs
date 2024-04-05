using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_LED: Voxel {

    public Voxel_LED(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("LED");
    }

    public override void Initialize() {
        base.Initialize();
        SendSignal(false);
    }
  
    public override Color GetVoxelColor() => Color.grey;

    public override void UpdateSignal() {
       for (int i = 0; i < 6; i++) {
            if (adjacent[i] == null) continue;
            // Invert signal position
            // ie if block on the right outgoing then block on left incoming 
            if (adjacent[i].signals[ (i%2 == 1) ? (i-1) : (i+1)]) {
                Debug.Log("IMMA LIGHT IT UP LIKE ITS DYNAMITE");
                return;
            }
        } 
    }
}

