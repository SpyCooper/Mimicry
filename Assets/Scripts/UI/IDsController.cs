using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IDsController : MonoBehaviour
{
    [SerializeField] GameObject IDs;
    [SerializeField] public Text text;
    [SerializeField] public Image image;

    // Massive list of Sprite Switches
    public Sprite Fork;
    public Sprite Spoon;
    public Sprite Spork;
    public Sprite ButterKnife;
    public Sprite ButcherKnife;
    public Sprite FryingPan;
    public Sprite Pot;
    public Sprite Screwdriver;
    public Sprite Computer;
    public Sprite PowerDrill;
    public Sprite WallOutlet;
    public Sprite Transformer;
    public Sprite Vacuum;
    public Sprite Fan;
    public Sprite Sink;
    public Sprite Bathtub;
    public Sprite Hose;
    public Sprite Dishwasher;
    public Sprite ClothesWasher;
    public Sprite Stove;
    public Sprite Oven;
    public Sprite Microwave;
    public Sprite Toaster;
    public Sprite ToasterOven;
    public Sprite Dryer;
    public Sprite Furnace;
    public Sprite MiniFridge;
    public Sprite FreezerFridge;
    public Sprite DoubleDoorFridge;
    public Sprite TopLoadingFreezer;
    public Sprite AirConditioner;
    public Sprite Log;
    public Sprite Table;
    public Sprite Dresser;
    public Sprite Trunk;
    public Sprite Stick;
    public Sprite RollingPin;
    public Sprite DishTowel;
    public Sprite Towel;
    public Sprite Sheets;
    public Sprite Bed;
    public Sprite Cup;
    public Sprite Fishbowl;
    public Sprite Aquarium;
    public Sprite Bowl;
    public Sprite BiggerBowl;
    public Sprite EvenBiggerBowl;
    public Sprite Brick;
    public Sprite Fireplace;
    public Sprite House;

    public event Action onBack;

    List<Text> IDsItems; 

    int selectedItem = 1;

    private void Awake() {
        IDsItems = IDs.GetComponentsInChildren<Text>().ToList();
    }
    
    public void OpenIDs() {
        IDs.SetActive(true);
        UpdateItemSelection();
    }

    public void CloseIDs() {
        IDs.SetActive(false);
    }

    public void HandleUpdate() {
        int prevSelection = selectedItem;
        
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            selectedItem += 5;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            selectedItem -= 5;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            --selectedItem;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            ++selectedItem;
        }
        selectedItem = Mathf.Clamp(selectedItem, 1, IDsItems.Count - 1);
        if (prevSelection != selectedItem) {
            UpdateItemSelection();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            ChangeIDsMimic(selectedItem);
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            onBack?.Invoke();
            selectedItem = 0;
            CloseIDs();
        }
    }

    void UpdateItemSelection() {
        for (int i = 0; i < IDsItems.Count; i++) {
            if (i == selectedItem) {
                IDsItems[i].color = Color.blue;
            }
            else {
                IDsItems[i].color = Color.black;
            }
        }
    }

    public void ChangeIDsMimic(int selectedItem) {
        switch(selectedItem) {
            case 1 :
                text.text = "ID: #01\nName: Fork\nType: Metal";
                image.sprite = Fork;
                break;
            case 2 :
                text.text = "ID: #02\nName: Spoon\nType: Metal";
                image.sprite = Spoon;
                break;
            case 3 :
                text.text = "ID: #03\nName: Spork\nType: Metal";
                image.sprite = Spork;
                break;
            case 4 :
                text.text = "ID: #04\nName: Butter Knife\nType: Metal";
                image.sprite = ButterKnife;
                break;
            case 5 :
                text.text = "ID: #05\nName: Butcher Knife\nType: Metal";
                image.sprite = ButcherKnife;
                break;
            case 6 :
                text.text = "ID: #06\nName: Frying Pan\nType: Metal";
                image.sprite = FryingPan;
                break;
            case 7 :
                text.text = "ID: #07\nName: Pot\nType: Metal";
                image.sprite = Pot;
                break;
            case 8 :
                text.text = "ID: #08\nName: Screwdriver\nType: Metal";
                image.sprite = Screwdriver;
                break;
            case 9 :
                text.text = "ID: #09\nName: Computer\nType: Electric";
                image.sprite = Computer;
                break;
            case 10 :
                text.text = "ID: #10\nName: Power Drill\nType: Electric";
                image.sprite = PowerDrill;
                break;
            case 11 :
                text.text = "ID: #11\nName: Wall Outlet\nType: Electric";
                image.sprite = WallOutlet;
                break;
            case 12 :
                text.text = "ID: #12\nName: Transformer\nType: Electric";
                image.sprite = Transformer;
                break;
            case 13 :
                text.text = "ID: #13\nName: Vacuum\nType: Electric";
                image.sprite = Vacuum;
                break;
            case 14 :
                text.text = "ID: #14\nName: Fan\nType: Electric";
                image.sprite = Fan;
                break;
            case 15 :
                text.text = "ID: #15\nName: Sink\nType: Water";
                image.sprite = Sink;
                break;
            case 16 :
                text.text = "ID: #16\nName: Bathtub\nType: Water";
                image.sprite = Bathtub;
                break;
            case 17 :
                text.text = "ID: #17\nName: Hose\nType: Water";
                image.sprite = Hose;
                break;
            case 18 :
                text.text = "ID: #18\nName: Dishwasher\nType: Water";
                image.sprite = Dishwasher;
                break;
            case 19 :
                text.text = "ID: #19\nName: Clothes Washer\nType: Water";
                image.sprite = ClothesWasher;
                break;
            case 20 :
                text.text = "ID: #20\nName: Stove\nType: Heat";
                image.sprite = Stove;
                break;
            case 21 :
                text.text = "ID: #21\nName: Oven\nType: Heat";
                image.sprite = Oven;
                break;
            case 22 :
                text.text = "ID: #22\nName: Microwave\nType: Heat";
                image.sprite = Microwave;
                break;
            case 23 :
                text.text = "ID: #23\nName: Toaster\nType: Heat";
                image.sprite = Toaster;
                break;
            case 24 :
                text.text = "ID: #24\nName: Toaster Oven\nType: Heat";
                image.sprite = ToasterOven;
                break;
            case 25 :
                text.text = "ID: #25\nName: Dryer\nType: Heat";
                image.sprite = Dryer;
                break;
            case 26 :
                text.text = "ID: #26\nName: Furnace\nType: Heat";
                image.sprite = Furnace;
                break;
            case 27 :
                text.text = "ID: #27\nName: Mini Fridge\nType: Cold";
                image.sprite = MiniFridge;
                break;
            case 28 :
                text.text = "ID: #28\nName: Freezer Fridge\nType: Cold";
                image.sprite = FreezerFridge;
                break;
            case 29 :
                text.text = "ID: #29\nName: Double Door Fridge\nType: Cold";
                image.sprite = DoubleDoorFridge;
                break;
            case 30 :
                text.text = "ID: #30\nName: Top Loading Freezer\nType: Cold";
                image.sprite = TopLoadingFreezer;
                break;
            case 31 :
                text.text = "ID: #31\nName: Air Conditioner\nType: Cold";
                image.sprite = AirConditioner;
                break;
            case 32 :
                text.text = "ID: #32\nName: Log\nType: Wood";
                image.sprite = Log;
                break;
            case 33 :
                text.text = "ID: #33\nName: Table\nType: Wood";
                image.sprite = Table;
                break;
            case 34 :
                text.text = "ID: #34\nName: Dresser\nType: Wood";
                image.sprite = Dresser;
                break;
            case 35 :
                text.text = "ID: #35\nName: Trunk\nType: Wood";
                image.sprite = Trunk;
                break;
            case 36 :
                text.text = "ID: #36\nName: Stick\nType: Wood";
                image.sprite = Stick;
                break;
            case 37 :
                text.text = "ID: #37\nName: Rolling Pin\nType: Wood";
                image.sprite = RollingPin;
                break;
            case 38 :
                text.text = "ID: #38\nName: Dish Towel\nType: Cloth";
                image.sprite = DishTowel;
                break;
            case 39 :
                text.text = "ID: #39\nName: Pile Of Towels\nType: Cloth";
                image.sprite = Towel;
                break;
            case 40 :
                text.text = "ID: #40\nName: Sheets\nType: Cloth";
                image.sprite = Sheets;
                break;
            case 41 :
                text.text = "ID: #41\nName: Bed\nType: Cloth";
                image.sprite = Bed;
                break;
            case 42 :
                text.text = "ID: #42\nName: Cup\nType: Glass";
                image.sprite = Cup;
                break;
            case 43 :
                text.text = "ID: #43\nName: Fishbowl\nType: Glass";
                image.sprite = Fishbowl;
                break;
            case 44 :
                text.text = "ID: #44\nName: Aquarium\nType: Glass";
                image.sprite = Aquarium;
                break;
            case 45 :
                text.text = "ID: #45\nName: Bowl\nType: Glass";
                image.sprite = Bowl;
                break;
            case 46 :
                text.text = "ID: #46\nName: Bigger Bowl\nType: Glass";
                image.sprite = BiggerBowl;
                break;
            case 47 :
                text.text = "ID: #47\nName: EVEN BIGGER BOWL\nType: Glass";
                image.sprite = EvenBiggerBowl;
                break;
            case 48 :
                text.text = "ID: #48\nName: Brick\nType: Brick";
                image.sprite = Brick;
                break;
            case 49 :
                text.text = "ID: #49\nName: Fireplace\nType: Brick";
                image.sprite = Fireplace;
                break;
            case 50 :
                text.text = "ID: #50\nName: House\nType: Brick";
                image.sprite = House;
                break;
        }
    }
}