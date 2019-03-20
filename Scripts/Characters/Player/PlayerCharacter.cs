using UnityEngine;

public sealed class PlayerCharacter : Character
{
    public static CharacterDelegate PlayerSpawnEvent;
    public static CharacterDelegate PlayerDeathEvent;

    public static PlayerCharacter Instance { get; private set; }

    public PlayerController Controller { get; private set; }
    //public PlayerUpgrades Upgrades { get; private set; }
    public int NumSpareParts
    {
        get => numSpareParts;
        set => numSpareParts = Mathf.Max(0, value);
    }

    private int numSpareParts;
    private AnimationSequence deathSequence;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            Controller = GetComponent<PlayerController>();
            //Upgrades = new PlayerUpgrades();
            deathSequence = GetComponent<PlayerDeathSequence>();
        }
    }

    private void OnEnable()
    {
        Controller.enabled = true;
    }

    private void OnDisable()
    {
        Controller.enabled = false;
    }

    private void Start()
    {
        PlayerSpawnEvent?.Invoke(this);
        NumSpareParts = 100;
    }

    public override void Die()
    {
        Controller.PlayerInputEnabled = false;
        Controller.ResetMovement();
        Controller.enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerDeathEvent?.Invoke(this);
        deathSequence.PlayAnimation();
    }
}
