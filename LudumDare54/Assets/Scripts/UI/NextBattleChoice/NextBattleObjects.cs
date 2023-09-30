using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextBattleObject
{
	// private GameManager gameManager;
	// void Start() {
	// 	gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	// }

    public float amountOfHealthRegained = 0f;

    // TODO: update to whatever enemy base class is
    public List<GameObject> enemies = new List<GameObject>();

    public Mutation mutation = null;

    public void SetValues(float healthRegained, List<GameObject> enemiesList, Mutation mutationIn) {
        amountOfHealthRegained = healthRegained;
        enemies = enemiesList;
        mutation = mutationIn;
    }


}