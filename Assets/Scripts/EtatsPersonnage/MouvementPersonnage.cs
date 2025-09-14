using UnityEngine;

/// <summary>
/// Gere le mouvement du personnage.
/// </summary>
public class MouvementPersonnage : EtatPersonnage, IntentionAttaque
{
    bool veutAttaquer; // Indique si le personnage veut attaquer
    public void OnIntentAttaquer()
    {
        veutAttaquer = true; // Le personnage veut attaquer
    }
    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la méthode de la classe parente
        Debug.Log("Personnage en mouvement...");
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // Vérifier si le personnage veut attaquer 
        if (veutAttaquer)
        {
            return new AttaquePersonnage(); // Passe à l'état d'attaque si le personnage veut attaquer
        }
        // Rester dans l'état de mouvement tant que le personnage ne veut pas attaquer
        else
        {
            return this;
        }
    }
}
