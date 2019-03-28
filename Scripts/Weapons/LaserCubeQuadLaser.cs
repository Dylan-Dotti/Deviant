using System.Collections.Generic;
using UnityEngine;

public class LaserCubeQuadLaser : MultiLaser
{
    public SingleLaser ForwardLaser => forwardLaser;
    public SingleLaser BackLaser => backLaser;
    public SingleLaser LeftLaser => leftLaser;
    public SingleLaser RightLaser => rightLaser;

    [SerializeField]
    private SingleLaser forwardLaser;
    [SerializeField]
    private SingleLaser backLaser;
    [SerializeField]
    private SingleLaser leftLaser;
    [SerializeField]
    private SingleLaser rightLaser;

    private AudioSource laserSound;
    private List<AudioSource> childLaserSounds;

    private void Awake()
    {
        laserSound = GetComponent<AudioSource>();
        childLaserSounds = new List<AudioSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            AudioSource childSource = transform.GetChild(i).
                GetComponent<AudioSource>();
            if (childSource != null)
            {
                childLaserSounds.Add(childSource);
            }
        }
    }

    public override void FireWeapon()
    {
        childLaserSounds.ForEach(s => s.mute = true);
        base.FireWeapon();
        if (laserSound != null)
        {
            laserSound.Play();
        }
    }

    public override void CancelFireWeapon()
    {
        childLaserSounds.ForEach(s => s.mute = false);
        base.CancelFireWeapon();
        if (laserSound != null)
        {
            laserSound.Stop();
        }
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        Lasers.ForEach(l => l.TurnToFace(l.FirePoint.position +
            l.FirePoint.forward * 100f));
    }
}
