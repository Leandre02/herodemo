using UnityEngine;
using System;

public class Vie : MonoBehaviour
{
    [SerializeField] private float pointsDeVieMax = 100f;
    public float PointsDeVieMax => pointsDeVieMax;

    public float PointsDeVie { get; private set; }

    // Event pour prévenir l'UI / autres systèmes
    public event Action<float, float> OnVieChange; // (pvActuels, pvMax)

    void Awake()
    {
        PointsDeVie = pointsDeVieMax;
        OnVieChange?.Invoke(PointsDeVie, pointsDeVieMax);
    }

    public void PrendreDegats(float degats)
    {
        if (degats <= 0f) return;
        PointsDeVie = Mathf.Max(0f, PointsDeVie - degats);
        OnVieChange?.Invoke(PointsDeVie, pointsDeVieMax);
        if (PointsDeVie <= 0f) Mourir();
    }

    public void Soigner(float valeur)
    {
        if (valeur <= 0f) return;
        PointsDeVie = Mathf.Min(pointsDeVieMax, PointsDeVie + valeur);
        OnVieChange?.Invoke(PointsDeVie, pointsDeVieMax);
    }

    private void Mourir()
    {
        // TODO: anim mort, disable IA, etc.
        // Destroy(gameObject); // si tu veux
    }
}
