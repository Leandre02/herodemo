using UnityEngine;

/// <summary>
/// Gere le mouvement du personnage.
/// </summary>
public class MouvementPersonnage : EtatPersonnage
{
  
    public override void EntrerEtat(Personnage personnage)
    {
        base.EntrerEtat(personnage); // Appel de la m�thode de la classe parente
        var anim = personnage.GetComponent<Animator>(); // Charge l'animator
        personnage.DemarrerMarche();
    }

    public override EtatPersonnage ExecuterEtat(Personnage personnage)
    {
        // Si le joueur est au sol, a demand� un saut (via une entr�e de mouvement verticale positive), et n'est pas d�j� en train de sauter
        if (personnage.Controleur.isGrounded && personnage.EntreeMouvement.y > 0.01f && !personnage.SautEnCours)
        {
            return new SautPersonnage();
        }

        // Si le joueur n'appuie plus sur le bouton de mouvement alors retour en etat d attente
        if (personnage.EntreeMouvement.sqrMagnitude <= 0.01f)
        {
            return new AttentePersonnage();
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
        personnage.ArreterMarche();
    }
}
