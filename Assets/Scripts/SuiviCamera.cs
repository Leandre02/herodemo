using UnityEngine;


/// <summary>
/// Represente une caméra qui suit un personnage avec un léger décalage et un lissage
/// </summary>
public class SuiviCamera : MonoBehaviour
{
    public Transform cible;                 // le personnage à suivre
    public Vector3 decalage = new Vector3(0, 2, -5); // position relative de la caméra
    public float vitesseLissage = 5f;       // vitesse d'interpolation

    void LateUpdate()
    {
        if (!cible) return;

        // Position désirée = position de la cible + décalage
        Vector3 positionVoulue = cible.position + decalage;

        // Lissage entre la position actuelle et la position voulue
        Vector3 positionLisse = Vector3.Lerp(transform.position, positionVoulue, vitesseLissage * Time.deltaTime);

        transform.position = positionLisse;

        // Regarder la cible (avec un petit décalage en hauteur)
        transform.LookAt(cible.position + Vector3.up * 1.5f);
    }
}
