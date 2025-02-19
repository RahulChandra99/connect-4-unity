using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject redCoin, yellowCoin;
    [SerializeField] private float coinFallSpeed = 10f;

    private int currentPlayer = 1; // Player 1 starts
    private bool activeTurn = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ColumnPressed(int columnNumber)
    {
        if (!activeTurn)
        {
            Debug.Log("Wait until turn is over");
            return;
        }

        int emptySpaceRow = PlayField.Instance.ValidMove(columnNumber);
        if (emptySpaceRow != -1)
        {
            StartCoroutine(PlayCoin(emptySpaceRow, columnNumber));
        }
        else
        {
            Debug.Log($"Column {columnNumber} is full.");
        }
    }

    private IEnumerator PlayCoin(int row, int column)
    {
        activeTurn = false;

        GameObject coin = Instantiate(currentPlayer == 1 ? redCoin : yellowCoin);
        
        //position from where coins start falling
        coin.transform.position = new Vector3(startPoint.position.x + column, startPoint.position.y + 1, startPoint.position.z);

        Vector3 goalPos = new Vector3(startPoint.position.x + column, startPoint.position.y - row, startPoint.position.z);

        while (Vector3.Distance(coin.transform.position, goalPos) > 0.01f)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, goalPos, coinFallSpeed * Time.deltaTime);
            yield return null;
        }

        PlayField.Instance.DropCoin(row, column, currentPlayer);

        activeTurn = true;
        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }
}