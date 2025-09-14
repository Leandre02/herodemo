using System.Collections.Generic;    // permet d'utiliser les listes 
using UnityEngine;

/// <summary>
/// Represente une arme dans le jeu
/// </summary>
public class Arme : MonoBehaviour
{
    [SerializeField, Tooltip("Dégâts infligés par l'arme.")]
    private float degat;

    /// <summary>
    /// Dégâts infligés par l'arme
    /// </summary>
    public float Degat => degat;

    [SerializeField, Tooltip("Nom de l'arme.")]
    private string nom;

    /// <summary>
    /// Contient la liste des cibles déjà frappés au cours d'une attaque
    /// </summary>
    private HashSet<FrappableParArme> objetsFrappes;

    private void Awake()
    {
        objetsFrappes = new();
    }

    /// <summary>
    /// Commence une nouvelle attaque avec l'arme
    /// </summary>
    public void CommencerAttaque()
    {
        objetsFrappes.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        // Si l'objet peut être frappé par une arme
        if (other.gameObject.TryGetComponent(out FrappableParArme objetFrappe)
            && !objetsFrappes.Contains(objetFrappe))
        {
            objetFrappe.RecevoirFrappe(this);
            objetsFrappes.Add(objetFrappe);
        }
    }
}
