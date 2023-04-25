using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerController : MonoBehaviour, ISavable
{
    //overworld = background
    //soildobjects = foreground


    /* Allows for speed of the player to be edited in the Unity Editor */

//////////////////UGLY TELEPORTING STUFF////////////////////////
    public LayerMask GotoStore;
    public LayerMask Town1LeaveStore;
    public Transform storedestination;
    public Transform town1leavestoredestination;

    public LayerMask GotoHouse1Town1;
    public LayerMask LeaveHouse1Town1;
    public Transform House1Town1destination;
    public Transform house1town1leavedestination;

    public LayerMask GotoHouse2Town1;
    public LayerMask LeaveHouse2Town1;
    public Transform House2Town1destination;
    public Transform house2town1leavedestination;

    public LayerMask GotoStoreTown2;
    public LayerMask LeaveStoreTown2;
    public Transform store2destination;
    public Transform town2leavestoredestination;

    public LayerMask GoToGym1;
    public LayerMask LeaveGym1;
    public Transform Gym1Destination;
    public Transform LeaveGym1destination;

    public LayerMask GotoStoreTown3;
    public LayerMask LeaveStoreTown3;
    public Transform store3destination;
    public Transform town3leavestoredestination;

    public LayerMask GoToGym2;
    public LayerMask LeaveGym2;
    public Transform Gym2Destination;
    public Transform LeaveGym2destination;

    public LayerMask GoToGym3;
    public LayerMask LeaveGym3;
    public Transform Gym3Destination;
    public Transform LeaveGym3destination;
    
    public LayerMask GotoStoreTown4;
    public LayerMask LeaveStoreTown4;
    public Transform store4destination;
    public Transform town4leavestoredestination;

    public LayerMask GotoStoreTown5;
    public LayerMask LeaveStoreTown5;
    public Transform store5destination;
    public Transform town5leavestoredestination;

    public LayerMask GoToGym4;
    public LayerMask LeaveGym4;
    public Transform Gym4Destination;
    public Transform LeaveGym4destination;

    public LayerMask GoToHOA;
    public LayerMask LeaveHOA;
    public Transform HOADestination;
    public Transform LeaveHOAdestination;
///////////////////////////////////////////////////////////////
    [SerializeField] string _name;
    [SerializeField] Sprite sprite;
    
    public event Action OnEncounter;
    public event Action<Collider2D> OnEnterTrainerView;
    

    // Input for the movement
    private Vector2 input;

    private Character character;

    private void Awake(){
        character = GetComponent<Character>();
    }

    /* UPDATE FUNCTION. */
    public void HandleUpdate(){

        if(!character.IsMoving){
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Gets rid of diagonal direction
            if(input.x != 0){
                input.y = 0;
            }

            // If movement is detected
            if(input != Vector2.zero){
                StartCoroutine(character.Move(input, OnMoveOver));                
            }
        }

        character.HandleUpdate();


        // Interact button to interact with the Overworld
        if(Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Interact());

        
    }

    /* Allows for interaction with the Overworld npcs, buildings, items, etc. */
    IEnumerator Interact(){

        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position , facingDir, Color.blue, 2.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.1f, GameLayers.i.InteractableLayer);
        if(collider != null){
            yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }

    }

    private void OnMoveOver()
    {
        CheckForEncounters();
        CheckForPortals();
        CheckIfInTrainerView();
    }




    /* Checks to see if we have encountered any live mimics in the wild */
    private void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null){
            if(UnityEngine.Random.Range(1, 101) <= 10){
                character.Animator.IsMoving = false;
                OnEncounter();
            }
        }
    }
    
    private void CheckForPortals(){
        if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoStore) != null){
                transform.position = storedestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, Town1LeaveStore) != null){
                transform.position = town1leavestoredestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoHouse1Town1) != null){
                transform.position = House1Town1destination.transform.position;
        }else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveHouse1Town1) != null){
                transform.position = house1town1leavedestination.transform.position;
        }else if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoHouse2Town1) != null){
                transform.position = House2Town1destination.transform.position;
        }else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveHouse2Town1) != null){
                transform.position = house2town1leavedestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoStoreTown2) != null){
                transform.position = store2destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveStoreTown2) != null){
                transform.position = town2leavestoredestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GoToGym1) != null){
                transform.position = Gym1Destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveGym1) != null){
                transform.position = LeaveGym1destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoStoreTown3) != null){
                transform.position = store3destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveStoreTown3) != null){
                transform.position = town3leavestoredestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GoToGym2) != null){
                transform.position = Gym2Destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveGym2) != null){
                transform.position = LeaveGym2destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GoToGym3) != null){
                transform.position = Gym3Destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveGym3) != null){
                transform.position = LeaveGym3destination.transform.position;
        }else if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoStoreTown4) != null){
                transform.position = store4destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveStoreTown4) != null){
                transform.position = town4leavestoredestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GotoStoreTown5) != null){
                transform.position = store5destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveStoreTown5) != null){
                transform.position = town5leavestoredestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, GoToGym4) != null){
                transform.position = Gym4Destination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveGym4) != null){
                transform.position = LeaveGym4destination.transform.position;
        }else if(Physics2D.OverlapCircle(transform.position, 0.3f, GoToHOA) != null){
                transform.position = HOADestination.transform.position;
        } else if(Physics2D.OverlapCircle(transform.position, 0.3f, LeaveHOA) != null){
                transform.position = LeaveHOAdestination.transform.position;
        }
    }

    private void CheckIfInTrainerView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);
        if(collider != null)
        {
             character.Animator.IsMoving = false;
             OnEnterTrainerView?.Invoke(collider);
        }
    }

    public object CaptureState() {
        var saveData = new PlayerSaveData() {
            position = new float[] {transform.position.x, transform.position.y},
            mimics = GetComponent<MimicParty>().Mimics.Select(p => p.GetSaveData()).ToList()
        };
        return saveData;
    }

    public void RestoreState(object state) {
        var saveData = (PlayerSaveData)state;
        var position = saveData.position;
        transform.position = new Vector3(position[0], position[1]);
        GetComponent<MimicParty>().Mimics = saveData.mimics.Select(s => new Mimic(s)).ToList();
    }

    public string Name {
        get => _name;
    }

    public Sprite Sprite {
        get => sprite;
    }
}

[Serializable]
public class PlayerSaveData {
    public float[] position;
    public List<MimicSaveData> mimics;
}