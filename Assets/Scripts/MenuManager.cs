using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] Transform menu;
    private float hiddenPosX;
    private float visiblePosX;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        hiddenPosX = Screen.width + 100f; // changed from 1024f due to my screen not compatible
        visiblePosX = hiddenPosX - 300f;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.menuStage && !isActive) Appear();
        if (!gm.menuStage && isActive) Disappear();

    }

    void Appear()
    {
        menu.transform.DOMoveX(visiblePosX, 0.4f);
        isActive = true;

    }
    void Disappear()
    {
        isActive = false;
        // DOTween.Kill(menu.transform);
        menu.transform.DOMoveX(hiddenPosX, 0.4f);
        Debug.Log("Disappearing Menu");
    }
}
