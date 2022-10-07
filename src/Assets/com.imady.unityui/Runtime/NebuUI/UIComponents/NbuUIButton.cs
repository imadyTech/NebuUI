using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NbuUIButton : MonoBehaviour
{
    public Text tooltip;
    public string content;

    public void OnMouseOver()
    {
        tooltip.text = content;
    }
    public void OnMouseExit()
    {
        tooltip.text = "";
    }
}
