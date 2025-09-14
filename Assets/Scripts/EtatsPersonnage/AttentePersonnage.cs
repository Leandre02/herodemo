using UnityEngine;

/// <summary>
/// Gere le comportement d'un personnage en attente
/// </summary>
public class AttentePersonnage : EtatPersonnage, IntentionAttaque
{
    bool veutAttaquer; // Indique si le personnage veut attaquer

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        Debug.Log("Personnage en attente...");
        veutAttaquer = false; // R�initialiser l'�tat d'attaque � l'entr�e dans l'�tat
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // V�rifier si le personnage veut attaquer 
        if (Input.GetButtonDown("Attaquer"))
        {
            veutAttaquer = true;
        }
        if (veutAttaquer)
        {
            return new AttaquePersonnage(); // Passe � l'�tat d'attaque si le personnage veut attaquer
        }
        // Rester dans l'�tat d'attente tant que le personnage ne veut pas attaquer
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
