using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckersGameManager : MonoBehaviour
{
    [Header("UI")]
    public Transform boardParent;     
    public Button cellPrefab;         

    [Header("Flow")]
    public string winSceneName = "WinScene";

    const int SIZE  = 8;
    const int EMPTY = 0;
    const int P_MAN = 1; 
    const int A_MAN = 2; 
    const int P_KING = 3;
    const int A_KING = 4;

    int[,] _board = new int[SIZE, SIZE];
    Button[,] _cells = new Button[SIZE, SIZE];
    TextMeshProUGUI[,] _labels = new TextMeshProUGUI[SIZE, SIZE];

    bool _playerTurn = true;
    int _selR = -1, _selC = -1;

    System.Random _rng = new System.Random();

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        BuildGrid();
        InitBoard();
        RefreshBoard();

        CenterMessage.Show(
            "Meldin has challenged you to checkers\n" +
            "and won't let you leave until you defeat him.",
            4f
        );
    }

    

    void BuildGrid()
    {
        
        for (int i = boardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(boardParent.GetChild(i).gameObject);
        }

        for (int r = 0; r < SIZE; r++)
        {
            for (int c = 0; c < SIZE; c++)
            {
                Button cell = Instantiate(cellPrefab, boardParent);
                _cells[r, c] = cell;

                TextMeshProUGUI lbl = cell.GetComponentInChildren<TextMeshProUGUI>();
                _labels[r, c] = lbl;

                int rr = r;
                int cc = c;
                cell.onClick.AddListener(() => OnCellClicked(rr, cc));
            }
        }
    }

    void InitBoard()
    {
        for (int r = 0; r < SIZE; r++)
        for (int c = 0; c < SIZE; c++)
            _board[r, c] = EMPTY;

        
        for (int r = 5; r < 8; r++)
        for (int c = 0; c < SIZE; c++)
        {
            if ((r + c) % 2 == 1)
                _board[r, c] = P_MAN;
        }

        
        for (int r = 0; r < 3; r++)
        for (int c = 0; c < SIZE; c++)
        {
            if ((r + c) % 2 == 1)
                _board[r, c] = A_MAN;
        }
    }

    void RefreshBoard()
    {
        for (int r = 0; r < SIZE; r++)
        {
            for (int c = 0; c < SIZE; c++)
            {
                Button cell = _cells[r, c];
                if (cell == null) continue;

                Image img = cell.GetComponent<Image>();
                bool dark = ((r + c) % 2 == 1);

                
                img.color = dark
                    ? new Color(0.65f, 0.32f, 0.32f)   
                    : new Color(0.95f, 0.95f, 0.95f);  

                TextMeshProUGUI lbl = _labels[r, c];
                if (lbl == null) continue;

                lbl.text = "";
                lbl.color = Color.white;

                if (!dark) continue;

                switch (_board[r, c])
                {
                    case P_MAN:  lbl.text = "●"; lbl.color = Color.white; break;
                    case A_MAN:  lbl.text = "●"; lbl.color = Color.black; break;
                    case P_KING: lbl.text = "♔"; lbl.color = Color.yellow; break;
                    case A_KING: lbl.text = "♚"; lbl.color = Color.yellow; break;
                }
            }
        }
    }

    

    bool IsPlayersPiece(int v) => (v == P_MAN || v == P_KING);
    bool IsAIPiece(int v)      => (v == A_MAN || v == A_KING);

    void MaybePromotePlayer(int r, int c)
    {
        if (_board[r, c] == P_MAN && r == 0)
            _board[r, c] = P_KING;
    }

    void MaybePromoteAI(int r, int c)
    {
        if (_board[r, c] == A_MAN && r == SIZE - 1)
            _board[r, c] = A_KING;
    }

    

    void OnCellClicked(int r, int c)
    {
        if (!_playerTurn)
            return;

        
        if (_selR < 0)
        {
            if (IsPlayersPiece(_board[r, c]))
            {
                _selR = r;
                _selC = c;
                CenterMessage.Show("Piece selected. Choose a destination.", 1.3f);
            }
            return;
        }

        
        if (r == _selR && c == _selC)
        {
            
            _selR = _selC = -1;
            return;
        }

        
        if (IsPlayersPiece(_board[r, c]))
        {
            _selR = r;
            _selC = c;
            CenterMessage.Show("Switched selected piece.", 1.0f);
            return;
        }

        
        if (TryPlayerMove(_selR, _selC, r, c))
        {
            _selR = _selC = -1;
            RefreshBoard();

            if (CheckWin(P_MAN))
            {
                OnPlayerWon();
            }
            else
            {
                StartCoroutine(AIMoveRoutine());
            }
        }
        else
        {
            CenterMessage.Show("Illegal move.", 1.0f);
            
            _selR = _selC = -1;
        }
    }

    bool TryPlayerMove(int r0, int c0, int r1, int c1)
    {
        int piece = _board[r0, c0];
        bool isKing = (piece == P_KING);

        if (!IsPlayersPiece(piece)) return false;
        if (_board[r1, c1] != EMPTY) return false;

        int dr = r1 - r0;
        int dc = Mathf.Abs(c1 - c0);
        int adr = Mathf.Abs(dr);

        if (dc == 1 && adr == 1)
        {
            
            if (!isKing && dr != -1) return false; 

            _board[r1, c1] = piece;
            _board[r0, c0] = EMPTY;
            MaybePromotePlayer(r1, c1);
            return true;
        }
        else if (dc == 2 && adr == 2)
        {
            
            int midR = (r0 + r1) / 2;
            int midC = (c0 + c1) / 2;
            int midPiece = _board[midR, midC];

            if (!IsAIPiece(midPiece)) return false;
            if (!isKing && dr != -2) return false; 

            _board[r1, c1] = piece;
            _board[r0, c0] = EMPTY;
            _board[midR, midC] = EMPTY;
            MaybePromotePlayer(r1, c1);
            return true;
        }

        return false;
    }

    

    IEnumerator AIMoveRoutine()
    {
        _playerTurn = false;
        yield return new WaitForSeconds(0.4f);

        DoRandomAIMove();
        RefreshBoard();

        if (CheckWin(A_MAN))
        {
            CenterMessage.Show("You lost the checkers game.", 2.5f);
            
        }
        else
        {
            _playerTurn = true;
        }
    }

    void DoRandomAIMove()
    {
        var moves = new List<(int r0, int c0, int r1, int c1)>();

        for (int r = 0; r < SIZE; r++)
        {
            for (int c = 0; c < SIZE; c++)
            {
                int piece = _board[r, c];
                if (!IsAIPiece(piece)) continue;

                bool isKing = (piece == A_KING);

                
                TryAddAIMMove(r, c, r + 1, c - 1, moves, false, isKing);
                TryAddAIMMove(r, c, r + 1, c + 1, moves, false, isKing);
                TryAddAIMMove(r, c, r + 2, c - 2, moves, true,  isKing);
                TryAddAIMMove(r, c, r + 2, c + 2, moves, true,  isKing);

                if (isKing)
                {
                    // backward moves for kings
                    TryAddAIMMove(r, c, r - 1, c - 1, moves, false, isKing);
                    TryAddAIMMove(r, c, r - 1, c + 1, moves, false, isKing);
                    TryAddAIMMove(r, c, r - 2, c - 2, moves, true,  isKing);
                    TryAddAIMMove(r, c, r - 2, c + 2, moves, true,  isKing);
                }
            }
        }

        if (moves.Count == 0)
            return;

        var m = moves[_rng.Next(moves.Count)];
        int movingPiece = _board[m.r0, m.c0];

        // capture if jump
        if (Mathf.Abs(m.r1 - m.r0) == 2)
        {
            int midR = (m.r0 + m.r1) / 2;
            int midC = (m.c0 + m.c1) / 2;
            _board[midR, midC] = EMPTY;
        }

        _board[m.r1, m.c1] = movingPiece;
        _board[m.r0, m.c0] = EMPTY;
        MaybePromoteAI(m.r1, m.c1);
    }

    void TryAddAIMMove(
        int r0, int c0, int r1, int c1,
        List<(int, int, int, int)> moves,
        bool isJump,
        bool isKing)
    {
        if (r1 < 0 || r1 >= SIZE || c1 < 0 || c1 >= SIZE) return;
        if (_board[r1, c1] != EMPTY) return;
        if (((r1 + c1) % 2) == 0) return; // dark squares only

        int dr = r1 - r0;
        int adr = Mathf.Abs(dr);

        if (!isKing)
        {
            // AI men move DOWN only
            if (!isJump && dr != 1) return;
            if (isJump && dr != 2) return;
        }
        else
        {
            // kings: allow up or down
            if (!isJump && adr != 1) return;
            if (isJump && adr != 2) return;
        }

        if (isJump)
        {
            int midR = (r0 + r1) / 2;
            int midC = (c0 + c1) / 2;
            if (!IsPlayersPiece(_board[midR, midC])) return;
        }

        moves.Add((r0, c0, r1, c1));
    }

    // ---------------- WIN / LOSE ----------------

    bool CheckWin(int who)
    {
        bool checkingPlayerWin = (who == P_MAN || who == P_KING);

        for (int r = 0; r < SIZE; r++)
        {
            for (int c = 0; c < SIZE; c++)
            {
                int v = _board[r, c];
                if (checkingPlayerWin)
                {
                    if (IsAIPiece(v)) return false;
                }
                else
                {
                    if (IsPlayersPiece(v)) return false;
                }
            }
        }
        return true;
    }

    void OnPlayerWon()
    {
        CenterMessage.Show("You defeated him at checkers!", 2.0f);
        StartCoroutine(GoToWinScene());
    }

    IEnumerator GoToWinScene()
    {
        yield return new WaitForSeconds(2.0f);
        if (!string.IsNullOrEmpty(winSceneName))
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}
