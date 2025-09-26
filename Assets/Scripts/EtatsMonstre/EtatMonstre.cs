using UnityEngine;

/// <summary>
/// Gere le comportement d'un monstre
/// Maquette de base de tous mes états
/// </summary>
public abstract class EtatMonstre
{
    /// <summary>
    /// Actions a accomplir lorsqu'un monstre entre dans l'etat
    /// </summary>
    /// <param name="monstre">Le monstre dont on gere le comportement</param>
    public virtual void EntrerEtat(Zombie monstre)
    {

        Debug.Log($"Monstre entre dans l'etat {this.GetType().Name}");
    }

    /// <summary>
    /// Actions a accomplir lorsqu'un monstre est dans cet etat. Comprends aussi la logique de 
    /// passage aux autres etats.
    /// </summary>
    /// <param name="monstre">Le monstre dont on gere le comportement</param>
    /// <returns>L'etat a executer a la prochaine frame.</returns>
    public abstract EtatMonstre ExecuterEtat(Zombie monstre);

    /// <summary>
    /// Actions a accomplir lorsqu'un monstre sort de l'etat
    /// </summary>
    /// <param name="monstre">Le monstre dont on gere le comportement</param>
    public virtual void SortirEtat(Zombie monstre)
    {
        Debug.Log($"Monstre sort de l'etat {this.GetType().Name}");
    }
}