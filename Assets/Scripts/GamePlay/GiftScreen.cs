using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    GiftMemberUI[] memberSlots;
    List<Mimic> mimics;

    int currentMember = 0;

    public Mimic selectedMember => mimics[currentMember];

    public void Init() {
        memberSlots = GetComponentsInChildren<GiftMemberUI>(true);
    }

    public void SetPartyData(List<Mimic> mimics) {
        this.mimics = mimics;
        
        for (int i = 0; i < memberSlots.Length; i++) {
            if (i < mimics.Count) {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].SetData(mimics[i]);
            }
            else {
                memberSlots[i].gameObject.SetActive(false);
            }
        }

        messageText.text = "Choose a Mimic.";
    }

    public void HandleUpdate(Action onSelected, Action onBack) {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            ++currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            ++currentMember;
        }

        currentMember = Mathf.Clamp(currentMember, 0, mimics.Count - 1);

        UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.E)) {
            onSelected?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Tab)) {
            onBack?.Invoke();
        }
    }

    public void UpdateMemberSelection(int selectedMember) {
        for (int i = 0; i < mimics.Count; i++) {
            if (i == selectedMember) {
                memberSlots[i].SetSelected(true);
            }
            else {
                memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string message) {
        messageText.text = message;
    }
}
