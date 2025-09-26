using UnityEngine;

/// <summary>
/// Gere le comportement d'un personnage en attente
/// </summary>
public class AttentePersonnage : EtatPersonnage
{

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la méthode de la classe parente
        var anim = personnage.GetComponent<Animator>(); // Charge l'animator
        personnage.ArreterMarche();
       
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // Si le joueur est au sol, a demandé un saut (via une entrée de mouvement verticale positive), et n'est pas déjà en train de sauter
        if (personnage.Controleur.isGrounded && personnage.EntreeMouvement.y > 0.01f && !personnage.SautEnCours)
        {
            return new SautPersonnage();
        }


        // Si le joueur appuie sur la touche de mouvement alors instantie un nouveau mouvement de personnage
        if (personnage.EntreeMouvement.sqrMagnitude > 0.01f)
        {
            return new MouvementPersonnage();
        }
        if (personnage.AttaqueEnCours)
        {
            return new AttaquePersonnage();
        }
        return this;
       
    }

    public override void SortirEtat(Personnage personnage)
    {
        base.SortirEtat(personnage);
        personnage.DemarrerMarche();
    }
}
