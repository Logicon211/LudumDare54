using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShotgunMutation : Mutation
{

public GameObject shotgunPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void useAbility(GameObject playerRef) {
        Debug.Log("TODO: setup lazer ability");
        
        Instantiate(shotgunPrefab, new Vector3(playerRef.transform.position.x + 0.5f, playerRef.transform.position.y+ 0.8f, playerRef.transform.position.z), Quaternion.identity);

    }
}
