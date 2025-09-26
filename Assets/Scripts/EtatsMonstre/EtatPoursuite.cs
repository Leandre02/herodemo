using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Gere l'état de poursuite d'un monstre
/// </summary>
public class EtatPoursuite : EtatMonstre
{

    // Le zombie entre dans l'état marche
    public override void EntrerEtat(Zombie monstre)
    {
        base.EntrerEtat(monstre);
        // Redemarre l'agent 
        var agent = monstre.GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.isStopped = false;
        }

        monstre.DemarrerMarche();
    }

    public override EtatMonstre ExecuterEtat(Zombie monstre)
    {
        Vector3 positionPersonnage = ControleurJeu.Instance.Personnage.transform.position;
        monstre.AssignerDestination(positionPersonnage);

        // Engager attaque
        float distanceAuPersonnage = Vector3.Distance(positionPersonnage, monstre.transform.position);
        if (distanceAuPersonnage < monstre.RayonAttaque)
        {
            return new EtatAttaque();
        }
        if (distanceAuPersonnage > monstre.RayonPoursuite)
        {
            return new EtatPatrouille();
        }

        return this;
    }

    // le zombie sort de l'état marche
    public override void SortirEtat(Zombie monstre)
    {
        base.SortirEtat(monstre);
        monstre.ArreterMarche();
    }
}
