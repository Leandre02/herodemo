using UnityEngine;

public class CalculVie : MonoBehaviour
{
    [SerializeField] private Vie cible;
    [SerializeField] private BarreProgression ui;

    void Awake()
    {
        if (!cible) cible = GetComponentInParent<Vie>();
        if (!ui) ui = GetComponentInChildren<BarreProgression>();
    }

    void OnEnable()
    {
        if (!cible || !ui) return;
        ui.SetProgression(cible.PointsDeVie / cible.PointsDeVieMax);        // l' init
        cible.OnVieChange += OnVieChange;                                   // l'event
    }

    void OnDisable()
    {
        if (cible != null) cible.OnVieChange -= OnVieChange;
    }

    void OnVieChange(float pv, float max) => ui.SetProgression(pv / max);
}
