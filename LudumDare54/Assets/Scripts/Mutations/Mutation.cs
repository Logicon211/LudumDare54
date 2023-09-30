using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Mutation : MonoBehaviour
{

    [SerializeField] public Sprite icon;
    [SerializeField] public string description;
    [SerializeField] public string mutationName;


    public abstract void useAbility();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return mutationName;
    }
}
