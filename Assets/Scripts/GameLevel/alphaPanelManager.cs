using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class alphaPanelManager : MonoBehaviour
{
    public GameObject alphaPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (alphaPanel != null)
        {
            CanvasGroup canvasGroup = alphaPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0, 2f);
            }
            else
            {
                Debug.LogError("CanvasGroup component not found on alphaPanel!");
            }
        }
        else
        {
            Debug.LogError("alphaPanel GameObject is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
