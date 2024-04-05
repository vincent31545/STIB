using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelRenderer : MonoBehaviour
{
    public Mesh renderingMesh;
    public Material renderingMaterial;

    [Header("Random Voxel Data")]
    [ColorUsage(false, true)] public Color ledOnColor;

    private RenderParams renderParams;
    private Matrix4x4[] voxelMatrices;
    private Vector4[] voxelColors;
    private List<Vector4> voxelData = new List<Vector4>();

    private void Awake() {
    
    }
    private void Start() {
        WorldManager.RegisterAddVoxelEvent(RefreshMatrices);
        WorldManager.RegisterRemoveVoxelEvent((v, i) => {
            voxelData.RemoveAt(i);
            RefreshMatrices();
        });
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
            if (i >= voxelData.Count) voxelData.Add(new Vector4(0, 0, 0, 0));
        }

        renderParams = new RenderParams(renderingMaterial);
        MaterialPropertyBlock materialProp = new MaterialPropertyBlock();
        materialProp.SetVectorArray("_InstanceColor", voxelColors);
        materialProp.SetVectorArray("_VoxelData", voxelData);
        renderParams.matProps = materialProp;
    }

    public void SetVoxData(Vector4 data, int voxelIndex) {
        voxelData[voxelIndex] = data;
        renderParams.matProps.SetVectorArray("_VoxelData", voxelData);
    }
    public void RefreshColorData() {
        for (int i = 0; i < voxelMatrices.Length; i++)
            voxelColors[i] = WorldManager.GetVoxel(i).GetVoxelColor();
        renderParams.matProps.SetVectorArray("_InstanceColor", voxelColors);
    }
}
