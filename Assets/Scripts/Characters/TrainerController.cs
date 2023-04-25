using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] string _name;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialouge dialogue;
    [SerializeField] Dialouge dialogueAfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    // State
    bool battleLost = false;

    Character character;
    MimicGiver mimicGiver;

    private void Awake()
    {
        character = GetComponent<Character>();
        mimicGiver = GetComponent<MimicGiver>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    private void Update()
    {
        character.HandleUpdate();
    }

    public IEnumerator Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (!battleLost)
        {
            // Show dialogue
            yield return DialogueManager.Instance.ShowDialouge(dialogue); 
            GameController.Instance.StartTrainerBattle(this);
        }
        else 
        {
            // state = NPCState.Dialogue;
            character.LookTowards(initiator.position);
            
            if (mimicGiver != null && mimicGiver.CanBeGiven())
            {
                Debug.Log("Reached Mimic giver");
                yield return mimicGiver.GiveMimic(initiator.GetComponent<PlayerController>());
            }
            else
            {
                yield return DialogueManager.Instance.ShowDialouge(dialogueAfterBattle);
            }
            
            // idleTimer = 0.5f;
            // state = NPCState.Idle;
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        // Show exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // Walk towards the player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        // Show dialogue
        yield return DialogueManager.Instance.ShowDialouge(dialogue);
        GameController.Instance.StartTrainerBattle(this);
    }

    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 90f;
        else if (dir == FacingDirection.Left)
            angle = 270f;
        
        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public object CaptureState() {
        return battleLost;
    }

    public void RestoreState(object state) {
        battleLost = (bool)state;
        if (battleLost) {
            fov.gameObject.SetActive(false);
        }
    }

    public string Name {
        get => _name;
    }

    public Sprite Sprite {
        get => sprite;
    }
}
