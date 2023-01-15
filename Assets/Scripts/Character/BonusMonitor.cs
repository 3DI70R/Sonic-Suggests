
public class BonusMonitor : DestroyableObject
{
    public override void OnHit(PlayerCharacter player)
    {
        base.OnHit(player);
        player.ringCount += 10;
    }
}
