using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public event Action<int> onMenuSelected;
    public event Action onBack;

    List<Text> menuItems; 

    int selectedItem = 0;

    private void Awake() {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }
    
    public void OpenMenu() {
        menu.SetActive(true);
        UpdateItemSelection();
    }

    public void CloseMenu() {
        menu.SetActive(false);
    }

    public void HandleUpdate() {
        int prevSelection = selectedItem;
        
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            ++selectedItem;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            --selectedItem;
        }
        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);
        if (prevSelection != selectedItem) {
            UpdateItemSelection();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            onMenuSelected?.Invoke(selectedItem);
            selectedItem = 0;
            CloseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            onBack?.Invoke();
            selectedItem = 0;
            CloseMenu();
        }

    }

    void UpdateItemSelection() {
        for (int i = 0; i < menuItems.Count; i++) {
            if (i == selectedItem) {
                menuItems[i].color = Color.blue;
            }
            else {
                menuItems[i].color = Color.black;
            }
        }
    }
}
