using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialouge dialouge;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    Character character;
    MimicGiver mimicGiver;
    Shopkeep shopkeep;

    private void Awake()
    {
        character = GetComponent<Character>();
        shopkeep = GetComponent<Shopkeep>();
        mimicGiver = GetComponent<MimicGiver>();
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialogue;
            character.LookTowards(initiator.position);
            
            if (mimicGiver != null && mimicGiver.CanBeGiven())
            {
                Debug.Log("Reached Mimic giver");
                yield return mimicGiver.GiveMimic(initiator.GetComponent<PlayerController>());
            }
            else if(shopkeep != null)
            {
                yield return shopkeep.Heal(initiator, dialouge);
            }
            else
            {
                yield return DialogueManager.Instance.ShowDialouge(dialouge);
            }
            
            idleTimer = 0.5f;
            state = NPCState.Idle;
        }
        
            
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
        }

        character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentPattern]);
        
        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
}

public enum NPCState { Idle, Walking, Dialogue } 