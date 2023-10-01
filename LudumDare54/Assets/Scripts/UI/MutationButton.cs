using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MutationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isInQueue = false;
    private AbilityQueueMenu abilityQueueMenu;
    private GameObject mutation;
    private Mutation mutationScript;
    public Image buttonIcon;

    public bool nonInteractable = false;

    // public int buttonIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nonInteractable) {
            buttonIcon.raycastTarget = false;
        }
    }

    public void OnClick() {
        if(!nonInteractable) {
            if (isInQueue) {
                abilityQueueMenu.UnsetQueuedAbility(this.gameObject);
            } else {
                abilityQueueMenu.SetAbilityAsQueued(this.gameObject);
            } 
        }
    }

    public void SetAbilityQueueMenu(AbilityQueueMenu aqm) {
        abilityQueueMenu = aqm;
    }

    public void SetMutation(GameObject mut) {
        mutation = mut;
        mutationScript = mutation.GetComponent<Mutation>();
        buttonIcon.sprite = mutationScript.icon;
    }

    public GameObject GetMutation() {
        return mutation;
    }

    private void OnMouseOver() {
        Tooltip.ShowTooltipStatic(mutationScript.description);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Tooltip.ShowTooltipStatic(mutationScript.description);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Tooltip.HideTooltipStatic();
    }



    private void OnMouseExit() { 
        Tooltip.HideTooltipStatic();
    }
}
