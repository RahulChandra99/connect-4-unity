using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    public static PlayField Instance { get; private set; }
    
    //note : player number => 0 : empty, 1 : player1, 2 : player2
    private const int NumRows = 6;
    private const int NumColumns = 7;

    private int[,] _playingBoard = new int[NumRows, NumColumns];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    

    public int ValidMove(int columnSelected)
    {
        if (columnSelected < 0 || columnSelected >= NumColumns)
        {
            Debug.LogWarning($"Invalid column: {columnSelected}. Must be between 0 and {NumColumns - 1}.");
            return -1;
        }
        
        for (int i = NumRows - 1; i >= 0; i--)
        {
            
            if (_playingBoard[i, columnSelected] == 0)
            {
                return i;
            }
        }

        Debug.Log("Column is full.");
        return -1;
    }

    public void DropCoin(int x, int y, int player)
    {
        _playingBoard[x, y] = player;

        CheckforWin();
    }

    void CheckforWin()
    {
        HorizontalCheck();
        VerticalCheck();
        DiagonalCheck();
    }

    bool HorizontalCheck()
    {
        for (int row = 0; row < NumRows; row++)
        {
            for (int column = 0; column <= NumColumns - 4; column++)
            {
                int player = _playingBoard[row, column];

                if (player > 0)
                {
                    if (player == _playingBoard[row, column + 1] &&
                        player == _playingBoard[row, column + 2] &&
                        player == _playingBoard[row, column + 3])
                    {
                        Debug.Log($"Horizontal Win for player {player} at {row} , {column}");
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    bool VerticalCheck()
    {
        for (int column = 0; column < NumColumns; column++)
        {
            for (int row = 0; row <= NumRows - 4; row++)
            {
                int player = _playingBoard[row, column];

                if (player > 0)
                {
                    if (player == _playingBoard[row+1, column] &&
                        player == _playingBoard[row+2, column] &&
                        player == _playingBoard[row+3, column])
                    {
                        Debug.Log($"Vertical win for Player {player} at {row} , {column}");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    bool DiagonalCheck()
    {
        // Check for bottom-left to top-right diagonal (↗)
        for (int row = 0; row < NumRows - 4; row++)
        {
            for (int column = 0; column < NumColumns - 4; column++)
            {
                int player = _playingBoard[row, column];

                if (player > 0 &&
                    player == _playingBoard[row + 1, column + 1] &&
                    player == _playingBoard[row + 2, column + 2] &&
                    player == _playingBoard[row + 3, column + 3])
                {
                    Debug.Log($"Diagonal win for Player {player} at {row} , {column}");
                    return true;
                }
            }
        }
        
        // Check for top-left to bottom-right diagonal (↘)
        for (int row = 3; row < NumRows; row++)
        {
            for (int column = 0; column < NumColumns - 4; column++)
            {
                int player = _playingBoard[row, column];

                if (player > 0 &&
                    player == _playingBoard[row - 1, column + 1] &&
                    player == _playingBoard[row - 2, column + 2] &&
                    player == _playingBoard[row - 3, column + 3])
                {
                    Debug.Log($"Diagonal win for Player {player} at {row} , {column}");
                    return true;
                }
            }
        }
        
        // Bottom-right to top-left (↖)
        for (int row = 0; row <= NumRows - 4; row++)
        {
            for (int column = 3; column < NumColumns; column++) // Start from column 3
            {
                int player = _playingBoard[row, column];

                if (player > 0 &&
                    player == _playingBoard[row + 1, column - 1] &&
                    player == _playingBoard[row + 2, column - 2] &&
                    player == _playingBoard[row + 3, column - 3])
                {
                    Debug.Log($"Reverse right diagonal (↖) win for Player {player} at Row {row}, Column {column}");
                    return true;
                }
            }
        }

        // Top-right to bottom-left (↙)
        for (int row = 3; row < NumRows; row++) // Start from row 3
        {
            for (int column = 3; column < NumColumns; column++) // Start from column 3
            {
                int player = _playingBoard[row, column];

                if (player > 0 &&
                    player == _playingBoard[row - 1, column - 1] &&
                    player == _playingBoard[row - 2, column - 2] &&
                    player == _playingBoard[row - 3, column - 3])
                {
                    Debug.Log($"Reverse left diagonal (↙) win for Player {player} at Row {row}, Column {column}");
                    return true;
                }
            }
        }
        
        return false;
    }

    private string DebugBoard()
    {
        StringBuilder debugValue = new StringBuilder();

        for (int i = 0; i < NumRows; i++)
        {
            debugValue.Append("|");

            for (int j = 0; j < NumColumns; j++)
            {
                debugValue.Append(_playingBoard[i, j]).Append(",");
            }

            debugValue.Append("|\n");
        }

        return debugValue.ToString();
    }
}
