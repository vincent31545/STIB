using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelRenderer : MonoBehaviour
{
    public Mesh renderingMesh;
    public Material renderingMaterial;

    private RenderParams renderParams;
    private Matrix4x4[] voxelMatrices;
    private Vector4[] voxelColors;

    private void Awake() {
    
    }
    private void Start() {
        WorldManager.RegisterAddVoxelEvent(RefreshMatrices);
        WorldManager.RegisterRemoveVoxelEvent(RefreshMatrices);
        WorldManager.RegisterClearVoxelsEvent(RefreshMatrices);
    }

    private void Update() {
        Graphics.RenderMeshInstanced(renderParams, renderingMesh, 0, voxelMatrices);
    }

    private void RefreshMatrices() {        
        voxelMatrices = new Matrix4x4[WorldManager.GetVoxelCount()];
        voxelColors = new Vector4[WorldManager.GetVoxelCount()];
        for (int i = 0; i < voxelMatrices.Length; i++) {
            voxelMatrices[i] = Matrix4x4.TRS(WorldManager.GetVoxel(i).position + (Vector3.up + Vector3.forward + Vector3.right) * 0.5f, Quaternion.identity, Vector3.one);

            voxelColors[i] = WorldManager.GetVoxel(i).GetVoxelColor();
        }

        renderParams = new RenderParams(renderingMaterial);
        MaterialPropertyBlock materialProp = new MaterialPropertyBlock();
        materialProp.SetVectorArray("_InstanceColor", voxelColors);
        renderParams.matProps = materialProp;
    }
}
