using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutationButton : MonoBehaviour
{
    public bool isInQueue = false;
    private AbilityQueueMenu abilityQueueMenu;
    private Mutation mutation;
    public Image buttonIcon;

    // public int buttonIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        if (isInQueue) {
            abilityQueueMenu.UnsetQueuedAbility(this.gameObject);
        } else {
            abilityQueueMenu.SetAbilityAsQueued(this.gameObject);
        } 
    }

    public void SetAbilityQueueMenu(AbilityQueueMenu aqm) {
        abilityQueueMenu = aqm;
    }

    public void SetMutation(Mutation mut) {
        mutation = mut;
        buttonIcon.sprite = mutation.icon;
    }

    public Mutation GetMutation() {
        return mutation;
    }
}
