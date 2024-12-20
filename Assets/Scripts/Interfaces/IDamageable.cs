using UnityEngine;

public interface IDamageable
{
    public float Health { get; set; }
    public void GetHit(float damage, Vector2 knockBack);
    public void GetHit(float damage);
}
