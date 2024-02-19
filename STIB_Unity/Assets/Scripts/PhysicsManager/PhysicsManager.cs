using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class PhysicsManager: MonoBehaviour
{
    List<Voxel> voxels;

    void Start() {
        voxels = WorldManager.GetVoxels(); 
    }
    
    public static List<Vector3Int> RaycastBox(Vector3 origin, Vector3 dir, int len) {
        Vector3 norm = Vector3.Normalize(dir);
        List<Vector3Int> hit = new List<Vector3Int>();
        for (int i = 0; i < len; i++) {
            hit.Add(GetGridPos(origin + dir*i));
        }
        return hit;
    }
}