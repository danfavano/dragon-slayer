using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState
{
    Setup,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}

public class BattleManager : MonoBehaviour
{
    [Header("Battle Setup")]
    [SerializeField] private int playerStartingHealth = 100;
    [SerializeField] private int enemyStartingHealth = 60;
    [SerializeField] private string exitSceneName = "GetStarted_Scene";

    [Header("Enemies")]
    [SerializeField] private MonsterData[] enemies = new MonsterData[3];

    public MonsterData[] Enemies => enemies;

    public BattleState CurrentState { get; private set; } = BattleState.Setup;
    public int PlayerHealth { get; private set; }
    public int EnemyHealth { get; private set; }

    public event Action<BattleState> BattleStateChanged;

    private void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        PlayerHealth = Mathf.Max(1, playerStartingHealth);
        EnemyHealth = Mathf.Max(1, enemyStartingHealth);
        SetState(BattleState.PlayerTurn);
    }

    public void PlayerAttack(int damage)
    {
        if (CurrentState != BattleState.PlayerTurn)
        {
            Debug.LogWarning("PlayerAttack called while it is not the player's turn.");
            return;
        }

        if (damage <= 0)
        {
            Debug.LogWarning("PlayerAttack damage must be greater than 0.");
            return;
        }

        EnemyHealth = Mathf.Max(0, EnemyHealth - damage);
        if (EnemyHealth == 0)
        {
            SetState(BattleState.Victory);
            return;
        }

        SetState(BattleState.EnemyTurn);
    }

    public void EnemyAttack(int damage)
    {
        if (CurrentState != BattleState.EnemyTurn)
        {
            Debug.LogWarning("EnemyAttack called while it is not the enemy's turn.");
            return;
        }

        if (damage <= 0)
        {
            Debug.LogWarning("EnemyAttack damage must be greater than 0.");
            return;
        }

        PlayerHealth = Mathf.Max(0, PlayerHealth - damage);
        if (PlayerHealth == 0)
        {
            SetState(BattleState.Defeat);
            return;
        }

        SetState(BattleState.PlayerTurn);
    }

    public void ExitBattle()
    {
        if (string.IsNullOrWhiteSpace(exitSceneName))
        {
            Debug.LogError("Exit scene name is empty. Set Exit Scene Name in the inspector.");
            return;
        }

        SceneManager.LoadScene(exitSceneName);
    }

    private void SetState(BattleState nextState)
    {
        CurrentState = nextState;
        BattleStateChanged?.Invoke(CurrentState);
    }
}
