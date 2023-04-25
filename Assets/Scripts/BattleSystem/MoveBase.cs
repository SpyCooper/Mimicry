using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Move", menuName = "Mimic/Create new move")]

public class MoveBase : ScriptableObject
{
    [SerializeField] string _name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] MimicType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int uses;


    public string Name {
        get { return _name;}
    }

    public string Description {
        get { return description;}
    }

    public MimicType Type {
        get { return type;}
    }

    public int Power {
        get { return power;}
    }

    public int Accuracy {
        get { return accuracy;}
    }

    public int Uses {
        get { return uses;}
    }
}
