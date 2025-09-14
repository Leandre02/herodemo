using UnityEngine;


/// <summary>
/// Le gestionnaire principal du jeu.
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    private static ControleurJeu instance; // Instance unique de la classe
    public static ControleurJeu Instance => instance; // Acc�s public � l'instance

    // R�f�rence au personnage principal du jeu
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
