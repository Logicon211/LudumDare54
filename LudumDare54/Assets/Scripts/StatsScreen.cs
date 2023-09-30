using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsScreen : MonoBehaviour
{
    private GameManager gameManager;
    public TMP_Text passiveStats;
    public GameObject[] iconAndContainers = new GameObject[5];
    public TMP_Text[] iconTexts = new TMP_Text[5];

    public struct MutationTuple
    {
        public Mutation mutation;
        public int count;

        // Constructor to initialize the tuple
        public MutationTuple(Mutation obj, int value)
        {
            mutation = obj;
            count = value;
        }
    }


	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}

    // Update is called once per frame
    void Update()
    {
        // float damagePercentage = Mathf.FloorToInt((gameManager.GetPlayer().playerStats.damagePercentBonus - 1f) * 100f);
        // float cooldownPercentage = Mathf.FloorToInt((gameManager.GetPlayer().playerStats.cooldownPercentBonus - 1f) * 100f);
        // float speedPercentage = Mathf.FloorToInt((gameManager.GetPlayer().playerStats.speedBonus - 1f) * 100f);

        string stats = "";
        // stats += "Max Health:      " + gameManager.GetPlayer().playerStats.maxPlayerHealth + "\n";
        // stats += "Armor:                " + gameManager.GetPlayer().playerStats.armor + "\n";
        // stats += "Speed:              +" + speedPercentage + "%\n";
        // stats += "Damage:          +" + damagePercentage + "%\n";
        // stats += "Cooldown:      +" + cooldownPercentage + "%\n";
        // stats += "Overdrives:       " + gameManager.GetPlayer().playerStats.overdrive + "\n";

        // passiveStats.text = stats;
        // // gameManager.GetPlayer().playerStats.maxPlayerHealth;
        // for(int i = 0; i < gameManager.powerupListForUI.Count; i++) {
        //     iconAndContainers[i].GetComponent<Image>().sprite = gameManager.powerupListForUI[i].getPowerupIcon();
        //     iconTexts[i].text = gameManager.powerupListForUI[i].getPowerupName() + ": Level " + gameManager.powerupListForUI[i].Level;
        //     iconAndContainers[i].SetActive(true);
        // }

        // HashSet<Mutation, int> totalCount = new HashSet<Mutation, int>();
        UpdateStatScreen();

        stats += "TODO ADD STUFF:";
    }

    void UpdateStatScreen() {
        Dictionary<string, MutationTuple> totalCount = new Dictionary<string, MutationTuple>();


        for(int i = 0; i < gameManager.mutationDeck.Count; i++) {
            Mutation mutation = gameManager.mutationDeck[i].GetComponent<Mutation>();
            string mutationName = mutation.mutationName;
            Debug.Log(mutationName);
            if (!totalCount.ContainsKey(mutationName)) {
                totalCount.TryAdd(mutationName, new MutationTuple(mutation, 1));
            } else {
                 totalCount[mutationName] = new MutationTuple(mutation, totalCount[mutationName].count + 1);
            }
        }

        int currentColumn = 0;
        foreach (var item in totalCount)
        {
            string name = item.Key;
            MutationTuple value = item.Value;

            iconAndContainers[currentColumn].GetComponent<Image>().sprite = value.mutation.icon;
            iconTexts[currentColumn].GetComponent<TMP_Text>().text = value.mutation.description;

            iconAndContainers[currentColumn].SetActive(true);

            currentColumn++;
        }

        // Create a GameObjectTuple
        // MutationTuple tuple = new MutationTuple(someGameObject, someInteger);

        // // Access the GameObject and integer values
        // GameObject obj = tuple.gameObject;
        // int value = tuple.integerValue;
    }
}
