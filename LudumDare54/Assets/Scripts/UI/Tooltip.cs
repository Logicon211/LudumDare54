using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    private static Tooltip instance;
    public TMPro.TMP_Text text;
    // private RectTransform backgroundRectTransform;

    [SerializeField]
    private Camera uiCamera;
    

    private void Awake() {
        instance = this;
        // backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        HideTooltip();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localpoints;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localpoints);
        transform.localPosition = localpoints;
    }

    private void ShowTooltip(string tooltipText) {
        gameObject.SetActive(true);
        text.text = tooltipText;

    }

    private void HideTooltip() {
        gameObject.SetActive(false);
    }

    public static void ShowTooltipStatic(string tooltipString){
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltipStatic() {
        instance.HideTooltip();
    }
}
