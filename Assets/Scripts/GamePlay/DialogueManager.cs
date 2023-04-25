
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialougeBox;
    [SerializeField] Text dialougeText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialouge;
    public event Action OnCloseDialouge;

    public static DialogueManager Instance { get; private set; }

    private void Awake(){
        Instance = this;
    }

    public bool IsShowing { get; private set; }

    public void HandleUpdate()
    {
        
    }

    public IEnumerator ShowDialouge(Dialouge dialouge){

        yield return new WaitForEndOfFrame();

        OnShowDialouge?.Invoke();
        IsShowing = true;
        dialougeBox.SetActive(true);

        foreach (var line in dialouge.Lines)
        {
            yield return TypeDialouge(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        dialougeBox.SetActive(false);
        IsShowing = false;
        OnCloseDialouge?.Invoke();
    }

    public IEnumerator TypeDialouge(string line)
    {
        dialougeText.text = "";
        foreach(var letter in line.ToCharArray()){
            dialougeText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }



}
