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
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        Debug.Log("Personnage en mouvement...");
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // V�rifier si le personnage veut attaquer 
        if (veutAttaquer)
        {
            return new AttaquePersonnage(); // Passe � l'�tat d'attaque si le personnage veut attaquer
        }
        // Rester dans l'�tat de mouvement tant que le personnage ne veut pas attaquer
        else
        {
            return this;
        }
    }
}
