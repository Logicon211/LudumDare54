using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LazerMutations : Mutation
{

public GameObject lazerPrefab;

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
        
        Instantiate(lazerPrefab, new Vector3(playerRef.transform.position.x + 4.6f, playerRef.transform.position.y+ 0.8f, playerRef.transform.position.z), Quaternion.identity);

    }
}
