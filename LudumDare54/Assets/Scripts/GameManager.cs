﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private GameObject player;
	private PlayerCharacter playerScript;
	public static GameManager instance = null;

	private GameObject cameraObject;
	private AudioListener listener;

	public float maxAbilityRefreshTime = 5f;
	public bool abilityRefreshAvailable = false;

	private bool paused = false;
	private bool inCutScene = false;
	private bool victory = false;
	private bool loss = false;

	private string[] cutscenes = {"Pre-BossScene", "SecondBossScene"};

	private int nextCutsceneIndex = 0;
	private int currentCutSceneIndex;
	private AudioSource AS;
	private AudioLowPassFilter lpFilter;

	public AudioClip mainTheme;
	public AudioClip finalBossTheme;
	public AudioClip midBossTheme;

	public AudioSource oneShotAudioSource;
	public AudioClip errorPurchaseNoise;

	public AudioClip deathSound;

	public float volumeMax = 0.2f;

	private bool changeToShopMusic;

	public GameObject battleOptionMenu;
	public GameObject abilityQueueMenuObject;
	public AbilityRefreshBar abilityRefreshBar;
	private AbilityQueueMenu abilityQueueMenu;


	// public GameObject[] powerupObjects;

	// public List<Powerup> inactivePowerups = new List<Powerup>();
	// public List<Powerup> activePowerups = new List<Powerup>();

	// public List<Powerup> powerupListForUI = new List<Powerup>();

	// private EnemySpawner enemySpawner;

	private bool awaitingVictoryScreen = false;

	// public bool firstLevelUp = true
	public bool isInBattleOptionScreen = false;
	public bool isInAbilityQueueScreen = false;

	public List<GameObject> mutationDeck;

	public List<Mutation> mutationQueue;

	public List<Mutation> allPossibleMutations;

	private void Awake() {
		// Load powerups
		// LoadPowerups();
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		cameraObject = GameObject.FindWithTag("MainCamera");
		listener = cameraObject.GetComponent<AudioListener>();
		playerScript = player.GetComponent<PlayerCharacter>();
		AS = GetComponent<AudioSource>();
		lpFilter = GetComponent<AudioLowPassFilter>();
		// enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
		abilityQueueMenu = abilityQueueMenuObject.GetComponent<AbilityQueueMenu>();

	}
	
	// Update is called once per frame
	void Update () {
		// CheckForWaveChange();
		if (playerScript != null)
			CheckGameOver();

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			NextBattlePopup();
		}

		if (Input.GetKeyUp(KeyCode.E)) {
			EnableAbilityQueueMenu();
		}
	}


	private void SpawnWave(int currentLevel) {
		// spawnManager.SpawnWave(currentLevel);
	}

	public void StartCutScene(int cutSceneIndex) {
		AS.volume = volumeMax;
		ResumeMainMusic();

		Debug.Log("START SCENE INDEX: " + cutSceneIndex);
		currentCutSceneIndex = cutSceneIndex;
		SceneManager.LoadScene(cutscenes[cutSceneIndex], LoadSceneMode.Additive);
		listener.enabled = false; // Disabling the main cameras audio listener so that we have exactly one listener
		PauseGame();
	}

	public void StopCutScene() {
		Debug.Log("END SCENE INDEX: " + currentCutSceneIndex);
		SceneManager.UnloadSceneAsync(cutscenes[currentCutSceneIndex]);
		listener.enabled = true;
		// SetEnemyCountToZero();
		UnPauseGame();
	}

	public void CheckGameOver() {
		if (SceneManager.GetActiveScene().name != "GameOverScreen" && !victory){
			if (playerScript.GetHealth() <= 0f && !loss){
				// Debug.Log("PLAY GAMEOVER");
				loss = true;
				oneShotAudioSource.PlayOneShot(deathSound);
				playerScript.DisableOnDeath();
				IEnumerator coroutine = GameOverCoRoutine(1.5f);
				StartCoroutine(coroutine);
			}
		} else if(SceneManager.GetActiveScene().name != "VictoryScene" && victory && playerScript.GetHealth() <= 0f) {
			if(!awaitingVictoryScreen) {
				awaitingVictoryScreen = true;
				oneShotAudioSource.PlayOneShot(deathSound);
				playerScript.DisableOnDeath();
				IEnumerator coroutine = VictoryCoRoutine(0.5f);
				StartCoroutine(coroutine);
			}
		}
	}

	// public void GoToGameOverScreen() {
	// 	SceneManager.LoadScene("GameOverScreen", LoadSceneMode.Single);
	// }

    private IEnumerator GameOverCoRoutine(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
			SceneManager.LoadScene("GameOverScreen", LoadSceneMode.Single);
            print("WaitAndPrint " + Time.time);
        }
    }

	private IEnumerator VictoryCoRoutine(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
			if (SceneManager.GetActiveScene().name != "VictoryScene") {
				SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
			}
        }
    }

	public void Victory() {
		Debug.Log("YOU WIN");
		if (SceneManager.GetActiveScene().name != "VictoryScene") {
			victory = true;
			SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
		}

	}

	// public void TimesUp() {
	// 	victory = true;
	// 	enemySpawner.SpawnFinalBoss();
	// }

	public void PauseGame() {
		paused = true;
		Time.timeScale = 0;	
	}

	public void UnPauseGame() {
		paused = false;
		Time.timeScale = 1;
	}

	public void PlayShopMusic() {
		changeToShopMusic = true;
	}

	public void ResumeMainMusic() {
		changeToShopMusic = false;
	}

	public void PlayMainMusic() {
		AS.clip = mainTheme;
		AS.Play();
	}

	public void PlayMidBossMusic() {
		AS.clip = midBossTheme;
		AS.Play();
	}

	public void PlayFinalBossMusic() {
		AS.clip = finalBossTheme;
		AS.Play();
	}

	// public void PlayErrorNoise() {
	// 	if(changeToShopMusic) {
	// 		shopTheme.PlayOneShot(errorPurchaseNoise);
	// 	} else {
	// 		AS.PlayOneShot(errorPurchaseNoise);
	// 	}
	// }

	public void enableLowPassFilter() {
		lpFilter.enabled = true;
	}

	public void disableLowPassFilter() {
		lpFilter.enabled = false;
	}

	public PlayerCharacter GetPlayer() {
		return playerScript;
	}
	public void NextBattlePopup() {
		if (battleOptionMenu) {
			// Generate 3 level up choices;
			// List<Powerup> powerupList = new List<Powerup>(3);
			// for (int i = 0; i < 3; i++) {
			// 	bool selectedPowerup = false;
			// 	while(!selectedPowerup) {
			// 		int randomSelection = 0;
			// 		if (firstLevelUp) {
			// 			randomSelection = Random.Range(0, Mathf.CeilToInt(powerupObjects.Length/2) - 0);
			// 		} else {
			// 			randomSelection = Random.Range(0, powerupObjects.Length - 0);
			// 		}
			// 		if(!powerupList.Contains(powerupObjects[randomSelection].GetComponent<Powerup>())) {
			// 			powerupList.Add(powerupObjects[randomSelection].GetComponent<Powerup>());
			// 			selectedPowerup = true;
			// 		}
			// 	}
			// }
			PauseGame();	

			battleOptionMenu.SetActive(true);
			isInBattleOptionScreen = true;
			NextBattlePopup popUp = battleOptionMenu.GetComponent<NextBattlePopup>();
			enableLowPassFilter();
			// popUp.PopUp(powerupList);
			// TODO: Generate next battle options
			NextBattleObject[] generatedBattleOptions = new NextBattleObject[3];
			generatedBattleOptions[0] = new NextBattleObject();
			generatedBattleOptions[0].SetValues(10f, new List<GameObject>(), allPossibleMutations[0]);
			generatedBattleOptions[1] = new NextBattleObject();
			generatedBattleOptions[1].SetValues(0f, new List<GameObject>(), allPossibleMutations[0]);
			generatedBattleOptions[2] = new NextBattleObject();
			generatedBattleOptions[2].SetValues(5f, new List<GameObject>(), allPossibleMutations[0]);
			popUp.PopUp(generatedBattleOptions);
		} else {
			Debug.Log("No Battle Option menu set in scene, cant open menu...");
		}
	}

	public void SetNextBattle(NextBattleObject battleObject) {
		Debug.Log("TODO: Setting next battle: " + battleObject.amountOfHealthRegained);

		UnPauseGame();
		// TODO: Add the mutation selection to current deck;
		// TODO: use select battle object to do something
	}

	public void EnableAbilityQueueMenu() {
		if (abilityRefreshAvailable && !isInBattleOptionScreen) {
			abilityQueueMenuObject.SetActive(true);
			abilityQueueMenu.AddRandomAbilitiesToSelection();
			isInAbilityQueueScreen = true;
			enableLowPassFilter();
			PauseGame();
		} else {
			// TODO: Add error sound or something
		}
	}

	public void DisableAbilityQueueMenu() {
		abilityQueueMenuObject.SetActive(false);
		disableLowPassFilter();
		UnPauseGame();
	}

	public void ResetAbilityCountdown() {
		abilityRefreshAvailable = false;
		abilityRefreshBar.resetAbilityCountdown();
	}

	// public void DecreaseEnemyCount() {
	// 	if (enemyCount > 0)
	// 		enemyCount--;
	// }

	// public void DecreaseBossCount() {
	// 	if (bossCount > 0)
	// 		bossCount--;
	// }

	// //Used By cutscenes to indicate they are done
	// public void SetEnemyCountToZero() {
	// 	enemyCount = 0;
	// }
	
	//Utility Methods
	
	// -1 indicates the integer is not in the array
	private int FindInArray(int[] arr, int find) {
		for (int i = 0; i < arr.Length; i++) {
			if (find == arr[i])
				return i;
		}
		return -1;
	}

	// 1 - intro screen
	// 2 - start of gameplay
	public void Reset() {
		Debug.Log("RESET");
		victory = false;
		loss = false;
		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}

	public void LoadScene(int sceneIndex) {
		Debug.Log("SCENE INDEX " + sceneIndex);
		if (sceneIndex == 0) {
			SceneManager.LoadScene(sceneIndex);
			Destroy(gameObject);
		}
		// else if (sceneIndex == 2) {
		// 	Reset();
		// }
		else {
			SceneManager.LoadScene(sceneIndex);
		}
	}

	public bool IsPaused () {
		return paused;
	}

	// Retrieves game objects and adds them to the inactive pool
	// public void LoadPowerups() {
	// 	foreach (GameObject g in powerupObjects) {
	// 		Powerup powerup = g.GetComponent<Powerup>();
	// 		if (powerup != null) {
	// 			if (!powerup.active)	inactivePowerups.Add(powerup);
	// 			else {
	// 				activePowerups.Add(powerup);
	// 				if(!powerup.isUtilityPowerup()) {
	// 					powerupListForUI.Add(powerup);
	// 				}
	// 			} 
	// 		}
	// 	}
	// }

	// public void SetPowerupToActive(Powerup powerup) {
	// 	powerup.SetPowerupActive();
	// 	inactivePowerups.Remove(powerup);
	// 	if(!activePowerups.Contains(powerup)) {
	// 		activePowerups.Add(powerup);
	// 		if(!powerup.isUtilityPowerup()) {
	// 			powerupListForUI.Add(powerup);
	// 		}
	// 	}
	// }

	// public void SetPowerupToInactive(Powerup powerup) {
	// 	powerup.SetPowerupInActive();
	// 	activePowerups.Remove(powerup);
	// 	if(!inactivePowerups.Contains(powerup)) {
	// 		inactivePowerups.Add(powerup);
	// 	}
	// }
}
