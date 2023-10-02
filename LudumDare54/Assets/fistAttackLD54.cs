using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistAttackLD54 : MonoBehaviour
{

    public Tile targetTile;
    public GameObject thisFist;
    public GameObject cracks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handSlamJamBringTheBam(){

        Debug.Log("Fist has Slammed");

        // We are done with our fist animation
        thisFist.SetActive(false);

        //Is the player on the tile?
        GameObject tileEntity = targetTile.entityOnTile;
        if(tileEntity != null){
            if (tileEntity.gameObject.tag == "Player") {
                // Hurt this dude.
                 tileEntity.gameObject.GetComponent<PlayerCharacter>().Damage(10);
            }
        }

        // Instantiate the cracked ground.
        GameObject crack = Instantiate(cracks, new Vector3(targetTile.transform.position.x+0.25f, targetTile.transform.position.y, targetTile.transform.position.z), Quaternion.Euler(0f, 0f, 80.934f)); /*Quaternion.identity);*/
       // projectile.getComponent(battleGrid.getPlayerTile;
        crack.GetComponent<tileCracks>().myTile = targetTile;
        // give it a reference to the tile.
        targetTile.SetEntityOnTile(crack);

        Destroy(this.gameObject);
    }

}
