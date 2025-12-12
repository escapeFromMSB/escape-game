using UnityEngine;
using UnityEngine.Events;

public class KeycardSequencePuzzle : MonoBehaviour
{
    [Header("Puzzle")]
    public string puzzleTitle = "Keycard Access Puzzle";

    
    [Tooltip("Item IDs in the correct insertion order.")]
    public string[] sequence = new string[]
    {
        "keycard_blue",
        "keycard_green",
        "keycard_red"
    };

    [TextArea(4, 10)]
    public string riddleText =
        "Three colors guard the system.\n" +
        "First: the calm of clear skies.\n" +
        "Second: the color of fresh grass.\n" +
        "Last: the colors of a rose.\n\n" +
        "Present the keycards in that order.";

    [Header("Messages")]
    public float infoMessageDuration = 2.5f;

    [Header("Events")]
    public UnityEvent onSolved;

    private int _progress = 0;
    private bool _solved = false;
    private bool _riddleShownThisAttempt = false;

    public void ShowRiddle()
    {
        if (_riddleShownThisAttempt) return;
        _riddleShownThisAttempt = true;

        if (!string.IsNullOrEmpty(riddleText))
        {
            CenterMessage.Show(riddleText, 7f);
        }
    }

    
    
    
    public void TryInsertCard(string itemId, string displayName)
    {
        if (_solved)
        {
            CenterMessage.Show(puzzleTitle + "\nAlready solved.", infoMessageDuration);
            return;
        }

        if (sequence == null || sequence.Length == 0)
        {
            CenterMessage.Show("This puzzle is not configured.", infoMessageDuration);
            return;
        }

        if (!_riddleShownThisAttempt)
        {
            ShowRiddle();
        }

        string expected = sequence[_progress];

        if (itemId == expected)
        {
            _progress++;

            if (_progress >= sequence.Length)
            {
                _solved = true;
                CenterMessage.Show(puzzleTitle + "\nAccess granted!", 3.0f);
                GameFlags.KeycardPuzzleSolved = true;
                onSolved?.Invoke();
            }
            else
            {
                int remaining = sequence.Length - _progress;
                CenterMessage.Show(
                    "Correct keycard.\n" +
                    remaining + " more to go.",
                    infoMessageDuration
                );
            }
        }
        else
        {
            _progress = 0;
            _riddleShownThisAttempt = false;
            CenterMessage.Show(
                "Wrong keycard.\n" +
                "The sequence resets.",
                infoMessageDuration
            );
        }
    }
}
