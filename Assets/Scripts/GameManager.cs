using UnityEngine;


public enum GameState
{
    None,
    Wait,
    Play,
    Over
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameState gameState = GameState.None;

    public int playerIndex = 0;
    public void StarftState(GameState _gameState)
    {
        gameState = _gameState;
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Wait:
                break;
            case GameState.Play:
                break;
            case GameState.Over:
                break;
        }
    }

    private void UpdateState()
    {
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Wait:
                break;
            case GameState.Play:
                break;
            case GameState.Over:
                break;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
