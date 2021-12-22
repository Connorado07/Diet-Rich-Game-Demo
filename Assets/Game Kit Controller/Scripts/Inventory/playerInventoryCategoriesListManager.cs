using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.Events;

public class playerInventoryCategoriesListManager : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public Color categorySelectedIconColor;
	public Color categorUnSelectedIconColor;

	[Space]
	[Header ("Category Info List Settings")]
	[Space]

	public List<categoryInfo> categoryInfoList = new List<categoryInfo> ();

	[Space]
	[Header ("Debug")]
	[Space]

	public bool categoryListPanelActive;
	public bool selectingMultipleCategories;
	public List<string> categoriesStringList = new List<string> ();

	[Space]
	[Header ("Events Settings")]
	[Space]

	public UnityEvent eventOnCategoriesPanelOpened;
	public UnityEvent eventOnCategoriesPanelClosed;

	[Space]
	[Header ("Components")]
	[Space]

	public GameObject categoryListPanel;

	public RectTransform inventoryListPanel;

	public RectTransform inventoryListPanelScaleOnCategoryOpened;

	public RectTransform inventoryListPanelScaleOnCategoryClosed;

	public inventoryManager mainInventoryManager;

	public Scrollbar inventorySlotsScrollbar;

	public RawImage combineCategoriesIcon;

	categoryInfo currentCategoryInfo;

	void Start ()
	{
		categoryListPanel.SetActive (false);
	}

	public void selectInventoryCategory (string categoryName)
	{
		if (selectingMultipleCategories) {
			for (int i = 0; i < categoryInfoList.Count; i++) {
				currentCategoryInfo = categoryInfoList [i];

				if (currentCategoryInfo.Name == categoryName) {
					currentCategoryInfo.categorySelected = !currentCategoryInfo.categorySelected;

					currentCategoryInfo.categoryLowerIcon.SetActive (currentCategoryInfo.categorySelected);

					if (currentCategoryInfo.categorySelected) {
						currentCategoryInfo.categoryButtonIcon.color = categorySelectedIconColor;

						if (!categoriesStringList.Contains (categoryName)) {
							categoriesStringList.Add (categoryName);
						}
					} else {
						currentCategoryInfo.categoryButtonIcon.color = categorUnSelectedIconColor;

						if (categoriesStringList.Contains (categoryName)) {
							categoriesStringList.Remove (categoryName);
						}
					}
				}
			}

			for (int i = 0; i < mainInventoryManager.inventoryList.Count; i++) {
				if (categoriesStringList.Contains (mainInventoryManager.inventoryList [i].categoryName)) {
					mainInventoryManager.inventoryList [i].button.gameObject.SetActive (true);
				} else {
					mainInventoryManager.inventoryList [i].button.gameObject.SetActive (false);
				}
			}
		} else {
			for (int i = 0; i < mainInventoryManager.inventoryList.Count; i++) {
				if (mainInventoryManager.inventoryList [i].categoryName == categoryName) {
					mainInventoryManager.inventoryList [i].button.gameObject.SetActive (true);
				} else {
					mainInventoryManager.inventoryList [i].button.gameObject.SetActive (false);
				}
			}

			for (int i = 0; i < categoryInfoList.Count; i++) {
				currentCategoryInfo = categoryInfoList [i];

				if (currentCategoryInfo.Name == categoryName) {
					if (!currentCategoryInfo.categorySelected) {
						currentCategoryInfo.categorySelected = true;

						currentCategoryInfo.categoryLowerIcon.SetActive (currentCategoryInfo.categorySelected);

						if (currentCategoryInfo.categorySelected) {
							currentCategoryInfo.categoryButtonIcon.color = categorySelectedIconColor;
						} else {
							currentCategoryInfo.categoryButtonIcon.color = categorUnSelectedIconColor;
						}
					}
				} else {
					currentCategoryInfo.categorySelected = false;

					currentCategoryInfo.categoryLowerIcon.SetActive (false);

					currentCategoryInfo.categoryButtonIcon.color = categorUnSelectedIconColor;
				}
			}

			categoriesStringList.Clear ();
		}

		resetScroll ();

		mainInventoryManager.setInventoryOptionsOnSlotPanelActiveState (false);
	}

	public void enableOrDisableCategoryListPanel ()
	{
		categoryListPanelActive = !categoryListPanelActive;

		updateInventoryListPanelPosition ();
	}

	public void enableCategoryListPanel ()
	{
		categoryListPanelActive = true;

		updateInventoryListPanelPosition ();
	}

	public void disableCategoryListPanel ()
	{
		categoryListPanelActive = false;

		updateInventoryListPanelPosition ();
	}

	public void updateInventoryListPanelPosition ()
	{
		if (categoryListPanelActive) {
			eventOnCategoriesPanelOpened.Invoke ();
		} else {
			eventOnCategoriesPanelClosed.Invoke ();
		}
	
		categoryListPanel.SetActive (categoryListPanelActive);

		if (categoryListPanelActive) {

			inventoryListPanel.anchoredPosition = inventoryListPanelScaleOnCategoryOpened.anchoredPosition;

			inventoryListPanel.sizeDelta = inventoryListPanelScaleOnCategoryOpened.sizeDelta;
		} else {
			inventoryListPanel.anchoredPosition = inventoryListPanelScaleOnCategoryClosed.anchoredPosition;

			inventoryListPanel.sizeDelta = inventoryListPanelScaleOnCategoryClosed.sizeDelta;

			enableAllCategories ();

			if (selectingMultipleCategories) {
				enableOrDisableSelectingMultipleCategories ();
			} else {
				disableAllCategoriesIcons ();
			}

			categoriesStringList.Clear ();
		}

		resetScroll ();

		mainInventoryManager.setInventoryOptionsOnSlotPanelActiveState (false);
	}

	public void enableAllCategories ()
	{
		for (int i = 0; i < mainInventoryManager.inventoryList.Count; i++) {
			mainInventoryManager.inventoryList [i].button.gameObject.SetActive (true);
		}

		mainInventoryManager.setInventoryOptionsOnSlotPanelActiveState (false);
	}

	public void enableOrDisableSelectingMultipleCategories ()
	{
		selectingMultipleCategories = !selectingMultipleCategories;

		if (selectingMultipleCategories) {
			combineCategoriesIcon.color = categorySelectedIconColor;

			for (int i = 0; i < categoryInfoList.Count; i++) {
				if (categoryInfoList [i].categorySelected) {
					if (!categoriesStringList.Contains (categoryInfoList [i].Name)) {
						categoriesStringList.Add (categoryInfoList [i].Name);
					}
				}
			}
		} else {
			combineCategoriesIcon.color = categorUnSelectedIconColor;

			if (!categoryListPanelActive) {
				disableAllCategoriesIcons ();
			}
		}

		mainInventoryManager.setInventoryOptionsOnSlotPanelActiveState (false);
	}

	public void disableAllCategoriesIcons ()
	{
		for (int i = 0; i < categoryInfoList.Count; i++) {
			categoryInfoList [i].categorySelected = false;

			categoryInfoList [i].categoryLowerIcon.SetActive (categoryInfoList [i].categorySelected);

			categoryInfoList [i].categoryButtonIcon.color = categorUnSelectedIconColor;
		}
	}

	public void resetScroll ()
	{
		StartCoroutine (resetScrollCoroutine ());
	}

	private IEnumerator resetScrollCoroutine ()
	{
		inventorySlotsScrollbar.value = 0;

		yield return null;
		inventorySlotsScrollbar.value = 1;
	}

	[System.Serializable]
	public class categoryInfo
	{
		public string Name;
		public RawImage categoryButtonIcon;
		public GameObject categoryLowerIcon;
		public bool categorySelected;
	}
}
