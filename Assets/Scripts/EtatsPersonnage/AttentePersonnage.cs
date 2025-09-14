using UnityEngine;

/// <summary>
/// Gere le comportement d'un personnage en attente
/// </summary>
public class AttentePersonnage : EtatPersonnage, IntentionAttaque
{
    bool veutAttaquer; // Indique si le personnage veut attaquer

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la méthode de la classe parente
        Debug.Log("Personnage en attente...");
        veutAttaquer = false; // Réinitialiser l'état d'attaque à l'entrée dans l'état
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // Vérifier si le personnage veut attaquer 
        if (Input.GetButtonDown("Attaquer"))
        {
            veutAttaquer = true;
        }
        if (veutAttaquer)
        {
            return new AttaquePersonnage(); // Passe à l'état d'attaque si le personnage veut attaquer
        }
        // Rester dans l'état d'attente tant que le personnage ne veut pas attaquer
        else
        {
            return this;
        }
    }

    public void OnIntentAttaquer()
    {
       veutAttaquer = true; // Le personnage veut attaquer
    }
}
