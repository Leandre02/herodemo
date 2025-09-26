using UnityEngine;

/// <summary>
/// Gere le comportement d'un personnage en attente
/// </summary>
public class AttentePersonnage : EtatPersonnage
{

    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        var anim = personnage.GetComponent<Animator>(); // Charge l'animator
        personnage.ArreterMarche();
       
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // Si le joueur est au sol, a demand� un saut (via une entr�e de mouvement verticale positive), et n'est pas d�j� en train de sauter
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
