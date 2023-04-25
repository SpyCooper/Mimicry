using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Menu, PartyScreen, Dialogue, Battle, Cutscene, IDs, Evolution, GiftScreen}

public class GameController : MonoBehaviour
{
    GameState state;
    MenuControls menuController;
    IDsController IDsController;
    [SerializeField] PlayerController PlayerController;
    [SerializeField] DialogueManager DialogueManager;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] GiftScreen giftScreen;

    public static GameController Instance { get; private set; }


    private void Awake() {
        Instance = this;
        MimicDB.Init();
        MoveDB.Init();
        menuController = GetComponent<MenuControls>();
        IDsController = GetComponent<IDsController>();
    }

    private void Start() {
        menuController.onBack += () => {
            state = GameState.FreeRoam;
        };
        IDsController.onBack += () => {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;
        PlayerController.OnEncounter += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        PlayerController.OnEnterTrainerView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(PlayerController));

            }
        };

        partyScreen.Init();

        DialogueManager.Instance.OnShowDialouge += () => {
            state = GameState.Dialogue;
        };
        DialogueManager.Instance.OnCloseDialouge += () => {
            
            if(state == GameState.Dialogue){
                state = GameState.FreeRoam;
            }
        };

        EvolutionManager.evoMan.OnStartEvo += () => { state = GameState.Evolution;};
        EvolutionManager.evoMan.OnCompleteEvo += () => { state = GameState.FreeRoam;};
    }
 
    private void Update()
    {
        if (state == GameState.Menu) {
            menuController.OpenMenu();
            menuController.HandleUpdate();
        }
        if (state == GameState.IDs) {
            IDsController.OpenIDs();
            IDsController.HandleUpdate();
        }
        if(state == GameState.FreeRoam){
            // Menu can only be opened in the overworld
            if (Input.GetKeyDown(KeyCode.Escape)) {
                state = GameState.Menu;
            }
            PlayerController.HandleUpdate();
        } else if(state == GameState.Dialogue){
            DialogueManager.Instance.HandleUpdate();
        } else if (state == GameState.Battle) {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.PartyScreen) {
            Action onSelected = () => {
                // Go to Summary Screen
            };

            Action onBack = () => {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };
            
            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.GiftScreen)
        {
            Action onSelected = () => {
                var selectedMimic = giftScreen.selectedMember;
                giftScreen.SetMessageText($"Player received {selectedMimic.mimic_base.Name}!");
                PlayerController.GetComponent<MimicParty>().AddMimic(selectedMimic);
                giftScreen.gameObject.SetActive(false);
                EndGift();
            };

            Action onBack = () => {
                giftScreen.SetMessageText("You have to choose a Mimic to continue!");
                return;
            };
            giftScreen.HandleUpdate(onSelected, onBack);
        }
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit");
    }

    void OnMenuSelected(int selectedItem) {
        if (selectedItem == 0) {
            // Mimics
            partyScreen.gameObject.SetActive(true);
            partyScreen.SetPartyData(PlayerController.GetComponent<MimicParty>().Mimics);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 1) {
            // IDS
            state = GameState.IDs;
        }
        else if (selectedItem == 2) {
            // Save
            SavingSystem.i.Save("saveSlot");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 3) {
            // Load
            SavingSystem.i.Load("saveSlot");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 4) {
            // Exit
            QuitGame();
            state = GameState.FreeRoam;
        }
    }

    void StartBattle() {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = PlayerController.GetComponent<MimicParty>();
        var wildMimic = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildMimic();

        battleSystem.StartBattle(playerParty, wildMimic);
    }

    TrainerController trainer;

    public void StartTrainerBattle(TrainerController trainer) 
    {
        
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        var playerParty = PlayerController.GetComponent<MimicParty>();
        var trainerParty = trainer.GetComponent<MimicParty>();
        
        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }

    void EndBattle(bool won) 
    {
        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }
        else if(won == false)
        {
            SavingSystem.i.Load("saveSlot");
            state = GameState.FreeRoam;
        }

        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        var playerParty = PlayerController.GetComponent<MimicParty>();
        StartCoroutine(playerParty.Check4Evolutions());
    }

    public void StartGift()
    {
        state = GameState.GiftScreen;
    }

    public void EndGift()
    {
        state = GameState.FreeRoam;
    }
}
