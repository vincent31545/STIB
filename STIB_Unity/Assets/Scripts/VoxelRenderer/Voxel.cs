using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voxel
{
    public Voxel(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) {
        type = _type;
        position = _pos;
        adjacent = new Voxel[6];
        forward = 0;
        for (int i = 0; i < _adjacent.Length; i++) {
            if (_adjacent[i] != null) {
                adjacent[i] = _adjacent[i];
            }
        }
        signals = new bool[6] {false, false, false, false, false, false};
    }
    public virtual void Initialize() {

    }

    public bool RemoveAdjacent(int pos) {

        // Calculating which direction
        if (pos%2 == 1) pos--;
        else pos++;

        // Check that there is actually a block there
        if (adjacent[pos] == null) return false;

        adjacent[pos] = null;
        return true;
    }

    // This function is updated every tick
    public void SendSignal(bool sig) {
        signals[forward] = sig;
        // Gives the red circle texture
        WorldManager.instance.voxelRenderer.SetVoxData(new Vector4(sig ? 1 : 0, 0, 0, 0), WorldManager.GetVoxelIndex(this));
    }

    // This takes in a direction and turns it off before forward is called
    // Directions (0-5): x, -x, y, -y, z, -z
    public void TurnSignalOff (int dir) {
        signals[dir] = false;
    }

    public virtual void UpdateSignal() { }

    public VOXEL_TYPE type;
    public Vector3Int position;
    public Voxel[] adjacent;
    public bool[] signals;

    public bool invincible = false;

    // x, -x, y, -y, z, -z
    public int forward;

    public virtual Color GetVoxelColor() => Color.white;
}


public enum VOXEL_TYPE {
    None = -1,
    SEND = 0,
    WIRE = 1,
    LED = 2,
    NOT = 3,
    OR = 4,
    AND = 5
}
