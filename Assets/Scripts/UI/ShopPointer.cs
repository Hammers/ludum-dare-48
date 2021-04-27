using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopPointer : MonoBehaviour
{
    private Transform target;
    Camera mainCam;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCam = Camera.main;
    }

    public CanvasGroup innerObj;
    public float padding = 30f;
    public float targetOffset = 0f;
    private bool atTarget;
    private bool _show;
    public void Setup(Transform target)
    {
        this.target = target;
        Hide();
    }

    public void Show()
    {
        _show = true;
        innerObj.DOFade(1f, 0.2f);
    }

    public void Hide()
    {
        _show = false;
        innerObj.DOFade(0f, 0.2f);
    }
    
    void Update()
    {
        if (target == null)
            return;
        var indicatorPos = Vector2.zero;
        bool reachedTarget = false;
        var targetScreenPos =
            mainCam.WorldToScreenPoint(new Vector3(target.position.x, target.position.y + targetOffset, target.position.z));
        var centeredTarget =
            new Vector2(targetScreenPos.x - (Screen.width / 2), targetScreenPos.y - (Screen.height / 2));
        float rotation = 0f;
        var slope = centeredTarget.y / centeredTarget.x;

        var paddedWidth = (Screen.width - padding) / 2;
        var paddedHeight = (Screen.height - padding) / 2;

        if (centeredTarget.y > paddedHeight)
        {
            indicatorPos = new Vector2(paddedWidth / slope, paddedHeight);
        }
        else if (centeredTarget.y < -paddedHeight)
        {
            indicatorPos = new Vector2(-paddedWidth / slope, -paddedHeight);
        }
        else
        {
            if (!atTarget)
            {
                reachedTarget = true;
                atTarget = true;
            }

            indicatorPos = centeredTarget;
        }

        if (indicatorPos.x < -paddedWidth)
        {
            indicatorPos = new Vector2(-paddedWidth, slope * -paddedWidth);
        }
        else if (indicatorPos.x > paddedWidth)
        {
            indicatorPos = new Vector2(paddedWidth, slope * paddedWidth);
        }

        if (indicatorPos != centeredTarget)
        {

            if (_show)
            {
                innerObj.alpha = 1f;
            }

            rotation = Mathf.Atan2(centeredTarget.y, centeredTarget.x);
            rotation = (rotation * 180 / Mathf.PI) + 90f; //rad2deg
        }
        else
        {
            if (_show)
            {
                innerObj.alpha = 0f;
            }
        }

        
        #if UNITY_WEBGL
            indicatorPos /= 2;
        #endif
        
        rectTransform.anchoredPosition = indicatorPos;
        rectTransform.localRotation = Quaternion.Euler(0, 0, rotation);
        //Debug.Log($"Shop Point Pos: {indicatorPos} and rot {rotation}. _show: {_show}");
        if (reachedTarget)
        {
            //Debug.Log("STarting Tween");        
            //innerObj.DOLocalMoveY(30f, 1f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
        }
    }
}