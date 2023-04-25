using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Image mimicImage;

    Mimic _mimic;

    public void SetData(Mimic mimic)
    {
        _mimic = mimic;
        nameText.text = mimic.mimic_base.Name;
        levelText.text = "Lvl " + mimic.level;
        mimicImage.sprite = mimic.mimic_base.Sprite;
    }

    public void SetSelected(bool selected) {
        if (selected) {
            nameText.color = Color.blue;
        }
        else {
            nameText.color = Color.black;
        }
    }
}
