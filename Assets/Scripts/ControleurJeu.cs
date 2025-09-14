using UnityEngine;


/// <summary>
/// Le gestionnaire principal du jeu.
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    private static ControleurJeu instance; // Instance unique de la classe
    public static ControleurJeu Instance => instance; // Accès public à l'instance

    // Référence au personnage principal du jeu
    [field: SerializeField] public Personnage Personnage { get; private set; }

    /// <summary>
    /// Fonction permettant de s'assurer qu'il n'y a qu'une seule instance de ControleurJeu (singleton).
    /// </summary>
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


}
