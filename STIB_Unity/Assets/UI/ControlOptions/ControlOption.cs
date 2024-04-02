using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlOption : MonoBehaviour
{
    public Transform icon;
    public CanvasGroup cg;
    public TextMeshProUGUI nameText;
    [Space]
    public float offScale = 0.5f;
    public float scaleTime = 0.1f;
    [Space]
    public bool active = false;

    public void Refresh(int i) {
        icon.localScale = Vector3.Lerp(icon.localScale, Vector3.one * (active ? 1 : offScale), scaleTime * Time.deltaTime);
        nameText.text = ((VOXEL_TYPE)i).ToString();
    }
}
