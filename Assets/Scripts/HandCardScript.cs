using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardScript : MonoBehaviour, IPointerClickHandler
{
    public string cardName;

    public void OnPointerClick(PointerEventData pointerData)
    {
        var x = this.transform.localPosition.x;
        var y = this.transform.localPosition.y;
        var z = this.transform.localPosition.z;
        if (y == 0)
        {
            //セレクトカードマネージャーに選択したカードをセット
            SelectHandManager.SetSelectCard(this);

            this.transform.localPosition = new Vector3(x, y + 25, z);
            this.GetComponent<Outline>().effectColor = new Color(255f / 255f, 134f / 255f, 0f, 1f);
            this.GetComponent<Outline>().effectDistance = new Vector2(3f, 5f);
        }
        else
        {
            //セレクトカードマネージャーに選択したカードをセット
            SelectHandManager.ResetSelectCard(this);

            this.transform.localPosition = new Vector3(x, y - 25, z);
            this.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.5f);
            this.GetComponent<Outline>().effectDistance = new Vector2(1f, 1f);
        }

        /*var cardLen = cardName.Length;
        string lastStr;
        if (cardName != "Joker_Color")
        {
            lastStr = cardName.Substring(cardLen - 2);
        }
        
        var selectcards = SelectHandManager.selectCards;
        if(selectcards.Count == 0)
        {
            
        }
        else if (selectcards.Count == 1)
        {
            if(cardName == "Joker_Color")
            {
                SelectHandManager.SetSelectCard(cardName);
            }
            selectcards[0].Contains(lastStr);
        }*/
    }
}
