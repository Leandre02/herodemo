using UnityEngine;

/// <summary>
/// Gere le comportement d'un personnage en attaque
/// </summary>
public class AttaquePersonnage : EtatPersonnage
{
    float timer; // Timer pour gérer la durée de l'attaque
    float dureeAttaque = 0.5f; // Durée de l'attaque en secondes

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la méthode de la classe parente
        timer = 0f; // Réinitialiser le timer à l'entrée dans l'état
        Debug.Log("Personnage attaque !");
        personnage.GetComponent<Animator>()?.SetTrigger("Attaquer"); // Déclenche l'animation d'attaque si un Animator est présent
    }


    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        timer += Time.deltaTime; // Incrémenter le timer avec le temps écoulé depuis la dernière frame
        if (timer >= dureeAttaque)
        {
            // Si la durée de l'attaque est écoulée, retourner à l'état d'attente
            return new AttentePersonnage();
        }
        else
        {
            // Sinon, rester dans l'état d'attaque
            return this;
        }
            
    }
}
