using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] // CharacterController qui gere les collisions et le mouvement du personnage
public class Personnage : MonoBehaviour, FrappableParArme
{
    [SerializeField] private Arme arme; // Référence à l'arme du personnage
    private Animator anim; // Référence à l'Animator pour gérer les animations

    /// <summary>
    /// gestion du mouvement du personnage
    /// @param cameraJeu : référence à la caméra du jeu pour orienter le mouvement
    /// @param controleur : référence au CharacterController pour gérer les collisions et le mouvement
    /// </summary>
    [Header("ReferenceMouvement")]
    [SerializeField] private Transform cameraJeu; // Référence à la caméra du jeu
    private CharacterController controleur; // Référence au controleur de mon personnage

    // Propriétés pour accéder aux références de mouvement
    public Transform CameraJeu { get => cameraJeu; set => cameraJeu = value; }
    public CharacterController Controleur { get => controleur; set => controleur = value; }

    private static readonly int HashMarcher = Animator.StringToHash("Marche"); // Mon animation de marche
    private static readonly int HashAttaquer = Animator.StringToHash("Attaque"); // Mon animation d'attaque
    private static readonly int HashSaut = Animator.StringToHash("Saut"); // Animation pour le saut de mon personnage
    static readonly int HashMourir = Animator.StringToHash("Mourir"); // Animation de mort de mon personnage




    [SerializeField, Tooltip("Temps entre 2 coups (s)")] private float tempsAttaque = 0.8f; // Le temps d'attente entre chaque coups
    [SerializeField, Tooltip("Temps entre 2 saut (s)")] private float tempsSaut = 0.1f; // Le temps d'attente entre chaque sauts

    [SerializeField] float hauteurSaut = 1.6f; // La hauteur du saut

    // Accesseur de mon temps d'attaque pour mon AttaquePersonnage
    public float TempsAttaque => tempsAttaque;
    public float TempsSaut => tempsSaut;

    private bool attaqueEnCours; // Un trigger pour determiner si une attaque est en cours
    private bool sautEnCours;
    public bool AttaqueEnCours => attaqueEnCours; // Accesseur
    public bool SautEnCours => sautEnCours;
    public bool estMort;

    public bool EstMort => estMort;

    /// <summary>
    /// Paramètres de mouvement du personnage
    /// @param vitesseMarche : vitesse de marche du personnage
    /// @param vitesseRotation : vitesse de rotation du personnage
    /// @param gravite : force de gravité appliquée au personnage
    /// </summary>
    [Header("ParametresMouvement")]
    [SerializeField] private float vitesseMarche = 2.0f; // Vitesse de marche du personnage
    [SerializeField] private float vitesseRotation = 12f; // Vitesse de rotation du personnage
    private float gravite = 9.81f; // Force de gravité appliquée au personnage

    // Propriétés pour accéder aux paramètres de mouvement
    public float VitesseMarche { get => vitesseMarche; set => vitesseMarche = value; }
    public float VitesseRotation { get => vitesseRotation; set => vitesseRotation = value; }
    public float Gravite { get => gravite; set => gravite = value; }

    Vector3 vitesseVerticale; // Vitesse verticale du personnage (pour la gravité)
    Vector2 entreeMouvement; // Entrée de mouvement du joueur (via le Input System)

    public Vector2 EntreeMouvement => entreeMouvement; // Accesseur de mon vecteur pour mes differents etats

    private EtatPersonnage etatPrecedent; // L'état precedent de mon personnage
    private EtatPersonnage prochainEtat; // Le prochain etat de mon hero



    /// <summary>
    /// Méthode appelée lors de l'initialisation du script
    /// Cherche et assigne les références nécessaires au personnage
    /// @param controleur : référence au CharacterController pour gérer les collisions et le mouvement
    /// </summary>
    void Awake()
    {
        controleur = GetComponent<CharacterController>(); // Pre-charge le controlleur de mon perso
        if (!arme) arme = GetComponentInChildren<Arme>(true); // Assigne une arme a mon perso si pas assigné 
        anim = GetComponent<Animator>(); // Pour gérer les animations

        etatPrecedent = null;
        prochainEtat = new AttentePersonnage(); // On assigne par defaut l'etat attente a mon perso

        var vie = GetComponent<Vie>();
        if (vie != null)
        {
            vie.OnMort += OnMortJoueur; // Branchement au changement d'etat vers Mort
        }
    }

    public void DemarrerAttaque()
    {
        if (arme != null && !attaqueEnCours)
        {
            attaqueEnCours = true;
            anim?.SetTrigger(HashAttaquer);
            arme.CommencerAttaque();
            Invoke(nameof(FinirAttaque), tempsAttaque);
        }

    }


    // Input System: mappe une action "Attaque" sur clavier/souris ou manette
    public void OnAttaque(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        DemarrerAttaque();

    }

    public void FinirAttaque()
    {
        attaqueEnCours = false;
    }

    /// <summary>
    /// Méthode appelée à chaque frame pour mettre à jour le mouvement du personnage
    /// </summary>
    void Update()
    {

        // Lecture des entrées de mouvement
        float x = 0f, y = 0f;

        // Support pour le Input System, permet de gérer les entrées clavier
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        }

        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            DemarrerAttaque();
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DemarrerSaut();
        }



        entreeMouvement = new Vector2(x, y); // Récupération des entrées de mouvement
        entreeMouvement = Vector2.ClampMagnitude(entreeMouvement, 1f); // Limiter la magnitude à 1

        // Direction de mouvement en fonction de la caméra
        Vector3 avantCam = Vector3.forward;
        Vector3 droiteCam = Vector3.right;

        // Si la caméra est assignée, calculer les directions avant et droite par rapport à la caméra
        if (cameraJeu)
        {
            avantCam = Vector3.ProjectOnPlane(cameraJeu.forward, Vector3.up).normalized; // Avant de la caméra projeté sur le plan horizontal
            droiteCam = Vector3.ProjectOnPlane(cameraJeu.right, Vector3.up).normalized; // Droite de la caméra projeté sur le plan horizontal
        }

        Vector3 directionMouvement = avantCam * entreeMouvement.y + droiteCam * entreeMouvement.x; // Calcul de la direction de mouvement
        directionMouvement *= vitesseMarche; // Appliquer la vitesse de marche


        // Gestion de la rotation du personnage
        if (directionMouvement.sqrMagnitude > 0.0001f) // Vérifier si le personnage est en mouvement
        {
            // Rotation du personnage vers la direction de mouvement
            Quaternion rotationVoulue = Quaternion.LookRotation(directionMouvement);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationVoulue, vitesseRotation * Time.deltaTime);

        }


        // Gestion de la gravité du personnage lors des deplacements
        if (controleur.isGrounded) // Si le personnage est au sol
        {
            vitesseVerticale.y = -gravite * Time.deltaTime; // Appliquer une petite force vers le bas pour rester au sol
        }
        else // Si le personnage est en l'air
        {
            vitesseVerticale.y -= gravite * Time.deltaTime; // Appliquer la gravité
        }

        Vector3 deplacementTotal = directionMouvement + vitesseVerticale;

        // Appliquer le déplacement au CharacterController
        controleur.Move(deplacementTotal * Time.deltaTime);

        // 1) si changement -> on sort de l’ancien
        if (etatPrecedent != null && etatPrecedent != prochainEtat)
        {
            etatPrecedent.SortirEtat(this);
        }

        // 2) si changement -> on entre dans le nouveau
        if (etatPrecedent != prochainEtat)
        {
            prochainEtat.EntrerEtat(this);
        }

        // 3) on exécute l’état courant
        etatPrecedent = prochainEtat;
        prochainEtat = prochainEtat.ExecuterEtat(this);

        // 
        if (controleur.isGrounded)
        {
            if (vitesseVerticale.y < 0f)
            {
                vitesseVerticale.y = -2f;
            }
            else
            {
                vitesseVerticale.y -= gravite * Time.deltaTime; // Appliquer la gravité
            }
        }


    }

    // ===== Hooks de mes differents Etats marche =====
    public void DemarrerMarche() => anim?.SetBool(HashMarcher, true);
    public void ArreterMarche() => anim?.SetBool(HashMarcher, false);


    // ===== Hooks de mes differents Etats saut =====
    public void DemarrerSaut()
    {
        if (sautEnCours) return;
        
            sautEnCours = true;
            anim?.SetBool(HashSaut, true);
        vitesseVerticale.y = Mathf.Sqrt(2f * gravite * hauteurSaut); // Utilise la gravité pour effectuer le saut
        Invoke(nameof(ArreterSaut), tempsSaut);

    }
    public void ArreterSaut()
    {
        sautEnCours = false;
        
    }

    /// <summary>
    /// Permet d'encaisser des degats
    /// </summary>
    /// <param name="arme"></param>
    public void RecevoirFrappe(Arme arme)
    {
        GetComponent<Vie>()?.PrendreDegats(arme.Degat);
    }

    /// <summary>
    /// Méthode appelée lors de la mort du joueur.
    /// Déclenche l'animation de mort et désactive les contrôles du personnage.
    /// </summary>
    public void OnMortJoueur()
    {
        if (estMort) return;
        estMort = true;

        anim?.SetTrigger(HashMourir);

        // Bloque le gameplay
        this.enabled = false;
        if (controleur != null)
            controleur.enabled = false;
    }
}