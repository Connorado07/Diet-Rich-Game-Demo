using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryQuickAccessSlotElement : MonoBehaviour
{
	public quickAccessSlotInfo mainQuickAccessSlotInfo;

	[System.Serializable]
	public class quickAccessSlotInfo
	{
		public string Name;
		public bool slotActive;
		public GameObject slot;

		public Text amountText;
		public RawImage slotIcon;

		public RawImage rightSecondarySlotIcon;
		public RawImage leftSecondarySlotIcon;

		public bool secondarySlotActive;

		public string firstElementName;
		public string secondElementName;

		public Text iconNumberKeyText;

		public Transform slotSelectedIconPosition;

		public GameObject currentlySelectedIcon;

		public Image backgroundImage;

		public GameObject slotMainSingleContent;
		public GameObject slotMainDualContent;

		public GameObject amountTextContent;

		public inventoryInfo inventoryInfoAssigned;
	}
}