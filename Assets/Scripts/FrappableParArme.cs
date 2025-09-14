using UnityEngine;


/// <summary>
/// Indique qu'un objet peut �tre frapp� par une arme
/// </summary>
public interface FrappableParArme
{
    /// <summary>
    /// Traite la r�ception d'une frappe.
    /// </summary>
    /// <param name="arme">L'arme qui a frapp� l'objet</param>
    public void RecevoirFrappe(Arme arme);
}
