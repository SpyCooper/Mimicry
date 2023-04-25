using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicDB : MonoBehaviour
{
    static Dictionary<string, MimicBase> mimics;

    public static void Init() {
        mimics = new Dictionary<string, MimicBase>();
        
        var mimicArray = Resources.LoadAll<MimicBase>("");
        foreach (var mimic in mimicArray) {
            if (mimics.ContainsKey(mimic.Name)) {
                Debug.LogError($"There are two mimics with this name");
                continue;
            }
            mimics[mimic.Name] = mimic;
        }
    }

    public static MimicBase GetMimicByName(string name) {
        if (!mimics.ContainsKey(name)) {
            Debug.LogError($"There is no Mimic with the name {name}");
            return null;
        }
        return mimics[name];
    }
}