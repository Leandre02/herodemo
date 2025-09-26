using System;
using UnityEngine;


/// <summary>
/// Les PV de mes personnages / zombies qui meurt lorsque points <= 0 avec l'animation chargé automatiquement depuis l'animator correspondant
/// </summary>
public class Vie : MonoBehaviour
{
    [SerializeField] private float pointsDeVieMax = 100f;
    public float PointsDeVieMax => pointsDeVieMax;
   

    public float PointsDeVie { get; private set; }

    // Event pour prévenir l'UI / autres systèmes
    public event Action<float, float> OnVieChange; // (pvActuels, pvMax)
    public event Action OnMort; // Pour la mort de mes personnages / zombie

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
        if (PointsDeVie <= 0f)
        {
            Mourir();
        }
    }

    /// <summary>
    /// Méthode pour soigner le personnage
    /// </summary>
    /// <param name="valeur"></param>
    public void Soigner(float valeur)
    {
        if (valeur <= 0f) return;
        PointsDeVie = Mathf.Min(pointsDeVieMax, PointsDeVie + valeur);
        OnVieChange?.Invoke(PointsDeVie, pointsDeVieMax);
    }


    // Methode qui appel la fonction mourrir correspondant a l'objet
    private void Mourir()
    {

        OnMort?.Invoke();

    }
}
