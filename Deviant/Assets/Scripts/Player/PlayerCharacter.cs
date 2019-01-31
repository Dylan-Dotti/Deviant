using UnityEngine;

public sealed class PlayerCharacter : Character
{
    public static CharacterDelegate PlayerSpawnEvent;
    public static CharacterDelegate PlayerDeathEvent;

    public static PlayerCharacter Instance { get; private set; }

    public PlayerController Controller { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Player Awake");
        if (Instance == null)
        {
            Instance = this;
            Controller = GetComponent<PlayerController>();
        }
    }

    private void Start()
    {
        PlayerSpawnEvent?.Invoke(this);
        Controller.enabled = true;
    }

    public override void Die()
    {
        PlayerDeathEvent?.Invoke(this);
        gameObject.SetActive(false);
    }
}
