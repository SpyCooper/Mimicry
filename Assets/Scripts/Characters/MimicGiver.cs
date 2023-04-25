using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimicGiver : MonoBehaviour, ISavable
{
    [SerializeField] List<Mimic> mimics;
    [SerializeField] Dialouge dialogue;

    [SerializeField] GiftScreen giftScreen;

    bool used = false;

    PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        giftScreen.Init();
    }

    public IEnumerator GiveMimic(PlayerController player)
    {
        Debug.Log("Inside Mimic giver");
        yield return DialogueManager.Instance.ShowDialouge(dialogue);
        giftScreen.gameObject.SetActive(true);
        InitMimics();
        giftScreen.SetPartyData(mimics);
        Debug.Log("show party screen");
        yield return new WaitForSeconds(.5f);
        // HandleSelection();
        GameController.Instance.StartGift();
        Debug.Log("handle selection");
        //player.GetComponent<MimicParty>().AddMimic(selectedMimic);

        used = true;

        yield return DialogueManager.Instance.TypeDialouge("Player received {selectedMimic.mimic_base.Name}.");
        yield return new WaitForSeconds(2f);
    }

    void HandleSelection() {
       
        Action onSelected = () => {
            var selectedMimic = giftScreen.selectedMember;
            giftScreen.SetMessageText($"Player received {selectedMimic.mimic_base.Name}!");
            player.GetComponent<MimicParty>().AddMimic(selectedMimic);
            //break new WaitForSeconds(2f);
            giftScreen.gameObject.SetActive(false);
        };

        Action onBack = () => {
            giftScreen.SetMessageText("You have to choose a Mimic to continue!");
            return;
        };
        giftScreen.HandleUpdate(onSelected, onBack);
    }

    public bool CanBeGiven()
    {
        return mimics != null && !used;
    }

    void InitMimics()
    {
        foreach (var mimic in mimics)
        {
            mimic.Init();
        }
    }

    public object CaptureState() {
        return used;
    }

    public void RestoreState(object state) {
        used = (bool)state;
        if (used) {
            giftScreen.gameObject.SetActive(false);
        }
    }
}
