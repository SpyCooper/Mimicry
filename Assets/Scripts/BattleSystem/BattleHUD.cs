using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text HP_FractionText;
    [SerializeField] HPBar hpBar;

    Mimic _mimic;

    public void SetData(Mimic mimic)
    {
        _mimic = mimic;
        nameText.text = mimic.mimic_base.Name;
        levelText.text = "Lvl " + mimic.level;
        HP_FractionText.text = mimic.currentHp + "/" + mimic.MaxHp;
        hpBar.SetHP((float) mimic.currentHp / mimic.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.setHPSmooth((float)_mimic.currentHp / _mimic.MaxHp);
        HP_FractionText.text = _mimic.currentHp + "/" + _mimic.MaxHp;
    }
}
