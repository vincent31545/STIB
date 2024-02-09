using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelRenderer : MonoBehaviour
{
    public Mesh renderingMesh;
    public Material renderingMaterial;

    private RenderParams renderParams;
    private Matrix4x4[] voxelMatrices;

    private void Awake() {
        renderParams = new RenderParams(renderingMaterial);
    }
    private void Start() {
        WorldManager.RegisterAddVoxelEvent(RefreshMatrices);
        WorldManager.RegisterRemoveVoxelEvent(RefreshMatrices);
        WorldManager.RegisterClearVoxelsEvent(RefreshMatrices);

        for (int x = 0; x < 10; x++)
            for (int z = 0; z < 10; z++)
                WorldManager.AddVoxel(new Voxel(VOXEL_TYPE.TEST, new Vector3(x * 2, 0, z * 2)));
    }

    private void Update() {
        Graphics.RenderMeshInstanced(renderParams, renderingMesh, 0, voxelMatrices);
    }

    private void RefreshMatrices() {
        voxelMatrices = new Matrix4x4[WorldManager.GetVoxels().Count];
        for (int i = 0; i < voxelMatrices.Length; i++) {
            voxelMatrices[i] = Matrix4x4.TRS(WorldManager.GetVoxel(i).position, Quaternion.identity, Vector3.one);
        }
    }
}
