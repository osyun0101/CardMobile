using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutPutClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject Canvas;
    public GameObject SelfHandPanel;
    public GameObject Image;

    public void OnPointerClick(PointerEventData pointerData)
    {
        OutPutButton.SetSubmit(Canvas, this.gameObject, SelfHandPanel);
        var SubmitImagePanel = Canvas.transform.Find("SubmitImage(Clone)").transform;
        var Triangle_1 = SubmitImagePanel.Find("Triangle");
        Triangle_1.gameObject.SetActive(false);
        var Triangle_2 = Image.transform.Find("Triangle2");
        Triangle_2.gameObject.SetActive(false);
        var clickPanel_1 = SubmitImagePanel.Find("GetClickPanel");
        clickPanel_1.gameObject.SetActive(false);
        var clickPanel_2 = Image.transform.Find("GetClickPanel2");
        clickPanel_2.gameObject.SetActive(false);
        GameManager.NextTurn(Canvas, PhotonNetwork.LocalPlayer.ActorNumber);
    }
}