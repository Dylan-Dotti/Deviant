using UnityEngine;

public sealed class PlayerCharacter : Character
{
    public CharacterDelegate PlayerSpawnEvent;
    public CharacterDelegate PlayerDeathEvent;

    public static PlayerCharacter Instance { get; private set; }

    public PlayerController Controller { get; private set; }
    public bool IsActiveInWorld { get; private set; }

    public int NumSpareParts
    {
        get => numSpareParts;
        set => numSpareParts = Mathf.Max(0, value);
    }

    [SerializeField]
    private AudioSource engineSource;
    [SerializeField]
    private int numSpareParts;
    private PlayerDeathSequence deathSequence;

    protected override void Awake()
    {
        base.Awake();
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
        IsActiveInWorld = true;
    }

    private void OnEnable()
    {
        Controller.enabled = true;
        engineSource.Play();
    }

    private void OnDisable()
    {
        Controller.enabled = false;
        engineSource.Stop();
    }

    public override void Die()
    {
        Controller.PlayerInputEnabled = false;
        Controller.ResetMovement();
        Controller.enabled = false;
        engineSource.Stop();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        IsActiveInWorld = false;
        PlayerDeathEvent?.Invoke(this);
        deathSequence.PlayAnimation();
    }
}
