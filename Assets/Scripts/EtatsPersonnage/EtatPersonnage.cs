using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gere le comportement d'un personnage
/// </summary>
public abstract class EtatPersonnage
{

///
    /// <summary>
    /// Actions a accomplir lorsqu'un personnage entre dans l'etat
    /// </summary>
    /// <param name="personnage">Le personnage dont on gere le comportement</param>
    public virtual void EntrerEtat(Personnage personnage)
    {
        Debug.Log($"Personnage entre dans l'etat {this.GetType().Name}");
    }
    /// <summary>
    /// Actions a accomplir lorsqu'un personnage est dans cet etat. Comprends aussi la logique de 
    /// passage aux autres etats.
    /// </summary>
    /// <param name="personnage">Le personnage dont on gere le comportement</param>
    /// <returns>L'etat a executer a la prochaine frame.</returns>
    public abstract EtatPersonnage ExecuterEtat(Personnage personnage);
    /// <summary>
    /// Actions a accomplir lorsqu'un personnage sort de l'etat
    /// </summary>
    /// <param name="personnage">Le personnage dont on gere le comportement</param>
    public virtual void SortirEtat(Personnage personnage)
    {
        Debug.Log($"Personnage sort de l'etat {this.GetType().Name}");
    }

}
