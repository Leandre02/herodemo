using UnityEngine;


/// <summary>
/// Indique qu'un objet peut être frappé par une arme
/// </summary>
public interface FrappableParArme
{
    /// <summary>
    /// Traite la réception d'une frappe.
    /// </summary>
    /// <param name="arme">L'arme qui a frappé l'objet</param>
    public void RecevoirFrappe(Arme arme);
}
