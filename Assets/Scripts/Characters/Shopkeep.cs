using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeep : MonoBehaviour
{
    public IEnumerator Heal(Transform player, Dialouge dialouge)
    {
        yield return DialogueManager.Instance.ShowDialouge(dialouge);

        var playerParty = player.GetComponent<MimicParty>();
        playerParty.Mimics.ForEach(Mimic => Mimic.Heal());
        playerParty.PartyUpdated();
    }
}
