public class ContainmentWall : SpeedBreakable
{
    protected override void Break()
    {
        GameManager.I.ContainmentWallBroken();
        base.Break();
    }
}