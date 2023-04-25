using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject choiceBox;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text moveUSES;
    [SerializeField] Text movePower;
    [SerializeField] Text moveType;
    [SerializeField] Text moveDescription;

    [SerializeField] Text yesText;
    [SerializeField] Text noText;


    public void SetDialog(string dialog){
        dialogText.text = dialog;
    }

    public void EnableDialogText(bool enabled){
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled){
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled){
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void EnableChoiceBox(bool enabled){
        choiceBox.SetActive(enabled);
    }


    public void UpdateActionSelection(int selectedAction){
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if(i == selectedAction)
            {
                actionTexts[i].color = Color.blue;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move){
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if(i == selectedMove)
            {
                moveTexts[i].color = Color.blue;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }

        moveUSES.text = $"USES: {move.Uses}/{move.Base.Uses}";
        movePower.text = $"PWR: {move.Base.Power.ToString()}";
        moveType.text = $"TYPE: {move.Base.Type.ToString()}";
        moveDescription.text = move.Base.Description;
    }

    public void UpdateChoice(bool yesSelected)
    {
        if (yesSelected)
        {
            yesText.color = Color.blue;
            noText.color = Color.black;
        }
        else
        {
            yesText.color = Color.black;
            noText.color = Color.blue;
        }
    }

    public void SetMoveNames(List<Move> moves){

        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }
}
