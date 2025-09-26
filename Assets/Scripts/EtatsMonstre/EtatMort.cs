using UnityEngine;
using UnityEngine.AI;

public class EtatMort : EtatMonstre
{
    public override void EntrerEtat(Zombie monstre)
    {
        base.EntrerEtat(monstre);
        // Arr�ter le mouvement du zombie
        var agent = monstre.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
        }

        // Lancer l'animation de mort
        var anim = monstre.GetComponent<Animator>();
        monstre.ArreterMarche();
        monstre.Mourir();

        // D�truire le zombie apr�s 2 secondes pour laisser l'animation se jouer
        Object.Destroy(monstre.gameObject, 2f);
    }

    public override EtatMonstre ExecuterEtat(Zombie monstre)
    {
        // Rester dans l'�tat de mort
        return this;
    }
   
}
