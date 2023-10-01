using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityQueueMenu : MonoBehaviour
{
    private GameObject[] drawnAbilities;
    private GameObject[] selectedAbilities;

    public GameObject confirmButton;
    
    private GameManager gameManager;

    public GameObject mutationButton;

    public List<Transform> drawnPositions;
    public List<Transform> queuedPositions;

    private List<GameObject> destroyWhenClosed;
    // Start is called before the first frame update

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        destroyWhenClosed = new List<GameObject>();
        drawnAbilities = new GameObject[5];
        selectedAbilities = new GameObject[3];
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Hide confirm button until all abilities are queued
    }

    public void AddRandomAbilitiesToSelection() {
        // TODO: instantiate random drawn abilities here
        // gameManager.mutationDeck
        List<GameObject> tempList = new List<GameObject>(gameManager.mutationDeck);
        for(int i = 0; i < 5; i++) {
            int randomIndex = Random.Range(0, tempList.Count);

            GameObject mutationButtonObject = Instantiate(mutationButton, drawnPositions[i]);

            drawnAbilities[FindNextEmptyIndex(drawnAbilities)] = mutationButtonObject; //Add(mutationButtonObject);
            MutationButton mutationButtonCreated = mutationButtonObject.GetComponent<MutationButton>();
            mutationButtonCreated.SetAbilityQueueMenu(this);
            mutationButtonCreated.SetMutation(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
            // mutationButtonCreated.buttonIndex = i;
            destroyWhenClosed.Add(mutationButtonObject);
            
        }
    }

    // Confirm Ability Selection 
    public void CloseAbilityMenu() {
        gameManager.ResetAbilityCountdown();
        List<Mutation> queuedList = new List<Mutation>();
        foreach (GameObject item in selectedAbilities)
        {
            queuedList.Add(item.GetComponent<MutationButton>().GetMutation().GetComponent<Mutation>());
        }

        gameManager.mutationQueue = queuedList;
        foreach (GameObject item in destroyWhenClosed)
        {
            Destroy(item);
        }

        // Clear out lists
        drawnAbilities = new GameObject[5];
        selectedAbilities = new GameObject[3];
        confirmButton.SetActive(false);
        gameManager.DisableAbilityQueueMenu();
    }

    public void SetAbilityAsQueued(GameObject button) {
        if (GetSizeOfArray(selectedAbilities) < 3) {
            int indexOfButton =System.Array.IndexOf(drawnAbilities, button);
            drawnAbilities[indexOfButton] = null;
            selectedAbilities[FindNextEmptyIndex(selectedAbilities)] = button;

            indexOfButton = System.Array.IndexOf(selectedAbilities, button);
            button.transform.SetParent(queuedPositions[indexOfButton], false);
            // button.transform.position = new Vector3(queuedPositions[indexOfButton].position.x, queuedPositions[indexOfButton].position.y, queuedPositions[indexOfButton].position.z);

            button.GetComponent<MutationButton>().isInQueue = true;

            if (GetSizeOfArray(selectedAbilities) >= 3) {
                confirmButton.SetActive(true);
            }
        } else {
            // TODO: Play error noise for trying to queue too much?
        }
    }

    public void UnsetQueuedAbility(GameObject button) {
        int indexOfButton =System.Array.IndexOf(selectedAbilities, button);
        selectedAbilities[indexOfButton] = null;
        drawnAbilities[FindNextEmptyIndex(drawnAbilities)] = button;

        indexOfButton = System.Array.IndexOf(drawnAbilities, button);
        button.transform.SetParent(drawnPositions[indexOfButton], false);
        // button.transform.position = new Vector3(drawnPositions[indexOfButton].position.x, drawnPositions[indexOfButton].position.y, drawnPositions[indexOfButton].position.z);
    
        button.GetComponent<MutationButton>().isInQueue = false;

        if (GetSizeOfArray(selectedAbilities) < 3) {
            confirmButton.SetActive(false);
        }
    }

    private int FindNextEmptyIndex(GameObject[] arrayToSearch) {
        for (int i = 0; i < arrayToSearch.Length; i++) {
            if(arrayToSearch[i] == null) {
                return i;
            }
        }
        // This means it's full so not sure what to return here
        return 0;
    }

    private int GetSizeOfArray(GameObject[] arrayToCheck) {
        int count = 0;
        for (int i = 0; i < arrayToCheck.Length; i++) {
            if(arrayToCheck[i] != null) {
                count++;
            }
        }
        // This means it's full so not sure what to return here
        return count;
    }
}
