using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// État de l'attaque de mon zombie
/// </summary>
public class EtatAttaque : EtatMonstre
{
    public float timerAttaque; // Le timer entre chaque attaque

    public override void EntrerEtat(Zombie monstre)
    {
        base.EntrerEtat(monstre);
        // Arrêter le déplacement de l'IA
        var agent = monstre.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
        }
        // Démarrer l'animation d'attaque
        var anim = monstre.GetComponent<Animator>();
        monstre.ArreterMarche(); // Arrete l'animation de marche pour pas melanger l'animator
        timerAttaque = 0f; 
        monstre.DemarrerAttaque();
        
    }

    /// <summary>
    /// Methode d execution de l attaque de mon zombie
    /// Mesure la distance par rapport au joueur et non a la destination pour eviter des alternances trop rapide entre poursuite et attaque
    /// </summary>
    /// <param name="monstre"></param>
    /// <returns></returns>
    public override EtatMonstre ExecuterEtat(Zombie monstre)
    {
        // Position actuelle du joueur
        var posCible = ControleurJeu.Instance.Personnage.transform.position;

        // Garder le zombie orienté vers la cible (à plat sur le plan XZ)
        Vector3 dir = (posCible - monstre.transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            monstre.transform.rotation = Quaternion.Slerp(
                monstre.transform.rotation,
                Quaternion.LookRotation(dir),
                10f * Time.deltaTime
            );

        // Si la cible s’éloigne, on repasse en poursuite
        float distance = Vector3.Distance(monstre.transform.position, posCible);
        if (distance > monstre.RayonAttaque * 1.05f)
            return new EtatPoursuite();

        // Timer d’attaque
        timerAttaque += Time.deltaTime;
        if (timerAttaque >= monstre.TempsAttaque)
        {
            monstre.DemarrerAttaque();
            timerAttaque = 0f;
        }
        return this;
    }


    public override void SortirEtat(Zombie monstre)
    {
        base.SortirEtat(monstre);
        // Arrêter l'animation d'attaque
        var anim = monstre.GetComponent<Animator>();
        if (anim != null)
        {
            anim.ResetTrigger("Attaque");
        }
        // Arrêter l'attaque
        monstre.FinirAttaque();

        // Redemarre l'agent 
        var agent = monstre.GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.isStopped = false;
        }
    }
}
