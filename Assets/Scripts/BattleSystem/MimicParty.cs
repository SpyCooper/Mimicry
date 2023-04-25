using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MimicParty : MonoBehaviour
{
    [SerializeField] List<Mimic> mimics;

    public event Action OnUpdated;

    public List<Mimic> Mimics {
        get {
            return mimics;
        }
        set {
            mimics = value;
        }
    }

    private void Start() {
        foreach (var mimic in mimics) {
            mimic.Init();
        }
    }

    public Mimic GetHealthyMimic() {
        return mimics.Where(x => x.currentHp > 0).FirstOrDefault();
    }

    public void PartyUpdated()
    {
        OnUpdated?.Invoke();
    }

    public IEnumerator Check4Evolutions()
    {
        foreach(var mimic in mimics)
        {
            var evolution = mimic.Check4Evolution();
            if (evolution != null)
            {
                yield return EvolutionManager.evoMan.Evolve(mimic, evolution);
            }
        }
        PartyUpdated();
    }

    public void AddMimic(Mimic newMimic)
    {
        if (mimics.Count < 6)
        {
            mimics.Add(newMimic);
        }
        else 
        {
            // Transfer to PC
        }
    }
}
