using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuText : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public delegate void MouseEventDelegate(StartMenuText menuText);

    public event MouseEventDelegate MouseEnterEvent;
    public event MouseEventDelegate MouseExitEvent;

    private TextColorLerp colorLerper;

    private void Awake()
    {
        colorLerper = GetComponent<TextColorLerp>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEvent?.Invoke(this);
        colorLerper.LerpForward();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExitEvent?.Invoke(this);
        colorLerper.LerpReverse();
    }
}
