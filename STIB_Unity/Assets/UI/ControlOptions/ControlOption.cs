using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlOption : MonoBehaviour
{
    public Transform icon;
    public CanvasGroup cg;
    [Space]
    public float offScale = 0.5f;
    public float scaleTime = 0.1f;
    [Space]
    public bool active = false;

    public void Refresh() {
        icon.localScale = Vector3.Lerp(icon.localScale, Vector3.one * (active ? 1 : offScale), scaleTime * Time.deltaTime);
    }
}
