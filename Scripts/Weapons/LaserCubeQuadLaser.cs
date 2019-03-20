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

    private void Awake()
    {
        //wrong
        /*ForwardLaser = transform.Find("Forward Weapon").
            GetComponent<SingleLaser>();
        BackLaser = transform.Find("Back Weapon").
            GetComponent<SingleLaser>();
        LeftLaser = transform.Find("Left Weapon").
            GetComponent<SingleLaser>();
        RightLaser = transform.Find("Right Weapon").
            GetComponent<SingleLaser>();*/
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        Lasers.ForEach(l => l.TurnToFace(l.FirePoint.position +
            l.FirePoint.forward * 100f));
    }
}
