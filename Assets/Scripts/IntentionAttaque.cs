using UnityEngine;

/// <summary>
/// Interface indiquant qu'un objet peut recevoir une intention d'attaque.
/// </summary>
public interface IntentionAttaque
{
    /// <summary>
    /// La méthode appelée lorsqu'une intention d'attaque est reçue.
    /// </summary>
    void OnIntentAttaquer();
}
