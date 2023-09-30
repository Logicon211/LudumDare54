using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextBattleChoice : MonoBehaviour
{

    public TMP_Text title;
    public TMP_Text content;

    public NextBattlePopup nextBattlePopup;

    public Image buttonIcon;
    public Transform mutationButtonPosition;
    public GameObject mutationButton;

    private Mutation mutation;

    private GameManager gameManager;

    private GameObject mutationButtonObject;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseBattleOption() {
        // TODO: Put some logic here on level up selection
        Debug.Log("Chose level up title: " + title.text);
        // if(powerup) {
        //     powerup.LevelUp(1);
        //     powerup.getGameManager().SetPowerupToActive(powerup);
        // } else {
        //     Debug.Log("ERROR: no powerup associated...");
        // }
        // Time.timeScale = 1f;
        nextBattlePopup.ClosePopUp();
    }

    public void setTitle(string text) {
        title.text = text;
    }

    public void setContent(string text) {
        string parsedText = text.Replace("<br>", "\n");
        content.text = parsedText;
    }

    public void setIcon(Sprite spriteIcon) {
        buttonIcon.sprite = spriteIcon;
    }

    public void setMutation(Mutation mutationIn) {
        mutation = mutationIn;
        mutationButtonObject = Instantiate(mutationButton, mutationButtonPosition);
        mutationButtonObject.transform.localScale = new Vector3(mutationButtonObject.transform.localScale.x/2, mutationButtonObject.transform.localScale.y/2, mutationButtonObject.transform.localScale.z/2);
        mutationButtonObject.GetComponent<MutationButton>().nonInteractable = true;
        mutationButtonObject.GetComponent<MutationButton>().SetMutation(mutation);
    }

    public void clearMutationButton() {
        Destroy(mutationButtonObject);
    }
}
