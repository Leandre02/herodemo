using UnityEngine;

/// <summary>
/// Gere le comportement d'un personnage en attaque
/// </summary>
public class AttaquePersonnage : EtatPersonnage
{
    float timer; // Timer pour g�rer la dur�e de l'attaque
    float dureeAttaque = 0.5f; // Dur�e de l'attaque en secondes

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        timer = 0f; // R�initialiser le timer � l'entr�e dans l'�tat
        Debug.Log("Personnage attaque !");
        personnage.GetComponent<Animator>()?.SetTrigger("Attaquer"); // D�clenche l'animation d'attaque si un Animator est pr�sent
    }


    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        timer += Time.deltaTime; // Incr�menter le timer avec le temps �coul� depuis la derni�re frame
        if (timer >= dureeAttaque)
        {
            // Si la dur�e de l'attaque est �coul�e, retourner � l'�tat d'attente
            return new AttentePersonnage();
        }
        else
        {
            // Sinon, rester dans l'�tat d'attaque
            return this;
        }
            
    }
}
