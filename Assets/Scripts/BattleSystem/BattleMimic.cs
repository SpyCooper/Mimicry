using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleMimic : MonoBehaviour
{
    [SerializeField] bool isPlayerMimic;
    [SerializeField] BattleHUD hud;

    public Mimic mimic {get; set;}

    Image Image;
    Vector3 originalPos;
    Color originalColor;

    public bool IsPlayerMimic {
        get {return isPlayerMimic; }
    }
    
    public BattleHUD Hud {
        get { return hud; }
    }

    public Mimic Mimic { get; set; }

    private void Awake()
    {
        Image = GetComponent<Image>();
        originalPos = Image.transform.localPosition;
        originalColor = Image.color;
    }

    public void Setup(Mimic Mimic)
    {
        mimic = Mimic;
        GetComponent<Image>().sprite = mimic.mimic_base.Sprite;
        //hud.gameObject.SetActive(true);
        //hud.SetData(mimic);
    }

    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerMimic)
        {
           sequence.Append(Image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(Image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }

        sequence.Append(Image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(Image.DOColor(Color.gray, 0.1f));
        sequence.Append(Image.DOColor(originalColor, 0.1f));
    }
}
