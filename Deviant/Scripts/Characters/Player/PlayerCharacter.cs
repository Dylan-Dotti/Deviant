using UnityEngine;

public sealed class PlayerCharacter : Character
{
    public static CharacterDelegate PlayerSpawnEvent;
    public static CharacterDelegate PlayerDeathEvent;

    public static PlayerCharacter Instance { get; private set; }

    public const string PLAYER_BODY_TAG = "PlayerBody";

    public PlayerController Controller { get; private set; }

    private AnimationSequence deathSequence;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Player Awake");
        if (Instance == null)
        {
            Instance = this;
            Controller = GetComponent<PlayerController>();
            deathSequence = GetComponent<PlayerDeathSequence>();
        }
    }

    private void Start()
    {
        PlayerSpawnEvent?.Invoke(this);
        Controller.enabled = true;
    }

    public override void Die()
    {
        //Controller.KeyboardInputEnabled = false;
        Controller.ResetMovement();
        Controller.enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerDeathEvent?.Invoke(this);
        deathSequence.PlayAnimation();
        //gameObject.SetActive(false);
    }
}
