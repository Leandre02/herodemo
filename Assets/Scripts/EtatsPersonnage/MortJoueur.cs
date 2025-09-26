using UnityEngine;

/// <summary>
/// L'etat de mort de mon personnage qui met fin a la partie
/// </summary>
public class MortJoueur : EtatPersonnage
{
    /// <summary>
    /// Actions � accomplir lorsqu'un personnage entre dans l'�tat de mort.
    /// </summary>
    /// <param name="personnage">Le personnage dont on g�re le comportement</param>
    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        personnage.ArreterMarche(); // Arr�te l'animation de marche pour ne pas m�langer l'animator
        personnage.OnMortJoueur(); // D�clenche la logique de fin de partie
    }

    /// <summary>
    /// Actions � accomplir lorsqu'un personnage est dans l'�tat de mort.
    /// </summary>
    /// <param name="personnage">Le personnage dont on g�re le comportement</param>
    /// <returns>L'�tat � ex�cuter � la prochaine frame (reste sur MortJoueur)</returns>
    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {

        Debug.Log($"La Partie est termin�e !!!");
        // Reste dans l'�tat de mort, aucune transition
        return this;
    }
}
