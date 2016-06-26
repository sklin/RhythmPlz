using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public MenuController currentMenu;
	public MenuController[] menus;

	void Start() {
		ShowMenu (currentMenu);
	}

	public void ShowMenu(MenuController menu) {
		if (currentMenu != null) {
			currentMenu.isOpen = false;
			
		}
		currentMenu = menu;
		currentMenu.isOpen = true;
	}

	public void HideMenu() {
		currentMenu.isOpen = false;
	}

	public void UnhideMenu() {
		currentMenu.isOpen = true;
	}

}
