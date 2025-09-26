using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Etat de patrouille d'un monstre
/// </summary>
public class EtatPatrouille : EtatMonstre
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


    // Le zombie execute sa patrouille et si le character du controleur se trouve a proximité le poursuit direct
    public override EtatMonstre ExecuterEtat(Zombie monstre)
    {
        //Destination atteinte
        if (Vector3.Distance(monstre.transform.position, monstre.Destination) < 1.0f)
        {
            monstre.ChoisirDestination();
        }

        // Engager poursuite
        if (Vector3.Distance(ControleurJeu.Instance.Personnage.transform.position,
            monstre.transform.position) < monstre.RayonPoursuite)
        {
            return new EtatPoursuite();
        }

        // Continuer la patrouille
        return this;
    }

    // le zombie sort de l'état marche
    public override void SortirEtat(Zombie monstre)
    {
        base.SortirEtat(monstre);
        monstre.ArreterMarche();
    }
}
