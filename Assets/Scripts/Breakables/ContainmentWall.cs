using Sirenix.OdinInspector;

public class ContainmentWall : SpeedBreakable, ILevelObject
{
    [ShowInInspector, ReadOnly] public int LevelIndex { get; set; }

    public void Init(int levelIndex)
    {
        this.LevelIndex = levelIndex;
    }

    public void Alarm() { }

    protected override void Break()
    {
        GameManager.I.ContainmentWallBroken(LevelIndex);
        base.Break();
    }
}