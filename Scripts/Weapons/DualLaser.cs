using UnityEngine;

public class DualLaser : LaserWeapon
{
    [SerializeField]
    private SingleLaser leftLaser;
    [SerializeField]
    private SingleLaser rightLaser;

    public override void FireWeapon()
    {
        leftLaser.FireWeapon();
        rightLaser.FireWeapon();
    }

    public override void AttemptFireWeapon()
    {
        leftLaser.AttemptFireWeapon();
        rightLaser.AttemptFireWeapon();
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        leftLaser.TurnToFace(targetPos);
        rightLaser.TurnToFace(targetPos);
    }

    public override void CancelFireWeapon()
    {
        leftLaser.CancelFireWeapon();
        rightLaser.CancelFireWeapon();
    }
}
