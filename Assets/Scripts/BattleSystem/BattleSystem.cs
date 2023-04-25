using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen, AboutToUse, None }


public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleMimic playerMimic;
    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleMimic enemyMimic;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] Image playerImage;
    [SerializeField] Image trainerImage;

    public event Action<bool> OnBattleOver;

    BattleState prevState;
    BattleState state;
    int currentAction;
    int currentMove;
    bool aboutToUseChoice = true;

    MimicParty playerParty;
    MimicParty trainerParty;
    Mimic wildMimic;

    bool isTrainerBattle = false;
    PlayerController player;
    TrainerController trainer;
    int escapeAttempts;

    public void StartBattle(MimicParty playerParty, Mimic wildMimic)
    {
        isTrainerBattle = false;
        this.playerParty = playerParty;
        this.wildMimic = wildMimic;
        StartCoroutine(SetupBattle());
    }

    public void StartTrainerBattle(MimicParty playerParty, MimicParty trainerParty)
    {
        this.playerParty = playerParty;
        this.trainerParty = trainerParty;
        isTrainerBattle = true;
        player = playerParty.GetComponent<PlayerController>();
        trainer = trainerParty.GetComponent<TrainerController>();

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerHUD.gameObject.SetActive(false);
        enemyHUD.gameObject.SetActive(false);
        
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        
        if (!isTrainerBattle)
        {
            // wild mimic battle
            enemyHUD.gameObject.SetActive(true);
            enemyMimic.Setup(wildMimic);
            enemyHUD.SetData(enemyMimic.mimic);
            
            playerHUD.gameObject.SetActive(true);
            playerImage.gameObject.SetActive(false);
            playerMimic.gameObject.SetActive(true);
            var playersMimic = playerParty.GetHealthyMimic();
            playerMimic.Setup(playersMimic);
            playerHUD.SetData(playersMimic);


            dialogBox.SetMoveNames(playerMimic.mimic.Moves);
            dialogBox.SetDialog($"A wild {enemyMimic.mimic.mimic_base.Name} appeared.");

            yield return new WaitForSeconds(2f);
        }
        else
        {
            // trainer

            // show trainer and player sprites
            playerMimic.gameObject.SetActive(false);
            enemyMimic.gameObject.SetActive(false);

            playerImage.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(true);
            playerImage.sprite = player.Sprite;
            trainerImage.sprite = trainer.Sprite;

            dialogBox.SetDialog($"{trainer.Name} started a battle!");
            yield return new WaitForSeconds(2f);

            // send out first mimic of trainer
            enemyHUD.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(false);
            enemyMimic.gameObject.SetActive(true);
            var trainerMimic = trainerParty.GetHealthyMimic();
            enemyMimic.Setup(trainerMimic);
            enemyHUD.SetData(trainerMimic);
            dialogBox.SetDialog($"{trainer.Name} sent out {enemyMimic.mimic.mimic_base.Name}.");
            yield return new WaitForSeconds(2f);

            // send out first mimic of player
            playerHUD.gameObject.SetActive(true);
            playerImage.gameObject.SetActive(false);
            playerMimic.gameObject.SetActive(true);
            var playersMimic = playerParty.GetHealthyMimic();
            playerMimic.Setup(playersMimic);
            playerHUD.SetData(playersMimic);
            dialogBox.SetDialog($"Go {playersMimic.mimic_base.Name}!");
            yield return new WaitForSeconds(2f);
            dialogBox.SetMoveNames(playerMimic.mimic.Moves);
        }

        escapeAttempts = 0;
        partyScreen.Init();
        PlayerAction();
    }

    void PlayerAction(){

        state = BattleState.PlayerAction;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen() {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Mimics);
        partyScreen.gameObject.SetActive(true);
    }

    IEnumerator AboutToUse(Mimic newMimic)
    {
        state = BattleState.Busy;
        dialogBox.SetDialog($"{trainer.Name} is about to use {newMimic.mimic_base.Name}. Do you want to change Mimics?");
        yield return new WaitForSeconds(1f);
        
        state = BattleState.AboutToUse;
        dialogBox.EnableChoiceBox(true);
    }

    void PlayerMove(){
        
        state = BattleState.PlayerMove;
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        
        var move = playerMimic.mimic.Moves[currentMove];

        /*
        if (playerMimic.mimic.Moves[0].Uses == 0 && playerMimic.mimic.Moves[1].Uses == 0 && playerMimic.mimic.Moves[2].Uses == 0 && playerMimic.mimic.Moves[3].Uses == 0)
        {
            dialogBox.SetDialog($"no moves left");
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyMove());
            yield break;
        }
        */
        int counter = 0;
        for (int i = 0; i < playerMimic.mimic.Moves.Count; i++)
        {
            if (playerMimic.mimic.Moves[i].Uses == 0)
            {
                counter++;
            }
            if (counter == playerMimic.mimic.Moves.Count)
            {
                dialogBox.SetDialog($"No Moves Left");
                yield return new WaitForSeconds(1f);
                StartCoroutine(EnemyMove());
                yield break;
            }
        }

        if (move.Uses == 0)
        {
            dialogBox.SetDialog($"Cannot Use Move");
            yield return new WaitForSeconds(1f);
            PlayerAction();
            yield break;
        }
        else
        {
            move.Uses--;
        }
        


        dialogBox.SetDialog($"{playerMimic.mimic.mimic_base.Name} used {move.Base.Name}.");

        yield return new WaitForSeconds(2f);

        playerMimic.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyMimic.PlayHitAnimation();

        var damageDetails = enemyMimic.mimic.TakeDamage(move, playerMimic.mimic);
        yield return enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            dialogBox.SetDialog($"{enemyMimic.mimic.mimic_base.Name} fainted.");
            yield return new WaitForSeconds(2f);
            playerMimic.mimic.mimicsDefeated += 1;
            if(playerMimic.mimic.mimicsDefeated == 2)
            {
                playerMimic.mimic.mimicsDefeated = 0;
                playerMimic.mimic.levelUp();
                playerHUD.SetData(playerMimic.mimic);
                dialogBox.SetDialog($"{playerMimic.mimic.mimic_base.Name} grew to level {playerMimic.mimic.level}!");
                yield return new WaitForSeconds(2f);
                dialogBox.SetMoveNames(playerMimic.mimic.Moves);
            }

            if (!isTrainerBattle)
            {
                OnBattleOver(true);
            }
            else 
            {
                var nextMimic = trainerParty.GetHealthyMimic();
                if (nextMimic != null)
                {
                    StartCoroutine(AboutToUse(nextMimic));
                }
                else {
                    OnBattleOver(true);
                }
            }
            
        }
        else{
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyMimic.mimic.GetRandomMove();
        dialogBox.SetDialog($"{enemyMimic.mimic.mimic_base.Name} used {move.Base.Name}.");

        yield return new WaitForSeconds(2f);

        enemyMimic.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerMimic.PlayHitAnimation();

        var damageDetails = playerMimic.mimic.TakeDamage(move, enemyMimic.mimic);
        yield return playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            dialogBox.SetDialog($"{playerMimic.mimic.mimic_base.Name} fainted.");
        
            yield return new WaitForSeconds(2f);

            var nextMimic = playerParty.GetHealthyMimic();
            if (nextMimic != null) {
                OpenPartyScreen();
            } else {
                OnBattleOver(false);
            }
            
        }
        else{
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Type > 1)
            dialogBox.SetDialog("It's super effective!");
        else if (damageDetails.Type < 1)
            dialogBox.SetDialog("It's not very effective...");

        yield return new WaitForSeconds(1f);
    }

    public void HandleUpdate(){
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen) {
            HandlePartySelection();
        }
        else if (state == BattleState.AboutToUse) {
            HandleAboutToUse();
        }
    }

    void HandleActionSelection(){
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (currentAction < 3)
            {
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (currentAction)
            {
                case 0: //Attack
                    PlayerMove();
                    break;

                case 1: //Items
                    break;

                case 2: //Mimics
                    OpenPartyScreen();
                    break;

                case 3: //Run
                    StartCoroutine(TryToEscape());
                    break;
                
                default:
                    break;
            }
        }
    }

    void HandleMoveSelection(){
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (currentMove < playerMimic.mimic.Moves.Count - 1)
            {
                ++currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }

        dialogBox.UpdateMoveSelection(currentMove, playerMimic.mimic.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.E))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Tab)) {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }

    void HandlePartySelection() {
        Action onSelected = () => {
            var selectedMember = partyScreen.selectedMember;
            if (selectedMember.currentHp <= 0) {
                partyScreen.SetMessageText("You can't send out a fainted Mimic!");
                return;
            }
            else if (selectedMember == playerMimic.mimic) {
                partyScreen.SetMessageText("You can't switch with the same Mimic.");
                return;
            }
            else {
                partyScreen.gameObject.SetActive(false);
                state = BattleState.Busy;
                StartCoroutine(SwitchMimic(selectedMember));
            }
        };

        Action onBack = () => {
            if (playerMimic.mimic.currentHp <= 0)
            {
                partyScreen.SetMessageText("You have to choose a Mimic to continue!");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            
            if (prevState == BattleState.AboutToUse)
            {
                prevState = BattleState.None;
                StartCoroutine(SendNextTrainerMimic());
            }
            else
                PlayerAction();
                
        };

        partyScreen.HandleUpdate(onSelected, onBack);
    }

    void HandleAboutToUse()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            aboutToUseChoice = !aboutToUseChoice;

        dialogBox.UpdateChoice(aboutToUseChoice);

        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogBox.EnableChoiceBox(false);
            if (aboutToUseChoice == true)
            {
                // Yes option

                prevState = BattleState.AboutToUse;
                OpenPartyScreen();
            }
            else
            {
                // No option

                StartCoroutine(SendNextTrainerMimic());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            dialogBox.EnableChoiceBox(false);
            StartCoroutine(SendNextTrainerMimic());
        }
    }

    IEnumerator SwitchMimic(Mimic newMimic) {
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(true);
        if (playerMimic.mimic.currentHp > 0) {
            dialogBox.SetDialog($"Come back {playerMimic.mimic.mimic_base.Name}!");
            yield return new WaitForSeconds(2f);
            dialogBox.SetDialog($"Come back {playerMimic.mimic.mimic_base.Name}!");
            yield return new WaitForSeconds(2f);
            
        }

        playerMimic.Setup(newMimic);
        playerHUD.SetData(newMimic);
        dialogBox.SetMoveNames(newMimic.Moves);
        dialogBox.SetDialog($"Go {newMimic.mimic_base.Name}!");
        yield return new WaitForSeconds(2f);
        
        if (prevState == BattleState.AboutToUse)
        {
            prevState = BattleState.None;
            StartCoroutine(SendNextTrainerMimic());
        }
        else
            StartCoroutine(EnemyMove());  
    }  

    IEnumerator SendNextTrainerMimic()
    {
        state = BattleState.Busy;

        var nextMimic = trainerParty.GetHealthyMimic();
        enemyMimic.Setup(nextMimic);
        enemyHUD.SetData(nextMimic);
        dialogBox.SetDialog($"{trainer.Name} sent out {nextMimic.mimic_base.Name}!");
        yield return new WaitForSeconds(2f);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(true);
        state = BattleState.PlayerAction;
    } 

    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        if (isTrainerBattle)
        {
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            dialogBox.SetDialog("You cannot run away from homeowners.");
            yield return new WaitForSeconds(2f);
            dialogBox.EnableDialogText(false);
            dialogBox.EnableActionSelector(true);
            state = BattleState.PlayerAction;
            yield break;
        }

        ++escapeAttempts;

        int playerSpeed = playerMimic.mimic.mimic_base.Speed;
        int enemySpeed = enemyMimic.mimic.mimic_base.Speed;

        if (enemySpeed < playerSpeed)
        {
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            dialogBox.SetDialog("You ran away safely.");
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                dialogBox.EnableActionSelector(false);
                dialogBox.EnableDialogText(true);
                dialogBox.SetDialog("You ran away safely.");
                yield return new WaitForSeconds(2f);
                OnBattleOver(true);
            }
            else
            {
                dialogBox.EnableActionSelector(false);
                dialogBox.EnableDialogText(true);
                dialogBox.SetDialog("You couldn't escape.");
                dialogBox.EnableDialogText(false);
                dialogBox.EnableActionSelector(true);
                yield return new WaitForSeconds(2f);
                state = BattleState.PlayerAction;
            }
        }
    }
}
