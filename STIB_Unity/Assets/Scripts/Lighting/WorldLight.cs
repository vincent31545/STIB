using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WorldLight : MonoBehaviour
{
    private void Update() {
        Shader.SetGlobalVector("_WorldLightDirection", transform.forward);
    }
}
