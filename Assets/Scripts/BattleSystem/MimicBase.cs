using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mimic", menuName = "Mimic/Create new mimic")]

public class MimicBase : ScriptableObject
{
    [SerializeField] string _name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite sprite;

    [SerializeField] MimicType type1;

    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;

    [SerializeField] int currentHp;

    [SerializeField] List<LearnableMove> learnableMoves;

    [SerializeField] List<Evolution> evolutions;

    public string Name {
        get {return _name;}
    }

    public string Description {
        get {return description;}
    }

    public Sprite Sprite {
        get {return sprite;}
    }

    
    public MimicType Type1 {
        get {return type1;}
    }

    public int MaxHp {
        get {return maxHp;}
    }

    public int CurrentHp {
        get {return currentHp;}
    }

    public int Attack {
        get {return attack;}
    }

    public int Defense {
        get {return defense;}
    }

    public int Speed {
        get {return speed;}
    }

    public List<LearnableMove> LearnableMoves {
        get {return learnableMoves;}
    }

    public List<Evolution> Evolutions => evolutions;
}


[System.Serializable]
public class LearnableMove 
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get {return moveBase;}
    }

    public int Level {
        get {return level;}
    }
}

[System.Serializable]
public class Evolution 
{
    [SerializeField] MimicBase evolvesInto;
    [SerializeField] int evolutionLevel;

    public MimicBase EvolvesInto => evolvesInto;

    public int EvolutionLevel => evolutionLevel;
}

public enum MimicType
{
    None,
    Metal,
    Electric,
    Water,
    Heat,
    Cold,
    Wood,
    Cloth,
    Glass,
    Brick
}

public enum Stat
{
    Attack,
    Defense,
    Speed
}

public class TypeChart
{
    static float[][] chart =
    {
        
        ///                           METL ELTC WATR HEAT COLD WOOD CLTH GLAS BRCK
        /*Metal*/       new float[] {  1f, .5f,  1f, .5f,  1f,  2f,  1f,  2f,  1f},
        /*Electric*/    new float[] {  2f, .5f,  2f,  1f,  1f, .5f, .5f,  1f,  2f},
        /*Water*/       new float[] {  1f, .5f, .5f,  1f, .5f,  2f,  2f,  1f,  1f},
        /*Heat*/        new float[] {  2f,  1f,  1f, .5f,  2f,  1f, .5f, .5f,  1f},
        /*Cold*/        new float[] {  1f,  1f,  2f,  2f, .5f,  1f, .5f,  1f,  1f},
        /*Wood*/        new float[] { .5f,  2f, .5f,  1f,  1f,  1f,  1f,  1f,  1f},
        /*Cloth*/       new float[] {  1f,  2f, .5f,  2f,  2f,  1f,  1f, .5f,  1f},
        /*Glass*/       new float[] { .5f,  1f,  1f,  2f,  1f,  1f,  2f, .5f, .5f},
        /*Brick*/       new float[] {  1f, .5f,  1f,  1f,  1f,  1f,  1f,  2f,  2f}
    };


    public static float GetEffectiveness(MimicType attackType, MimicType defenseType)
    {
        if (attackType == MimicType.None || defenseType == MimicType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}