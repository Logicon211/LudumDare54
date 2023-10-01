using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMutation : Mutation
{

    public float shieldValue = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void useAbility(GameObject playerRef) {
        playerRef.GetComponent<PlayerCharacter>().TurnOnShield(shieldValue);
    }
}
