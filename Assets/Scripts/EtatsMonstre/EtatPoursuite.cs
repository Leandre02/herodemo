using UnityEngine;


/// <summary>
/// Gere l'état de poursuite d'un monstre
/// </summary>
public class EtatPoursuite : EtatMonstre
{
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
}
