using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextGrid
{
    public delegate void MouseEventDelegate(LerpWaveText lcText);

    public event MouseEventDelegate MouseEnterEvent;
    public event MouseEventDelegate MouseExitEvent;
    public event MouseEventDelegate MouseClickEvent;

    private LerpWaveText[,] textGrid;

    public TextGrid(List<List<LerpWaveText>> rowInputs)
    {
        InitializeTextGrid(rowInputs);
    }

    public LerpWaveText GetTextAt(int row, int col)
    {
        return IndexInRange(row, col) ? textGrid[row, col] : null;
    }

    public HashSet<LerpWaveText> GetTextNeighbors(int row, int col)
    {
        return GetTextNeighbors(GetTextAt(row, col));
    }

    public HashSet<LerpWaveText> GetTextNeighbors(LerpWaveText baseText)
    {
        HashSet<LerpWaveText> neighbors = new HashSet<LerpWaveText>();
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                int row = baseText.Row - 1 + r;
                int col = baseText.Column - 1 + c;
                LerpWaveText neighbor = GetTextAt(row, col);
                if (neighbor != null && neighbor != baseText)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }

    public int GetManhattanDistance(ITextGridElement a, ITextGridElement b)
    {
        return Mathf.Abs(a.Row - b.Row) + Mathf.Abs(a.Column - b.Column);
    }

    public int GetDiagonalDistance(ITextGridElement a, ITextGridElement b)
    {
        return Mathf.Max(Mathf.Abs(a.Row - b.Row),
            Mathf.Abs(a.Column - b.Column));
    }

    public int GetVerticalDistance(ITextGridElement a, ITextGridElement b)
    {
        return Mathf.Abs(a.Row - b.Row);
    }

    public int GetHorizontalDistance(ITextGridElement a, ITextGridElement b)
    {
        return Mathf.Abs(a.Column - b.Column);
    }

    public bool IndexInRange(int row, int col)
    {
        return row >= 0 && row < textGrid.GetLength(0) &&
            col >= 0 && col < textGrid.GetLength(1);
    }

    public void InitializeTextGrid(List<List<LerpWaveText>> rowInputs)
    {
        Debug.Log("Initializing text grid");
        //get longest row length
        int longestRowLength = 0;
        foreach (List<LerpWaveText> textRow in rowInputs)
        {
            if (textRow.Count > longestRowLength)
            {
                longestRowLength = textRow.Count;
            }
        }
        //initialize grid
        textGrid = new LerpWaveText[rowInputs.Count(), longestRowLength];
        for (int r = 0; r < textGrid.GetLength(0); r++)
        {
            float halfNumSpaces = (longestRowLength - rowInputs[r].Count) / 2f;
            int numLeftSpaces = Mathf.FloorToInt(halfNumSpaces);
            int numRightSpaces = Mathf.CeilToInt(halfNumSpaces);
            for (int c = numLeftSpaces; c < longestRowLength - numRightSpaces; c++)
            {
                LerpWaveText lwText = rowInputs[r][c - numLeftSpaces];
                lwText.Row = r;
                lwText.Column = c;
                lwText.MouseEnterEvent += OnTextMouseEnter;
                lwText.MouseExitEvent += OnTextMouseExit;
                lwText.MouseClickEvent += OnTextMouseClick;
                textGrid[r, c] = lwText;
            }
        }

        Debug.Log("Text grid initialized");
        Debug.Log("Longest Row: " + longestRowLength);
        for (int r = 0; r < textGrid.GetLength(0); r++)
        {
            string rowStr = "";
            for (int c = 0; c < textGrid.GetLength(1); c++)
            {
                if (textGrid[r, c] != null)
                {
                    rowStr += textGrid[r, c].text;
                }
            }
            Debug.Log(rowStr);
        }
    }

    public void InitializeTextGrid(List<StartMenuButton> buttons)
    {
        InitializeTextGrid(ParseTitleMenuText(buttons));
    }

    private List<List<LerpWaveText>> ParseTitleMenuText(List<StartMenuButton> buttons)
    {
        List<List<LerpWaveText>> smTexts = new List<List<LerpWaveText>>();
        buttons.ForEach(b => smTexts.Add(b.TextComponents));
        return smTexts;
    }

    private void OnTextMouseEnter(LerpWaveText text)
    {
        MouseEnterEvent?.Invoke(text);
    }

    private void OnTextMouseExit(LerpWaveText text)
    {
        MouseExitEvent?.Invoke(text);
    }

    private void OnTextMouseClick(LerpWaveText text)
    {
        MouseClickEvent?.Invoke(text);
    }
}
