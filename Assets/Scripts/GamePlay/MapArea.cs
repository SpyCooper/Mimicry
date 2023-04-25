using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Mimic> wildMimics;

    public Mimic GetRandomWildMimic() {
        var wildMimic = wildMimics[Random.Range(0, wildMimics.Count)];
        wildMimic.Init();
        return wildMimic;
    }
}
