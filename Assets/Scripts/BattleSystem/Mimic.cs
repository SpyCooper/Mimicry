using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]

public class Mimic
{
    [SerializeField] MimicBase _base;
    [SerializeField] int _level;

    public MimicBase mimic_base {
        get {
            return _base;
        }
    }
    public int level {
        get {
            return _level;
        }
    }
    
    public int currentHp { get; set;}

    public List<Move> Moves { get; set;}
    public Dictionary<Stat, int> Stats {get; private set;}
    public List<Move> Desperate { get; set;}

    public int mimicsDefeated { get; set;}

    public void Init()
    {
        Moves = new List<Move>();
        foreach (var move in mimic_base.LearnableMoves) /// Sets the list of moves for the mimic
        {
            if (move.Level <= level){
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4){
                break;
            }
        }
        CalculateStats();
        currentHp = MaxHp; ///Defaults the currentHP to the maxHP
    }

    public Mimic(MimicSaveData saveData) {
        _base = MimicDB.GetMimicByName(saveData.name);
        currentHp = saveData.hp;
        _level = saveData.level;
        mimicsDefeated = mimicsDefeated;
        Moves = saveData.moves.Select(s => new Move(s)).ToList();
        CalculateStats();
    }

    public MimicSaveData GetSaveData() {
        var saveData = new MimicSaveData() {
            name = mimic_base.Name,
            hp = currentHp,
            level = level,
            mimicsDefeated = mimicsDefeated,
            moves = Moves.Select(m => m.GetSaveData()).ToList()
        };
        return saveData;
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((mimic_base.Attack * level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((mimic_base.Defense * level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((mimic_base.Speed * level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((mimic_base.MaxHp * level) / 100f) + 10;
    }

    int getStat(Stat stat)
    {
        return Stats[stat];
    }

    public int MaxHp { get; private set;}

    public int Attack {///Formula for Attack based on level
        get { return getStat(Stat.Attack);}
    }

    public int Defense { ///Formula for Defense based on level
        get { return getStat(Stat.Defense);}
    }

    public int Speed { ///Formula for Speed based on level
        get { return getStat(Stat.Speed);}
    }

    public DamageDetails TakeDamage(Move move, Mimic attacker)
    {
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.mimic_base.Type1); ///Multiply this by itself with Type2 for dual type
       
        var damageDetails = new DamageDetails()
        {
            Type = type, 
            Fainted = false
        };

        float modifiers = Random.Range(0.85f,1f) * type;
        float a = (2 * attacker.level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public void levelUp()
    {
        double oldMaxHP = MaxHp;
        ++_level;
        double healthPercent = (currentHp / oldMaxHP);
        currentHp = (int)(MaxHp * healthPercent);
        foreach (var move in mimic_base.LearnableMoves) /// Adds a move on level up.
        {
            if (move.Level == level){
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4){
                break;
            }
        }
    }

    public void Heal()
    {
        Init();
    }

    public Evolution Check4Evolution()
    {
        return mimic_base.Evolutions.FirstOrDefault(e => e.EvolutionLevel <= level);
    }

    public void Evolve(Evolution evo)
    {
        double oldMaxHP = MaxHp;
        double healthPercent = (currentHp / oldMaxHP);

        _base = evo.EvolvesInto;
        CalculateStats();
        
        currentHp = (int)(MaxHp * healthPercent);
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }

    public float Type { get; set; }
}

[System.Serializable]
public class MimicSaveData {
    public string name;
    public int hp;
    public int level;
    public int mimicsDefeated;
    public List<MoveSaveData> moves;
}