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
			choice1.SetBattleObject(battleObjectList[0]);
			choice1.setMutation(battleObjectList[0].mutation);
			choice1.setTitle("Health regained: " + battleObjectList[0].amountOfHealthRegained);
			// TODO: loop over enemy list and put it in the choice selection
			
			string text = "";
			Dictionary<string, int> totalCount = new Dictionary<string, int>();

			foreach (GameObject enemy in battleObjectList[0].enemies) {
				string enemyName = enemy.GetComponent<EnemyAi>().enemyName;
				if (!totalCount.ContainsKey(enemyName)) {
					totalCount.TryAdd(enemyName, 1);
				} else {
					totalCount[enemyName] = totalCount[enemyName] + 1;
				}
			}

			foreach (var item in totalCount)
			{
				string name = item.Key;
				int count = item.Value;
				text += "x" + count + " " + name + "\n";
			}

			choice1.setContent(text);
		}

		if (battleObjectList[1] != null) {
			choice2.SetBattleObject(battleObjectList[1]);
			choice2.setMutation(battleObjectList[1].mutation);
			choice2.setTitle("Health regained: " + battleObjectList[1].amountOfHealthRegained);
			string text = "";
			Dictionary<string, int> totalCount = new Dictionary<string, int>();

			foreach (GameObject enemy in battleObjectList[1].enemies) {
				string enemyName = enemy.GetComponent<EnemyAi>().enemyName;
				if (!totalCount.ContainsKey(enemyName)) {
					totalCount.TryAdd(enemyName, 1);
				} else {
					totalCount[enemyName] = totalCount[enemyName] + 1;
				}
			}

			foreach (var item in totalCount)
			{
				string name = item.Key;
				int count = item.Value;
				text += "x" + count + " " + name + "\n";
			}
			choice2.setContent(text);
		}

		if (battleObjectList[2] != null) {
			choice3.SetBattleObject(battleObjectList[2]);
			choice3.setMutation(battleObjectList[2].mutation);
			choice3.setTitle("Health regained: " + battleObjectList[2].amountOfHealthRegained);
			string text = "";
			Dictionary<string, int> totalCount = new Dictionary<string, int>();

			foreach (GameObject enemy in battleObjectList[2].enemies) {
				string enemyName = enemy.GetComponent<EnemyAi>().enemyName;
				if (!totalCount.ContainsKey(enemyName)) {
					totalCount.TryAdd(enemyName, 1);
				} else {
					totalCount[enemyName] = totalCount[enemyName] + 1;
				}
			}

			foreach (var item in totalCount)
			{
				string name = item.Key;
				int count = item.Value;
				text += "x" + count + " " + name + "\n";
			}
			choice3.setContent(text);
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
