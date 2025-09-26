using Unity.VisualScripting;
using UnityEngine;

public class SautPersonnage : EtatPersonnage
{

    float timer; // Timer pour gerer la dur�e du saut

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        timer = 0f; // R�initialiser le timer � l'entr�e dans l'�tat
        var anim = personnage.GetComponent<Animator>(); // Charge l'animator
        personnage.ArreterMarche(); // Arrete l'animation de marche pour pas melanger l'animator
        personnage.DemarrerSaut(); 

    }


    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        timer += Time.deltaTime; // Incr�menter le timer avec le temps �coul� depuis la derni�re frame
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
            // Sinon, rester dans l'�tat de saut
            return this;
        }

    }

    public override void SortirEtat(Personnage personnage)
    {
        base.SortirEtat(personnage);
        // Arr�ter l'animation d'attaque
        var anim = personnage.GetComponent<Animator>();
        personnage.ArreterSaut();
        

    }
}
