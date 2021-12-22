using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAbilitiesSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool abilitiesSystemEnabled = true;

	public bool abilitesModeActive;

	public int currentAbilityIndex;

	public string energyStatName = "Current Energy";

	public bool disableAbilitySystemOnFirstPerson;

	[Space]
	[Header ("Abilities List")]
	[Space]

	public List<abilityInfo> abilityInfoList = new List<abilityInfo> ();

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public bool canMove;
	public bool playerCurrentlyBusy;
	public abilityInfo currentAbilityInfo;
	public bool cancellingCurrentAbilityPress;
	public string currentAbilityName;

	public string previousAbilityActiveName;

	public bool abilityInputInUse;

	public bool pauseCheckUsingDeviceOnAbilityInput;

	[Space]
	[Header ("Abilities Components")]
	[Space]

	public playerController mainPlayerController;
	public playerStatsSystem mainPlayerStatsSystem;
	public playerAbilitiesUISystem mainPlayerAbilitiesUISystem;

	int energyStatIndex;

	void Start ()
	{
		if (abilitiesSystemEnabled) {
			energyStatIndex = mainPlayerStatsSystem.getStatValueIndex (energyStatName);

			bool setFirstAbilityAvailable = false;

			if (currentAbilityIndex < abilityInfoList.Count && currentAbilityIndex > 0) {
				if (abilityInfoList [currentAbilityIndex].abilityEnabled) {
					setCurrentAbilityByName (abilityInfoList [currentAbilityIndex].Name);
				} else {
					setFirstAbilityAvailable = true;
				}
			} else {
				setFirstAbilityAvailable = true;
			}

			if (setFirstAbilityAvailable) {
				int abilityInfoListCount = abilityInfoList.Count;

				for (int i = 0; i < abilityInfoListCount; i++) {
					abilityInfo abilityToCheck = abilityInfoList [i];

					if (abilityToCheck.abilityEnabled) {
						setCurrentAbilityByName (abilityToCheck.Name);

						return;
					}
				}
			}
		}
	}

	void Update ()
	{
		if (abilitiesSystemEnabled) {
			canMove = !mainPlayerController.isPlayerDead () && mainPlayerController.canPlayerMove ();
		}
	}

	public void setCurrentAbilityByName (string abilityName)
	{
		if (currentAbilityInfo != null) {
			if (abilityName.Equals (currentAbilityName)) {
				return;
			}

			if (currentAbilityInfo.avoidToUseOtherAbilitiesWhileLimitActive && currentAbilityInfo.timeLimitInProcess) {
				if (showDebugPrint) {
					print ("ability in process, can't user others in the meantime");
				}

				return;
			}
		}

		setCurrentAbilityByNameWithoutChecks (abilityName);
	}

	void setCurrentAbilityByNameWithoutChecks (string abilityName)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			if (abilityToCheck.abilityEnabled) {
				if (abilityToCheck.Name.Equals (abilityName)) {

					currentAbilityInfo = abilityToCheck;

					currentAbilityInfo.isCurrentAbility = true;

					currentAbilityInfo.activateUpdateAbility ();

					currentAbilityInfo.enableAbility ();

					currentAbilityInfo.checkEventOnSetCurrentAbility ();

					currentAbilityIndex = i;

					currentAbilityName = currentAbilityInfo.Name;
				} else {
					if (abilityToCheck.deactivateAbilityWhenChangeToAnother) {
						if (abilityToCheck.isCurrentAbility) {
							abilityToCheck.deactivateAbility ();

							abilityToCheck.checkEventToDeactivateAbility ();
						}

						abilityToCheck.stopActivateUpdateAbility ();

						abilityToCheck.disableAbilityCurrentActiveFromPressState ();
					}

					abilityToCheck.isCurrentAbility = false;
				}
			}
		}
	}

	public void removeCurrentAbilityInfo ()
	{
		currentAbilityInfo = null;
		currentAbilityIndex = -1;
	}

	public void disableAbilityGroupByName (List<string> abilityNameList)
	{
		enableOrDisableAbilityGroupByName (abilityNameList, false);
	}

	public void enableAbilityGroupByName (List<string> abilityNameList)
	{
		enableOrDisableAbilityGroupByName (abilityNameList, true);
	}

	public void enableOrDisableAbilityGroupByName (List<string> abilityNameList, bool state)
	{
		for (int i = 0; i < abilityNameList.Count; i++) {
			if (state) {
				enableAbilityByName (abilityNameList [i]);
			} else {
				disableAbilityByName (abilityNameList [i]);
			}
		}
	}

	public void enableAbilityByName (string abilityName)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			if (abilityToCheck.Name.Equals (abilityName)) {
				abilityToCheck.abilityEnabled = true;

				abilityToCheck.enableAbility ();

				return;
			}
		}
	}

	public void disableAbilityByName (string abilityName)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			if (abilityToCheck.Name.Equals (abilityName)) {
				abilityToCheck.abilityEnabled = false;

				abilityToCheck.disableAbility ();

				abilityToCheck.isCurrentAbility = false;
				abilityToCheck.stopActivateUpdateAbility ();

				abilityToCheck.disableAbilityCurrentActiveFromPressState ();

				return;
			}
		}
	}

	public void disableAllAbilites ()
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			abilityToCheck.abilityEnabled = false;

			abilityToCheck.disableAbility ();

			abilityToCheck.isCurrentAbility = false;

			abilityToCheck.stopActivateUpdateAbility ();

			abilityToCheck.disableAbilityCurrentActiveFromPressState ();
		}

		previousAbilityActiveName = "";
	}

	public void deactivateAllAbilites ()
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			abilityToCheck.deactivateAbility ();

			abilityToCheck.isCurrentAbility = false;
			abilityToCheck.stopActivateUpdateAbility ();

			abilityToCheck.disableAbilityCurrentActiveFromPressState ();

			if (abilityToCheck.timeLimitInProcess) {
				if (abilityToCheck.useCoolDown) {
					abilityToCheck.activateCoolDown ();
				} else {
					abilityToCheck.stopActivateTimeLimit ();
				}
			}

			abilityToCheck.checkEventToDeactivateAbility ();
		}
	}

	public void deactivateAbilityByName (string abilityName)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			if (abilityToCheck.Name.Equals (abilityName)) {

				abilityToCheck.deactivateAbility ();

				abilityToCheck.isCurrentAbility = false;
				abilityToCheck.stopActivateUpdateAbility ();

				abilityToCheck.disableAbilityCurrentActiveFromPressState ();

				if (abilityToCheck.timeLimitInProcess) {
					if (abilityToCheck.useCoolDown) {
						abilityToCheck.activateCoolDown ();
					} else {
						abilityToCheck.stopActivateTimeLimit ();
					}
				}

				abilityToCheck.checkEventToDeactivateAbility ();

				return;
			}
		}
	}

	public void setPauseCheckUsingDeviceOnAbilityInputState (bool state)
	{
		pauseCheckUsingDeviceOnAbilityInput = state;
	}

	public void togglePauseCheckUsingDeviceOnAbilityInputState ()
	{
		setPauseCheckUsingDeviceOnAbilityInputState (!pauseCheckUsingDeviceOnAbilityInput);
	}

	public bool playerIsBusy ()
	{
		if ((pauseCheckUsingDeviceOnAbilityInput || !mainPlayerController.isUsingDevice ()) &&
		    !mainPlayerController.isUsingSubMenu () &&
		    !mainPlayerController.isPlayerMenuActive () &&
		    !mainPlayerController.isActionActive ()) {

			return false;
		}

		return true;
	}

	public bool checkIfAbilityCanBeActivated ()
	{
		if (currentAbilityInfo != null) {
			if (!currentAbilityInfo.canBeUsedOnlyOnGround || mainPlayerController.isPlayerOnGround ()) {
				return true;
			}
		}

		return false;
	}

	public int getNumberOfAbilitiesAvailable ()
	{
		int numberOfAbilitiesAvailable = 0;

		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			if (abilityInfoList [i].abilityEnabled) {
				numberOfAbilitiesAvailable++;
			}
		}

		return numberOfAbilitiesAvailable;
	}

	public int getNumberOfAbilitiesCanBeShownOnWheelSelection ()
	{
		int numberOfAbilitiesAvailable = 0;

		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			if (abilityInfoList [i].abilityCanBeShownOnWheelSelection) {
				numberOfAbilitiesAvailable++;
			}
		}

		return numberOfAbilitiesAvailable;
	}

	public int getCurrentAbilityIndex ()
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			if (abilityInfoList [i].isCurrentAbility) {
				return i;
			}
		}

		return -1;
	}

	public bool checkIfAbilitiesAvailable ()
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			if (abilityInfoList [i].abilityEnabled) {
				return true;
			}
		}

		return false;
	}

	public abilityInfo getAbilityByName (string abilityName)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfo abilityToCheck = abilityInfoList [i];

			if (abilityToCheck.Name.Equals (abilityName)) {

				return abilityToCheck;
			}
		}

		return null;
	}

	public bool isAbilityInputInUse ()
	{
		return abilityInputInUse;
	}

	public bool isAbilitesModeActive ()
	{
		return abilitesModeActive;
	}

	public void setAbilitesModeActiveState (bool state)
	{
		abilitesModeActive = state;
	}

	public void usePowerBar (float powerAmount)
	{
		mainPlayerStatsSystem.usePlayerStatByIndex (energyStatIndex, powerAmount);
	}

	public bool isThereEnergyAvailable ()
	{
		return mainPlayerStatsSystem.getStatValueByIndex (energyStatIndex) > 0;
	}

	public bool checkIfAbilityNeedsEnergy ()
	{
		if (!currentAbilityInfo.useEnergyOnAbility) {
			return true;
		} else {
			float currentEnergyAmount = mainPlayerStatsSystem.getStatValueByIndex (energyStatIndex);

			if (currentEnergyAmount >= currentAbilityInfo.energyAmountUsed) {
				if (currentAbilityInfo.useEnergyWithRate) {
					return true;
				} else {
					return true;
				}
			}
		}
			
		return false;
	}

	public void checkAbilityUseEnergy ()
	{
		if (currentAbilityInfo.useEnergyOnAbility && !currentAbilityInfo.useEnergyWithRate) {
			usePowerBar (currentAbilityInfo.energyAmountUsed);
		}
	}

	public void checkAbilityUseEnergyInUpdate ()
	{
		if (currentAbilityInfo.useEnergyOnAbility && currentAbilityInfo.useEnergyWithRate) {
			if (isThereEnergyAvailable ()) {
				usePowerBar (currentAbilityInfo.energyAmountUsed);
			} else {
				currentAbilityInfo.deactivateAbility ();
			}
		}
	}

	public void cancelPressAbility ()
	{
		cancellingCurrentAbilityPress = true;
	}

	public bool checkIfAbilitiesOnUseOrCooldown (string abilityName)
	{
		int abilityIndex = abilityInfoList.FindIndex (a => a.Name == abilityName);

		if (abilityIndex > -1) {
			abilityInfo abilityToCheck = abilityInfoList [abilityIndex];

			if (abilityToCheck.useCoolDown && abilityToCheck.coolDownInProcess) {
				return true;
			}

			if (abilityToCheck.useTimeLimit && abilityToCheck.timeLimitInProcess) {
				return true;
			}
		}

		return false;
	}

	//Activate abilities externally without including the ability on the wheel menu, so it can be used separately
	public void inputSelectAndPressDownNewAbility (string abilityName)
	{
		setCurrentAbilityByName (abilityName);

		inputPressDownUseCurrentAbility ();
	}

	public void inputSelectAndPressDownNewAbilityTemporally (string abilityName, bool abilityIsTemporallyActivated)
	{
		if (abilityIsTemporallyActivated) {
			inputSelectAndPressDownNewSeparatedAbility (abilityName);

			checkPreviousAbilityActive ();
		} else {
			inputSelectAndPressDownNewAbility (abilityName);
		}
	}

	public void inputSelectAndPressDownNewSeparatedAbility (string abilityName)
	{
		if (currentAbilityInfo != null) {
			if (abilityName != previousAbilityActiveName) {
				previousAbilityActiveName = currentAbilityInfo.Name;
			}
		}

		inputSelectAndPressDownNewAbility (abilityName);
	}

	public void inputSelectAndPressHoldNewSeparatedAbility ()
	{
		inputPressHoldUseCurrentAbility ();
	}

	public void inputSelectAndPressUpNewSeparatedAbility ()
	{
		inputPressUpUseCurrentAbility ();

		checkPreviousAbilityActive ();
	}

	public void checkPreviousAbilityActive ()
	{
		if (previousAbilityActiveName != "") {
			setCurrentAbilityByNameWithoutChecks (previousAbilityActiveName);

			previousAbilityActiveName = "";
		}
	}


	//Main ability input functions
	public void inputPressDownUseCurrentAbility ()
	{
		abilityInputInUse = true;

		pressDownUseCurrentAbility ();
	}

	public void pressDownUseCurrentAbility ()
	{
		if (!abilitiesSystemEnabled) {
			return;
		}

		if (disableAbilitySystemOnFirstPerson && mainPlayerController.isPlayerOnFirstPerson ()) {
			return;
		}

		if (cancellingCurrentAbilityPress) {
			return;
		}

		if (currentAbilityInfo == null) {
			return;
		}

		playerCurrentlyBusy = playerIsBusy ();

		if (playerCurrentlyBusy) {
			return;
		}

		if (!currentAbilityInfo.useInputOnPressDown) {
			return;
		}

		if (!checkIfAbilityCanBeActivated ()) {
			return;
		}

		if (currentAbilityInfo.useCoolDown && currentAbilityInfo.coolDownInProcess) {
			return;
		}

		if (currentAbilityInfo.useTimeLimit && currentAbilityInfo.timeLimitInProcess && currentAbilityInfo.avoidAbilityInputWhileLimitActive) {
			return;
		}

		if (!checkIfAbilityNeedsEnergy () && !currentAbilityInfo.abilityCurrentlyActiveFromPressDown) {
			return;
		}

		if (mainPlayerAbilitiesUISystem.isSelectingAbilityActive ()) {
			return;
		}

		if (!currentAbilityInfo.abilityCurrentlyActiveFromPressDown || !currentAbilityInfo.useEnergyOnceOnPressDown) {
			if (currentAbilityInfo.useEnergyOnPressDown) {
				checkAbilityUseEnergy ();
			}
		}

		currentAbilityInfo.abilityCurrentlyActiveFromPressDown = !currentAbilityInfo.abilityCurrentlyActiveFromPressDown;
			
		currentAbilityInfo.useAbilityPressDown ();

		if (currentAbilityInfo.disableAbilityInputInUseStateOnPressDown) {
			abilityInputInUse = false;
		}

		if (currentAbilityInfo.useTimeLimit && currentAbilityInfo.useTimeLimitOnPressDown) {
			if (currentAbilityInfo.abilityCurrentlyActiveFromPressDown) {
				if (currentAbilityInfo.useLimitWhenAbilityCurrentActiveFromPress) {
					currentAbilityInfo.activateTimeLimit ();
				}
			} else {
				if (!currentAbilityInfo.useLimitWhenAbilityCurrentActiveFromPress) {
					currentAbilityInfo.activateTimeLimit ();
				}
			}
		}

		if (currentAbilityInfo.useCoolDown && currentAbilityInfo.useCoolDownOnPressDown) {
			if (currentAbilityInfo.abilityCurrentlyActiveFromPressDown) {
				if (currentAbilityInfo.useCoolDownWhenAbilityCurrentlyActiveFromPress) {
					currentAbilityInfo.activateCoolDown ();
				}
			} else {
				if (!currentAbilityInfo.useCoolDownWhenAbilityCurrentlyActiveFromPress) {
					currentAbilityInfo.activateCoolDown ();
				}
			}
		}
	}

	public void inputPressHoldUseCurrentAbility ()
	{
		pressHoldUseCurrentAbility ();
	}

	public void pressHoldUseCurrentAbility ()
	{
		if (!abilitiesSystemEnabled) {
			return;
		}

		if (disableAbilitySystemOnFirstPerson && mainPlayerController.isPlayerOnFirstPerson ()) {
			return;
		}

		if (cancellingCurrentAbilityPress) {
			return;
		}

		if (currentAbilityInfo == null) {
			return;
		}

		playerCurrentlyBusy = playerIsBusy ();

		if (playerCurrentlyBusy) {
			return;
		}

		if (!currentAbilityInfo.useInputOnPressHold) {
			return;
		}

		if (!checkIfAbilityCanBeActivated ()) {
			return;
		}

		if (!checkIfAbilityNeedsEnergy ()) {
			return;
		}

		if (mainPlayerAbilitiesUISystem.isSelectingAbilityActive ()) {
			return;
		}
			
		if (currentAbilityInfo.useEnergyOnPressHold) {
			checkAbilityUseEnergy ();
		}

		currentAbilityInfo.useAbilityPressHold ();
	}

	public void inputPressUpUseCurrentAbility ()
	{
		abilityInputInUse = false;

		pressUpUseCurrentAbility ();
	}

	public void pressUpUseCurrentAbility ()
	{
		if (!abilitiesSystemEnabled) {
			return;
		}

		if (disableAbilitySystemOnFirstPerson && mainPlayerController.isPlayerOnFirstPerson ()) {
			return;
		}

		if (cancellingCurrentAbilityPress) {
			cancellingCurrentAbilityPress = false;
			return;
		}

		if (currentAbilityInfo == null) {
			return;
		}

		playerCurrentlyBusy = playerIsBusy ();

		if (playerCurrentlyBusy) {
			return;
		}

		if (!currentAbilityInfo.useInputOnPressUp) {
			return;
		}

		if (currentAbilityInfo.checkIfInputPressDownBeforeActivateUp && !currentAbilityInfo.abilityCurrentlyActiveFromPressDown) {
			return;
		}

		if (currentAbilityInfo.useCoolDown && currentAbilityInfo.coolDownInProcess) {
			return;
		}

		if (currentAbilityInfo.useTimeLimit && currentAbilityInfo.timeLimitInProcess && currentAbilityInfo.avoidAbilityInputWhileLimitActive) {
			return;
		}

		if (!checkIfAbilityNeedsEnergy () && !currentAbilityInfo.abilityCurrentlyActiveFromPressUp) {
			return;
		}

		if (mainPlayerAbilitiesUISystem.isSelectingAbilityActive ()) {
			return;
		}

		if (currentAbilityInfo.abilityCurrentlyActiveFromPressUp || !currentAbilityInfo.useEnergyOnceOnPressUp) {
			if (currentAbilityInfo.useEnergyOnPressUp) {
				checkAbilityUseEnergy ();
			}
		}

		currentAbilityInfo.abilityCurrentlyActiveFromPressUp = !currentAbilityInfo.abilityCurrentlyActiveFromPressUp;

		currentAbilityInfo.useAbilityPressUp ();

		if (currentAbilityInfo.useTimeLimit && currentAbilityInfo.useTimeLimitOnPressUp) {
			if (currentAbilityInfo.abilityCurrentlyActiveFromPressUp) {
				if (currentAbilityInfo.useLimitWhenAbilityCurrentActiveFromPress) {
					currentAbilityInfo.activateTimeLimit ();
				}
			} else {
				if (!currentAbilityInfo.useLimitWhenAbilityCurrentActiveFromPress) {
					currentAbilityInfo.activateTimeLimit ();
				}
			}
		}

		if (currentAbilityInfo.useCoolDown && currentAbilityInfo.useCoolDownOnPressUp) {
			if (currentAbilityInfo.abilityCurrentlyActiveFromPressUp) {
				if (currentAbilityInfo.useCoolDownWhenAbilityCurrentlyActiveFromPress) {
					currentAbilityInfo.activateCoolDown ();
				}
			} else {
				if (!currentAbilityInfo.useCoolDownWhenAbilityCurrentlyActiveFromPress) {
					currentAbilityInfo.activateCoolDown ();
				}
			}
		}
	}

	public void setAbilitiesSystemEnabledState (bool state)
	{
		abilitiesSystemEnabled = state;
	}

	public void enableOrDisableAllAbilitiesFromEditor (bool state)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfoList [i].enableOrDisableAbilityFromEditor (state);
		}

		updateComponent ();
	}

	public void enableOrDisableAllAbilitiesCanBeShownOnWheelFromEditor (bool state)
	{
		int abilityInfoListCount = abilityInfoList.Count;

		for (int i = 0; i < abilityInfoListCount; i++) {
			abilityInfoList [i].enableOrDisableAllAbilitiesCanBeShownOnWheelFromEditor (state);
		}

		updateComponent ();
	}

	void updateComponent ()
	{
		GKC_Utils.updateComponent (this);

		GKC_Utils.updateDirtyScene ("Update All Abilities Enabled State", gameObject);
	}
}
