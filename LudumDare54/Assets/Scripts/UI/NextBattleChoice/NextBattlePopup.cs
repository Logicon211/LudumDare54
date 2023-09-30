using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextBattlePopup : MonoBehaviour
{

	public NextBattleChoice choice1;
	public NextBattleChoice choice2;
	public NextBattleChoice choice3;

	public MutationButton mutationButton;

	private GameManager gameManager;
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	public void PopUp (NextBattleObject[] battleObjectList) {
		if (battleObjectList[0] != null) {
			choice1.setMutation(battleObjectList[0].mutation);
			choice1.setTitle("Health gained: " + battleObjectList[0].amountOfHealthRegained);
			// TODO: loop over enemy list and put it in the choice selection
			choice1.setContent("battleObjectList[0].enemies[0].name or whatever");
			// choice1.setContent(battleObjectList[0].GetPowerupLevelDescription(battleObjectList[0].GetLevel()+1));
			// choice1.setIcon(battleObjectList[0].getPowerupIcon());
		}

		if (battleObjectList[1] != null) {
			choice2.setMutation(battleObjectList[1].mutation);
			choice2.setTitle("Health gained: " + battleObjectList[1].amountOfHealthRegained);
			choice1.setContent("battleObjectList[1].enemies[0].name or whatever");
			// choice2.setContent(battleObjectList[1].GetPowerupLevelDescription(battleObjectList[1].GetLevel()+1));
			// choice2.setIcon(battleObjectList[1].getPowerupIcon());
		}

		if (battleObjectList[2] != null) {
			choice3.setMutation(battleObjectList[2].mutation);
			choice3.setTitle("Health gained: " + battleObjectList[2].amountOfHealthRegained);
			choice1.setContent("battleObjectList[2].enemies[0].name or whatever");
			// choice3.setContent(battleObjectList[2].GetPowerupLevelDescription(battleObjectList[2].GetLevel()+1));
			// choice3.setIcon(battleObjectList[2].getPowerupIcon());
		}

		// Time.timeScale = 0f;
	}

	public void ClosePopUp() {
		choice1.clearMutationButton();
		choice2.clearMutationButton();
		choice3.clearMutationButton();
		gameObject.SetActive(false);
		gameManager.disableLowPassFilter();
		gameManager.isInBattleOptionScreen = false;
	}

}
