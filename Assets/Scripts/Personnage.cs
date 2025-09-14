using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Personnage : MonoBehaviour, FrappableParArme
{
    [SerializeField] private Arme arme;
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

    /// <summary>
    /// Méthode appelée lors de l'initialisation du script
    /// Cherche et assigne les références nécessaires au personnage
    /// @param controleur : référence au CharacterController pour gérer les collisions et le mouvement
    /// </summary>
    void Awake()
    {
        controleur = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); // Pour gérer les animations
    }

    // Input System: mappe une action "Attaque" sur clavier/souris ou manette
    public void OnAttaque(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (arme != null)
        {
            arme.CommencerAttaque();
            anim?.SetTrigger("Attaquer"); // Pour gerer les attaques du personnage
        }
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

    }

    /// <summary>
    /// Permet d'encaisser des degats
    /// </summary>
    /// <param name="a"></param>
    public void RecevoirFrappe(Arme a)
    {
        GetComponent<Vie>()?.PrendreDegats(arme.Degat);
    }
}
