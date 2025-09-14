using UnityEngine;


/// <summary>
/// Etat de patrouille d'un monstre
/// </summary>
public class EtatPatrouille : EtatMonstre
{
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
}
