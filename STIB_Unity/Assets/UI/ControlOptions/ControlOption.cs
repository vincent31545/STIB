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

    private bool activated = false;

    public void Activate(bool on, bool force = false) {
        if (activated != on || force) {
            cg.alpha = on ? 1.0f : 0.4f;
            StartCoroutine(Scale(icon.localScale, Vector3.one * (on ? 1 : offScale), scaleTime));
            activated = on;
        }
    }

    private IEnumerator Scale(Vector3 from, Vector3 to, float time) {
        from.z = to.z = 1;

        icon.localScale = from;
        float t = 0;
        while (t < time) {
            t += Time.deltaTime;
            icon.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }
        icon.localScale = to;
    }
}
