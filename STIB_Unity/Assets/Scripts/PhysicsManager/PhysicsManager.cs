using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class PhysicsManager: MonoBehaviour {

    void Start() { }

    public static Voxel GetVoxelHit(Vector3 rayOrigin, Vector3 rayDir, float rayLength, out Vector3Int normal) {
        var norm = rayDir.normalized; 
        int sectionCount = Mathf.CeilToInt(rayLength / 0.01f);

        Voxel v; 
        Vector3Int current, last = Vector3Int.zero; 

        for (int i = 0; i < sectionCount; i++) {
            current = WorldManager.GetGridPos(rayOrigin + norm * 0.01f * i); 
            v = WorldManager.GetVoxel(current); 

            if (v != null) { 
                if (last == Vector3Int.zero)
                    normal = Vector3Int.down; 
                else
                    normal = (last - v.position); 
                return v; 
            }
            last = current; 
        }
        
        normal = Vector3Int.zero; return null; 
    }
}