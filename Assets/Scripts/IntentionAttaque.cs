using UnityEngine;

/// <summary>
/// Interface indiquant qu'un objet peut recevoir une intention d'attaque.
/// </summary>
public interface IntentionAttaque
{
    /// <summary>
    /// La m�thode appel�e lorsqu'une intention d'attaque est re�ue.
    /// </summary>
    void OnIntentAttaquer();
}
