using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRefreshBar : MonoBehaviour
{

    private Transform bar;
    private SpriteRenderer barRenderer;
    private float currentTimeRemaining;

    private GameManager gameManager;

    private float maxAbilityRefreshTime;

    public GameObject abilityRefreshText;

    public AudioClip abilityRefreshReady;
    private AudioSource AS;

    private bool playedSound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        barRenderer = bar.Find("BarSprite").gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        maxAbilityRefreshTime = gameManager.maxAbilityRefreshTime;
        currentTimeRemaining = 0f;
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimeRemaining >= maxAbilityRefreshTime) {
            gameManager.abilityRefreshAvailable = true;
            currentTimeRemaining = maxAbilityRefreshTime;
            if(!playedSound){
                AS.PlayOneShot(abilityRefreshReady);
                playedSound = true;
            }
        } else {
            currentTimeRemaining += Time.deltaTime;
        }

        if (gameManager.abilityRefreshAvailable) {
            abilityRefreshText.SetActive(true);
        } else {
            abilityRefreshText.SetActive(false);
        }
        SetBar();
    }

    public void resetAbilityCountdown() {
        currentTimeRemaining = 0f;
        playedSound = false;
    }

    public float SetBar()
    {
        float normalizedCurrentTimeRemaining = Mathf.Clamp(currentTimeRemaining/maxAbilityRefreshTime, 0f, 1f);
        bar.localScale = new Vector3(normalizedCurrentTimeRemaining, 1f);
        return normalizedCurrentTimeRemaining;
    }
}
