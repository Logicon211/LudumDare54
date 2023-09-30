using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityQueueMenu : MonoBehaviour
{
    private List<GameObject> drawnAbilities;
    private List<GameObject> selectedAbilities;

    public GameObject confirmButton;
    
    private GameManager gameManager;

    public GameObject mutationButton;

    public List<Transform> drawnPositions;
    public List<Transform> queuedPositions;

    private List<GameObject> destroyWhenClosed;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        destroyWhenClosed = new List<GameObject>();
        drawnAbilities = new List<GameObject>(5);
        selectedAbilities = new List<GameObject>(3);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Hide confirm button until all abilities are queued
    }

    public void AddRandomAbilitiesToSelection() {
        // TODO: instantiate random drawn abilities here
        // gameManager.mutationDeck
        for(int i = 0; i < 5; i++) {
            int randomIndex = Random.Range(0, gameManager.mutationDeck.Count);
            // drawnAbilities.Add(gameManager.mutationDeck[randomIndex]);
            // TODO: instantiate these onto the UI element
            GameObject mutationButtonObject = Instantiate(mutationButton, this.transform);
            mutationButton.transform.position = new Vector3(drawnPositions[i].position.x, drawnPositions[i].position.y, drawnPositions[i].position.z);
            drawnAbilities.Add(mutationButtonObject);
            MutationButton mutationButtonCreated = mutationButtonObject.GetComponent<MutationButton>();
            mutationButtonCreated.SetAbilityQueueMenu(this);
            mutationButtonCreated.SetMutation(gameManager.mutationDeck[randomIndex].GetComponent<Mutation>());
            // mutationButtonCreated.buttonIndex = i;
            destroyWhenClosed.Add(mutationButtonObject);
        }
    }

    public void CloseAbilityMenu() {
        gameManager.ResetAbilityCountdown();
        gameManager.DisableAbilityQueueMenu();

        foreach (GameObject item in destroyWhenClosed)
        {
            Destroy(item);
        }
    }

    public void SetAbilityAsQueued(GameObject button) {
        if (selectedAbilities.Count < 3) {
            drawnAbilities.Remove(button);
            selectedAbilities.Add(button);

            int indexOfButton = selectedAbilities.IndexOf(button);
            Debug.Log("INDEX OF QUEUED BUTTON: " + indexOfButton);
            button.transform.position = new Vector3(queuedPositions[indexOfButton].position.x, queuedPositions[indexOfButton].position.y, queuedPositions[indexOfButton].position.z);

            button.GetComponent<MutationButton>().isInQueue = true;
        } else {
            // TODO: Play error noise for trying to queue too much?
        }
    }

    public void UnsetQueuedAbility(GameObject button) {
        selectedAbilities.Remove(button);
        drawnAbilities.Add(button);

        int indexOfButton = drawnAbilities.IndexOf(button);
        Debug.Log("INDEX OF UNQUEUED BUTTON: " + indexOfButton);
        button.transform.position = new Vector3(drawnPositions[indexOfButton].position.x, drawnPositions[indexOfButton].position.y, drawnPositions[indexOfButton].position.z);
    
        button.GetComponent<MutationButton>().isInQueue = false;
    }
}
