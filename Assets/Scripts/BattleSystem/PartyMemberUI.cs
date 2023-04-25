using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text HP_FractionText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color highlightedColor;

    Mimic _mimic;

    public void SetData(Mimic mimic)
    {
        _mimic = mimic;
        nameText.text = mimic.mimic_base.Name;
        levelText.text = "Lvl " + mimic.level;
        HP_FractionText.text = mimic.currentHp + "/" + mimic.MaxHp;
        hpBar.SetHP((float) mimic.currentHp / mimic.MaxHp);
    }

    public void SetSelected(bool selected) {
        if (selected) {
            nameText.color = highlightedColor;
        }
        else {
            nameText.color = Color.black;
        }
    }
}
