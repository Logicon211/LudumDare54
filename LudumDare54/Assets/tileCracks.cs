using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileCracks : MonoBehaviour
{


    public Tile myTile;
    private float timeToLive = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        timeToLive -= Time.deltaTime;
        if(timeToLive < 0){

        }
    }

    public void KillCracks(){
        myTile.RemoveEntityOnTile();
        Destroy(this.gameObject);
    }

}
