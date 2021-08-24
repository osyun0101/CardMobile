using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutPutButton : MonoBehaviour
{
    public GameObject Canvas;
    public void OutputCard()
    {
        var SubmitImagePanel = Canvas.transform.Find("SubmitImage").gameObject;
        var selectCards = SelectHandManager.selectCards;
        var random = Random.Range(0, selectCards.Count);
        var selectCard = selectCards[random];
        SubmitImagePanel.SetActive(true);

        SubmitImagePanel.GetComponent<RawImage>().texture = selectCard.gameObject.GetComponent<RawImage>().texture;
    }
}
