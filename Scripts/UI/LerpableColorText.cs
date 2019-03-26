using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// delete?
public class LerpableColorText : Text, IPointerEnterHandler, 
    IPointerExitHandler/*, IPointerClickHandler*/
{
    public delegate void MouseEventDelegate(LerpableColorText menuText);

    public event MouseEventDelegate MouseEnterEvent;
    public event MouseEventDelegate MouseExitEvent;
    public event MouseEventDelegate MouseClickEvent;

    private TextColorLerp colorLerper;

    protected override void Awake()
    {
        base.Awake();
        colorLerper = GetComponent<TextColorLerp>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExitEvent?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MouseClickEvent?.Invoke(this);
    }

    public Coroutine LerpTextColorForward()
    {
        return colorLerper.LerpForward();
    }

    public Coroutine LerpTextColorReverse()
    {
        return colorLerper.LerpReverse();
    }
}
