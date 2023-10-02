using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistHelperScript : MonoBehaviour
{

    public fistAttackLD54 fistRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fistHasLanded(){
        fistRef.handSlamJamBringTheBam();
    }
}
