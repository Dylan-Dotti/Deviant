using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Manages the color lerping effect for the title menu 
 * title and button components
 * 
 * Maintains a stack of activations, which propagate outward 
 * from the original letter and fades with distance. The 
 * stack is later unwound to reverse the effect when it is 
 * cancelled
 * 
 * Each activation is associated with a magnitude, and the 
 * color of each letter is determined by the sum of all 
 * magnitudes affecting it
 */ 
public class TextLerpWave : MonoBehaviour
{
    //stores the order of text color changes in a stack, to later be reversed
    public class WaveActivationSequence
    {
        public readonly LerpWaveText baseText;
        public readonly Stack<Dictionary<LerpWaveText,
            LerpWaveText.WaveForce>> activationStack;

        public WaveActivationSequence(LerpWaveText baseText,
            LerpWaveText.WaveForce baseForce)
        {
            this.baseText = baseText;
            activationStack = new Stack<Dictionary<
                LerpWaveText, LerpWaveText.WaveForce>>();
            Dictionary<LerpWaveText, LerpWaveText.WaveForce> baseDict =
                new Dictionary<LerpWaveText, LerpWaveText.WaveForce>();
            baseDict.Add(baseText, baseForce);
            activationStack.Push(baseDict);
        }

        public bool SequenceContainsText(LerpWaveText searchText)
        {
            foreach (Dictionary<LerpWaveText, LerpWaveText.WaveForce>
                dict in activationStack)
            {
                if (dict.ContainsKey(searchText))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public KeyValuePair<WaveActivationSequence, Coroutine> LerpForward(
        TextGrid textGrid, LerpWaveText baseText, int depth, 
        float sameRowFalloffScalar, float diffRowFalloffScalar)
    {
        LerpWaveText.WaveForce baseTextForce = baseText.AddWaveForce(1);
        WaveActivationSequence waveSequence = new WaveActivationSequence(
            baseText, baseTextForce);
        Coroutine waveRoutine = StartCoroutine(LerpTextColorCascadeForward(
            textGrid, waveSequence, depth, sameRowFalloffScalar, diffRowFalloffScalar));
        return new KeyValuePair<WaveActivationSequence, Coroutine>(
            waveSequence, waveRoutine);
    }

    public Coroutine LerpReverse(WaveActivationSequence sequence)
    {
        return StartCoroutine(LerpTextColorCascadeReverse(sequence));
    }

    public void CancelLerp(Coroutine lerpRoutine)
    {
        StopCoroutine(lerpRoutine);
    }

    public Coroutine ReverseSequenceAfter(KeyValuePair<WaveActivationSequence,
        Coroutine> waveSequencePair, float delay)
    {
        return StartCoroutine(ReverseSequenceDelayedCR(waveSequencePair, delay));
    }

    public Coroutine ReverseSequenceAfter(WaveActivationSequence sequence, 
        Coroutine routine, float delay)
    {
        return ReverseSequenceAfter(new KeyValuePair<WaveActivationSequence, 
            Coroutine>(sequence, routine), delay);
    }

    private IEnumerator LerpTextColorCascadeForward(TextGrid textGrid,
        WaveActivationSequence waveSequence, int depth, float sameRowFalloffScalar,
        float diffRowFalloffScalar)
    {
        Stack<Dictionary<LerpWaveText, LerpWaveText.WaveForce>> activationStack =
            waveSequence.activationStack;
        LerpWaveText baseText = waveSequence.baseText;
        for (int i = 0; i < depth; i++)
        {
            yield return new WaitForSeconds(0.05f);
            ICollection<LerpWaveText> previousActivated = activationStack.Peek().Keys;
            Dictionary<LerpWaveText, LerpWaveText.WaveForce> activated =
                new Dictionary<LerpWaveText, LerpWaveText.WaveForce>();
            foreach (LerpWaveText lwText in previousActivated)
            {
                ICollection<LerpWaveText> neighbors = textGrid.GetTextNeighbors(lwText);
                foreach (LerpWaveText neighborText in neighbors)
                {
                    if (neighborText != null && !waveSequence
                        .SequenceContainsText(neighborText) &&
                        !activated.ContainsKey(neighborText))
                    {
                        LerpWaveText.WaveForce force;
                        if (neighborText.Row == baseText.Row)
                        {
                            force = neighborText.AddWaveForce(1f - (sameRowFalloffScalar *
                                textGrid.GetHorizontalDistance(neighborText, baseText)));
                        }
                        else
                        {
                            force = neighborText.AddWaveForce(1f - (diffRowFalloffScalar *
                                textGrid.GetDiagonalDistance(neighborText, baseText)));
                        }
                        activated.Add(neighborText, force);
                    }
                }
            }
            if (activated.Count > 0)
            {
                activationStack.Push(activated);
            }
        }
    }

    private IEnumerator LerpTextColorCascadeReverse(WaveActivationSequence sequence)
    {
        Stack<Dictionary<LerpWaveText, LerpWaveText.WaveForce>> activationStack =
            sequence.activationStack;
        while (activationStack.Count > 0)
        {
            yield return new WaitForSeconds(0.25f);
            foreach (KeyValuePair<LerpWaveText, LerpWaveText.WaveForce>
                lwPair in activationStack.Pop())
            {
                lwPair.Key.RemoveWaveForce(lwPair.Value);
            }
        }
    }

    private IEnumerator ReverseSequenceDelayedCR(KeyValuePair<WaveActivationSequence, 
        Coroutine> waveSequencePair, float delay)
    {
        yield return new WaitForSeconds(delay);
        StopCoroutine(waveSequencePair.Value);
        LerpReverse(waveSequencePair.Key);
    }
}
