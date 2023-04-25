using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Move
{
    public MoveBase Base { get; set; }

    public int Uses { get; set;}
    public int maxUses { get; set;}

    public Move(MoveBase mBase){
        Base = mBase;
        Uses = mBase.Uses; 
        maxUses = mBase.Uses;
    }

    public Move(MoveSaveData saveData) {
        Base = MoveDB.GetMoveByName(saveData.name);
        Uses = saveData.uses;
    }

    public MoveSaveData GetSaveData() {
        var saveData = new MoveSaveData() {
            name = Base.name,
            uses = Uses
        };
        return saveData;
    }
    public void restore()
    {
        Uses = maxUses;
    }
}

[System.Serializable]
public class MoveSaveData {
    public string name;
    public int uses;
}
