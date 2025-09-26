using Unity.VisualScripting;
using UnityEngine;

public class SautPersonnage : EtatPersonnage
{

    float timer; // Timer pour gerer la durée du saut

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la méthode de la classe parente
        timer = 0f; // Réinitialiser le timer à l'entrée dans l'état
        var anim = personnage.GetComponent<Animator>(); // Charge l'animator
        personnage.ArreterMarche(); // Arrete l'animation de marche pour pas melanger l'animator
        personnage.DemarrerSaut(); 

    }


    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        timer += Time.deltaTime; // Incrémenter le timer avec le temps écoulé depuis la dernière frame
        if (timer >= personnage.TempsSaut && personnage.Controleur.isGrounded)
        {
            if (personnage.EntreeMouvement.sqrMagnitude > 0.01f)
            {
                return new MouvementPersonnage();
            }
            else
            {
                return new AttentePersonnage();
            }
        }
        else
        {
            // Sinon, rester dans l'état de saut
            return this;
        }

    }

    public override void SortirEtat(Personnage personnage)
    {
        base.SortirEtat(personnage);
        // Arrêter l'animation d'attaque
        var anim = personnage.GetComponent<Animator>();
        personnage.ArreterSaut();
        

    }
}
