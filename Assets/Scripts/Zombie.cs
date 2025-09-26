using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Ma classe zombie qui gere les mouvements de mon personnage zombie
/// Assigne un etat --> cet etat demande la designation de la cible au controleur
/// La vitesse du deplacement et l'acceleration sont decidés par le NavMesh agent
/// </summary>
[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class Zombie : MonoBehaviour, FrappableParArme
{
    private NavMeshAgent agent; // L'IA responsable des deplacements intelligents de mon personnage
    private Animator anim; // La propriete de l'animation assigné a mon zombie

    private static readonly int HashMarcher = Animator.StringToHash("Marche"); // Mon animation de marche
    private static readonly int HashAttaquer = Animator.StringToHash("Attaque"); // Mon animation d'attaque
    private static readonly int HashMourir = Animator.StringToHash("Mourir"); // Mon animation de mort



    [Header("Attaque")]
    [SerializeField] private Arme arme; // La propriete de l'arme de mon zombie
    [SerializeField, Tooltip("Temps entre 2 coups (s)")] private float tempsAttaque = 0.8f; // Le temps d'attente entre chaque coups

    // Accesseur de mon temps d'attaque pour mon EtatAttaque
    public float TempsAttaque => tempsAttaque;

    private bool attaqueEnCours; // Un trigger pour determiner si une attaque est en cours

    public Vector3 Destination { get; private set; } // Le vecteur qui definit la destination des mouvements de mon personnage

    [field: SerializeField] public float RayonPoursuite { get; private set; } = 10f; // le rayon de la poursuite
    [field: SerializeField] public float RayonAttaque { get; private set; } = 2f; // le rayon de l'attaque

    [SerializeField] private float rayonPatrouille = 12f; // le rayon de la patrouille
    private Vector3 positionInitiale; // Le vecteur de position initial de mon personnage

    private EtatMonstre etatPrecedent; // L'etat precedent de mon personnage 
    private EtatMonstre prochainEtat; // Le prochain etat de mon zombie

    // Ajout du champ booléen pour indiquer si le zombie est mort
    private bool estMort = false;
    public bool EstMort => estMort;

    /// <summary>
    /// Initialise le component NavMesh pour initier la position d'origine de mon personnage et determiner sa destination
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // Pre-charge l'IA de mon zombie
        anim = GetComponent<Animator>(); // Pre-charge les animations correspondantes

        positionInitiale = transform.position;     //  Le point de depart de mon zombie
        ChoisirDestination();                      // La destination de mon zombie

        agent.stoppingDistance = Mathf.Max(0.1f, RayonAttaque * 0.9f);

        etatPrecedent = null;
        prochainEtat = new EtatPatrouille(); // On initialise à l'etat patrouille par defaut


        // Branchement au changement d'etat vers Mourir
        var vie = GetComponent<Vie>();
        if (vie != null) vie.OnMort += () => { prochainEtat = new EtatMort(); };

    }

    /// <summary>
    /// La methode qui met a jour l'etat de mon zombie a chaque frame
    /// </summary>
    private void Update()
    {

        // determine si le zombie a atteint sa destination en etat patrouille si oui choisi une nouvelle destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && prochainEtat is EtatPatrouille)
            ChoisirDestination();


        // 1) si changement -> on sort de l’ancien
        if (etatPrecedent != null && etatPrecedent != prochainEtat)
            etatPrecedent.SortirEtat(this);

        // 2) si changement -> on entre dans le nouveau
        if (etatPrecedent != prochainEtat)
            prochainEtat.EntrerEtat(this);

        // 3) on exécute l’état courant
        etatPrecedent = prochainEtat;
        prochainEtat = prochainEtat.ExecuterEtat(this);

    }

    // ===== Hooks de mes differents Etats marche =====
    public void DemarrerMarche() => anim?.SetBool(HashMarcher, true);
    public void ArreterMarche() => anim?.SetBool(HashMarcher, false);



    // Methode pour mon état Attaque
    public void DemarrerAttaque()
    {
        if (attaqueEnCours || arme == null) return;

        attaqueEnCours = true;
        anim?.SetTrigger(HashAttaquer);
        arme.CommencerAttaque();
        Invoke(nameof(FinirAttaque), tempsAttaque);
    }

    public void FinirAttaque()
    {
        attaqueEnCours = false;
    }

    public void ChoisirDestination()
    {
        // point aléatoire sur un disque au sol autour de la positionInitiale
        Vector2 disque = Random.insideUnitCircle * rayonPatrouille;
        Vector3 pointAleatoire = positionInitiale + new Vector3(disque.x, 0f, disque.y);
        AssignerDestination(pointAleatoire);
    }

    public void AssignerDestination(Vector3 destination)
    {
        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 100f, NavMesh.AllAreas))
        {
            Destination = hit.position;
            agent.SetDestination(Destination);
        }
    }

    public void RecevoirFrappe(Arme arme) => GetComponent<Vie>()?.PrendreDegats(arme.Degat);

    // Méthode à ajouter dans la classe Zombie
    public void Mourir()
    {
        if (estMort) return; // Empêche d'appeler plusieurs fois

        estMort = true;

        // Déclenche l'animation de mort si l'Animator est présent
        if (anim == null)
            anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger(HashMourir);
    }
}
