using UnityEngine;
using UnityEngine.Events;

/* Representation of the player in the world.
 * Currently implemented as a Unity singleton,
 * though this will cause problems if I ever 
 * convert to more than one player
 * 
 * Contains PlayerController, keeps track of 
 * currency amount (spare parts), fires death 
 * event
 */
public sealed class PlayerCharacter : Character
{
    public UnityAction PlayerDeathEvent;

    public static PlayerCharacter Instance { get; private set; }

    public PlayerController Controller { get; private set; }
    public bool IsActiveInWorld { get; private set; }

    public int NumSpareParts
    {
        get => numSpareParts;
        set => numSpareParts = Mathf.Max(0, value);
    }

    [SerializeField]
    private AudioSource engineSound;
    [SerializeField]
    private int numSpareParts;
    private PlayerDeathAnimation deathAnimation;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            Controller = GetComponent<PlayerController>();
            deathAnimation = GetComponent<PlayerDeathAnimation>();
        }
    }

    private void Start()
    {
        IsActiveInWorld = true;
    }

    private void OnEnable()
    {
        Controller.enabled = true;
        engineSound.Play();
    }

    private void OnDisable()
    {
        Controller.enabled = false;
        engineSound.Stop();
    }

    public override void Die()
    {
        Controller.PlayerInputEnabled = false;
        Controller.ResetMovement();
        Controller.enabled = false;
        engineSound.Stop();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        IsActiveInWorld = false;
        PlayerDeathEvent?.Invoke();
        deathAnimation.PlayAnimation();
    }
}
