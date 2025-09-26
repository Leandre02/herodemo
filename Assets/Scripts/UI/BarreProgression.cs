using UnityEngine;
using UnityEngine.UI;

public class BarreProgression : MonoBehaviour
{
    [SerializeField] private Image barre;   

    public void SetProgression(float t)
    {
        if (!barre) return;
        barre.fillAmount = Mathf.Clamp01(t);
    }
}
