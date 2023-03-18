
public class BonusMonitor : DestroyableObject
{
    public override void OnHit(PlayerCharacter player)
    {
        base.OnHit(player);
        player.ringCount += 10;
        if (player.name == "Player")
            GameState.Instance.Rings += 10;
    }
}
