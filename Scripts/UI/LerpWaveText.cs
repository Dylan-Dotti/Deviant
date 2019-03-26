using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LerpWaveText : Text, IPointerEnterHandler,
    IPointerExitHandler, ITextGridElement
{
    public class WaveForce
    {
        public float ForceMagnitude { get; private set; }

        public WaveForce(float magnitude)
        {
            ForceMagnitude = magnitude;
        }
    }

    public delegate void MouseEventDelegate(LerpWaveText lwText);

    public event MouseEventDelegate MouseEnterEvent;
    public event MouseEventDelegate MouseExitEvent;
    public event MouseEventDelegate MouseClickEvent;

    public float CurrentWaveMagnitude
    {
        get => Mathf.Clamp01(waveForces.Sum(wf => wf.ForceMagnitude));
    }
    public TextColorLerp ColorLerper { get; private set; }

    public int Row
    {
        get => row;
        set => row = Mathf.Max(0, value);
    }
    public int Column
    {
        get => column;
        set => column = Mathf.Max(0, value);
    }

    private int row, column;
    private HashSet<WaveForce> waveForces;

    protected override void Awake()
    {
        base.Awake();
        waveForces = new HashSet<WaveForce>();
        ColorLerper = GetComponent<TextColorLerp>();
    }

    public WaveForce AddWaveForce(float percentMagnitude)
    {
        WaveForce wf = new WaveForce(percentMagnitude);
        waveForces.Add(wf);
        ColorLerper.LerpToPercentage(CurrentWaveMagnitude);
        return wf;
    }

    public void RemoveWaveForce(WaveForce force)
    {
        waveForces.Remove(force);
        ColorLerper.LerpToPercentage(CurrentWaveMagnitude);
    }

    public Coroutine LerpTextColorForward()
    {
        return ColorLerper.LerpForward();
    }

    public Coroutine LerpTextColorReverse()
    {
        return ColorLerper.LerpReverse();
    }

    public Coroutine LerpTextColorToPercent(float percentage)
    {
        return ColorLerper.LerpToPercentage(percentage);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExitEvent?.Invoke(this);
    }
}
