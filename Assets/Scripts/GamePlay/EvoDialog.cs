using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvoDialog : MonoBehaviour
{
    [SerializeField] Text dialogText;

    public void EnableDialogText(bool enabled){
        dialogText.enabled = enabled;
    }

    public void SetDialog(string dialog){
        dialogText.text = dialog;
    }
}