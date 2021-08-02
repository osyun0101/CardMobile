using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerData)
    {
        var x = this.transform.localPosition.x;
        var y = this.transform.localPosition.y;
        var z = this.transform.localPosition.y;
        if (y == 0)
        {
            this.transform.localPosition = new Vector3(x, y + 25, z);

            this.GetComponent<Outline>().effectColor = new Color(255f / 255f, 134f / 255f, 0f, 1f);
            this.GetComponent<Outline>().effectDistance = new Vector2(3f, 5f);
        }
        else
        {
            this.transform.localPosition = new Vector3(x, y - 25, z);
            this.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.5f);
            this.GetComponent<Outline>().effectDistance = new Vector2(1f, 1f);
        }
    }
}
