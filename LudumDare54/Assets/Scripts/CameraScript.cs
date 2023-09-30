using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject battlegrid;
    private float oldMiddle = -1;

    //No idea how expensive getting these is every update, so I'm saving these.
    private BattleGrid gridRef;
    private Transform cameraTransform;
    private Transform battleGridTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        gridRef = battlegrid.GetComponent<BattleGrid>();
        battleGridTransform = battlegrid.transform;
        cameraTransform = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(oldMiddle != gridRef.middle){
            Debug.Log("Updating camera position to: " + battleGridTransform.position.x + gridRef.middle);
            cameraTransform.position = new Vector3(battleGridTransform.position.x + gridRef.middle, battleGridTransform.position.y, -10);
            oldMiddle = gridRef.middle;
        }
        Debug.Log("battleGridTransform.position.x: " + battleGridTransform.position.x);
        Debug.Log("gridRef.middle: " + gridRef.middle);
        var combined = battleGridTransform.position.x + gridRef.middle;
        Debug.Log("Skipping updating camera position, could have been: " + combined);
        // cameraTransform.position = new Vector3(battleGridTransform.position.x + gridRef.middle, battleGridTransform.position.y, -10);
    }
}
