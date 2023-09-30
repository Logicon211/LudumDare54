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
    
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        barRenderer = bar.Find("BarSprite").gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        maxAbilityRefreshTime = gameManager.maxAbilityRefreshTime;
        currentTimeRemaining = gameManager.maxAbilityRefreshTime;
    }

    // public float SetCooldown(float normalizedCooldown)
    // {
    //     // currentCooldown = Mathf.Clamp(normalizedCooldown, 0f, 1f);
    //     // bar.localScale = new Vector3(currentCooldown, 1f);
    //     // barRenderer.color = new Color(currentCooldown, 0f, 1f - currentCooldown, 1f);
    //     // return currentCooldown;
    // }

    // Update is called once per frame
    void Update()
    {
        currentTimeRemaining -= Time.deltaTime;
        if(currentTimeRemaining <= 0f) {
            gameManager.abilityRefreshAvailable = true;
            currentTimeRemaining = 0f;
        }

        if (gameManager.abilityRefreshAvailable) {
            abilityRefreshText.SetActive(true);
        } else {
            abilityRefreshText.SetActive(false);
        }
        SetBar();
    }

    public void resetAbilityCountdown() {
        currentTimeRemaining = maxAbilityRefreshTime;
    }

    public float SetBar()
    {
        float normalizedCurrentTimeRemaining = Mathf.Clamp(currentTimeRemaining/maxAbilityRefreshTime, 0f, 1f);
        bar.localScale = new Vector3(normalizedCurrentTimeRemaining, 1f);
        return normalizedCurrentTimeRemaining;
    }
}
