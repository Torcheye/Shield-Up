using UnityEngine;

public class MouthAttack : BossAttack
{
    [SerializeField] private BulletConfig acidBullet;
    [SerializeField] private Vector2 normalLaunchPower;

    protected override void Attack()
    {
        var playerPos = DataManager.Instance.playerTransform.position;
        var x = (playerPos.x - transform.position.x) * normalLaunchPower.x;
        var y = (playerPos.y - transform.position.y) * normalLaunchPower.y;
        var dir = new Vector2(x, y);
        BulletFactory.Instance.SpawnBullet(acidBullet, transform.position, dir, true, transform);
    }

    protected override void EnhancedAttack()
    {
        //BulletFactory.Instance.SpawnBullet(enhancedBullet, transform.position, GetTargetDirection(), true, transform, target);
    }
}