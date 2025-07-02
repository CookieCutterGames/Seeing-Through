using System.Diagnostics;

public class ThrowItem : AbilityBase
{
    protected override void Execute()
    {
        if (!Player.Instance.isHoldingMug)
            return;
    }
}
