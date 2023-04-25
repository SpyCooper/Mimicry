using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField] EvoDialog evoDialogBox;
    [SerializeField] GameObject evolutionUI;
    [SerializeField] Image mimicImage;

    public event Action OnStartEvo;
    public event Action OnCompleteEvo;


    
    public static EvolutionManager evoMan {get; private set;}

    private void Awake()
    {
        evoMan = this;
    }

    public IEnumerator Evolve(Mimic mimic, Evolution evolution)
    {
        OnStartEvo?.Invoke();
        evolutionUI.SetActive(true);
        evoDialogBox.EnableDialogText(true);

        mimicImage.sprite = mimic.mimic_base.Sprite;
        evoDialogBox.SetDialog($"{mimic.mimic_base.Name} is evolving...");
        yield return new WaitForSeconds(2f);

        var oldMimic = mimic.mimic_base;
        mimic.Evolve(evolution);

        mimicImage.sprite = mimic.mimic_base.Sprite;
        evoDialogBox.SetDialog($"{oldMimic.Name} has evolved into {mimic.mimic_base.Name}!");
        yield return new WaitForSeconds(2f);

        evolutionUI.SetActive(false);
        evoDialogBox.EnableDialogText(false);
        OnCompleteEvo?.Invoke();
    }
}    //////////////////////////////FIX DIALOGUEEEEEEEEEEEEEEEEEEEEEE////////////////////////////////////
