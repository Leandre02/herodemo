using UnityEngine;
using UnityEngine.AI;

public class EtatAttaque : EtatMonstre
{
    private float cooldown = 1.0f;  // 1 coup/seconde (à ajuster)
    private float timer;

    public override void EntrerEtat(Zombie monstre)
    {
        timer = 0f;

        // Optionnel: stopper l'agent pour frapper sur place
        var agent = monstre.GetComponent<NavMeshAgent>();
        if (agent) agent.isStopped = true;

        // TODO si tu as un Animator :
        // monstre.GetComponent<Animator>()?.SetTrigger("Attaquer");
    }

    public override EtatMonstre ExecuterEtat(Zombie monstre)
    {
        var perso = ControleurJeu.Instance?.Personnage?.transform;
        if (!perso) return this; // sécurité

        // Si la cible est sortie de portée d'attaque -> retour en poursuite
        float d = Vector3.Distance(perso.position, monstre.transform.position);
        if (d > monstre.RayonAttaque)
        {
            var agent = monstre.GetComponent<NavMeshAgent>();
            if (agent) agent.isStopped = false;
            return new EtatPoursuite();
        }

        // Regarder la cible (rotation douce)
        Vector3 dir = perso.position - monstre.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
        {
            monstre.transform.rotation = Quaternion.RotateTowards(
                monstre.transform.rotation,
                Quaternion.LookRotation(dir),
                360f * Time.deltaTime
            );
        }

        // Cooldown d'attaque
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Debug.Log(" Zombie attaque !");
            timer = 0f;

            // TODO: appliquer des dégâts réels (animation event, hitbox, raycast, etc.)
            // Exemple si ton Personnage implémente une interface de PV :
            // perso.GetComponent<Vie>()?.PrendreDegats(valeur);
        }

        // On reste en Attaque tant que la cible est à portée
        return this;
    }

    public override void SortirEtat(Zombie monstre)
    {
        // Relancer l'agent quand on quitte l'état
        var agent = monstre.GetComponent<NavMeshAgent>();
        if (agent) agent.isStopped = false;
    }
}
