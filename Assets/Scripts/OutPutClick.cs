using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutPutClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject Canvas;
    public GameObject SelfHandPanel;

    public void OnPointerClick(PointerEventData pointerData)
    {
        OutPutButton.SetSubmit(Canvas, this.gameObject, SelfHandPanel);
    }
}