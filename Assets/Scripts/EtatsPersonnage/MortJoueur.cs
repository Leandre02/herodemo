using UnityEngine;

/// <summary>
/// L'etat de mort de mon personnage qui met fin a la partie
/// </summary>
public class MortJoueur : EtatPersonnage
{
    /// <summary>
    /// Actions à accomplir lorsqu'un personnage entre dans l'état de mort.
    /// </summary>
    /// <param name="personnage">Le personnage dont on gère le comportement</param>
    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la méthode de la classe parente
        personnage.ArreterMarche(); // Arrête l'animation de marche pour ne pas mélanger l'animator
        personnage.OnMortJoueur(); // Déclenche la logique de fin de partie
    }

    /// <summary>
    /// Actions à accomplir lorsqu'un personnage est dans l'état de mort.
    /// </summary>
    /// <param name="personnage">Le personnage dont on gère le comportement</param>
    /// <returns>L'état à exécuter à la prochaine frame (reste sur MortJoueur)</returns>
    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {

        Debug.Log($"La Partie est terminée !!!");
        // Reste dans l'état de mort, aucune transition
        return this;
    }
}
