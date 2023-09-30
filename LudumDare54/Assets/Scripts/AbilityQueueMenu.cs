using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityQueueMenu : MonoBehaviour
{
    public List<GameObject> drawnAbilities;
    public List<GameObject> selectedAbilities;

    public GameObject confirmButton;
    
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Hide confirm button until all abilities are queued
    }

    public void AddRandomAbilitiesToSelection() {
        // TODO: instantiate random drawn abilities here
    }

    public void CloseAbilityMenu() {
        gameManager.ResetAbilityCountdown();
        gameManager.DisableAbilityQueueMenu();
    }
}
