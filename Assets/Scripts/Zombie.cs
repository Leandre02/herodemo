using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Gere le comportement d'un zombie via des etats
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour, FrappableParArme
{
    private NavMeshAgent agent;          // le composant de navigation
    public Vector3 Destination { get; private set; } // La destination actuelle du zombie

    /// <summary>
    /// les etats possibles du monstre
    /// </summary>

    [field: SerializeField, Tooltip("Le rayon dans lequel le monstre commence à poursuivre le personnage.")]
    public float RayonPoursuite { get; private set; }

    [field: SerializeField, Tooltip("Le rayon dans lequel le monstre commence à attaque le personnage.")]
    public float RayonAttaque { get; private set; }

    [SerializeField, Tooltip("Rayon dans lequel le monstre patrouille")]
    private float rayonPatrouille;

    [SerializeField, Tooltip("Position initiale du monstre")]
    private Vector3 positionInitiale;

    #region Gestion de l'état du monstre
    /// <summary>
    /// État actuel du monstre
    /// </summary>
    private EtatMonstre etatPrecedent;

    private EtatMonstre prochainEtat;
    #endregion

    private void Awake()
    {
        // Récupère une référence sur le component
        agent = GetComponent<NavMeshAgent>();

        // On commence dans l'état de patrouille
        etatPrecedent = null;
        prochainEtat = new EtatPatrouille();

        // TODO : À DÉPLACER
        ChoisirDestination();
    }

    private void Start()
    {
        positionInitiale = transform.position;
    }

    private void Update()
    {
        // Prochain état représente l'état à exécuter
        if (etatPrecedent != prochainEtat)
        {
            prochainEtat.EntrerEtat(this);
        }
        // On n'a plus besoin de cette valeur, on fait donc la mise à jour
        etatPrecedent = prochainEtat;
        prochainEtat = prochainEtat.ExecuterEtat(this);      // Met à jour le prochain état à exécuter. 
                                                             // À partir de ce point, l'état de la frame est accessible dans etatPrecedent (voir ligne au-dessus).

        if (etatPrecedent != prochainEtat)
        {
            etatPrecedent.SortirEtat(this);
        }
    }

    /// <summary>
    /// Assigne une nouvelle destination au hasard au monstre
    /// </summary>
    public void ChoisirDestination()
    {
        /*Vector3 pointAleatoire = new Vector3(Random.value * 50.0f - 25.0f,
            transform.position.y,
            Random.value * 50.0f - 25.0f);*/
        Vector3 pointAleatoire = positionInitiale + Random.insideUnitSphere * rayonPatrouille;

        AssignerDestination(pointAleatoire);
    }

    /// <summary>
    /// Assigne une destination au monstre
    /// </summary>
    /// <param name="destination">La destination à assigner au monstre</param>
    public void AssignerDestination(Vector3 destination)
    {
        // Permet d'obtenir un point sur le nav mesh
        if (NavMesh.SamplePosition(destination, out NavMeshHit pointDeterminee, 100f, NavMesh.AllAreas))
        {
            Destination = pointDeterminee.position;
        }

        agent.destination = Destination;
    }

    /// <summary>
    /// Lorsqu'un monstre est frappé, il reçoit des dégâts.
    /// </summary>
    /// <param name="arme">L'arme qui a frappé le monstre.</param>
    public void RecevoirFrappe(Arme arme)
    {
        GetComponent<Vie>()?.PrendreDegats(arme.Degat);
    }

}


