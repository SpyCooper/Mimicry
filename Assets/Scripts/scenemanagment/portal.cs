using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour, iPlayerTriggerable
{
    public void onPlayerTriggerable(PlayerController player){
        Debug.Log("Player entered portal !");
    }
}
