using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    public Collider2D swordCollider;
    public float swordDamage = 1f;
    public float knockBackForce = 900f;

    void Start()
    {
        if (swordCollider == null)
        {
            Debug.LogWarning("Sword Collider Not Set");
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        collision.gameObject.SendMessage("GetHit", swordDamage);   
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.GetComponent<IDamageable>();


        if (collision.gameObject.tag == "Enemy" && damageableObject != null && collision.gameObject.name != "AreaDetection")
        {
            Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2) (collision.gameObject.transform.position - parentPosition).normalized;
            Vector2 knockBack = direction * knockBackForce;
            //collision.gameObject.SendMessage("GetHit", swordDamage, knockBack);
            damageableObject.GetHit(swordDamage, knockBack);
        }
        else
        {
            Debug.LogWarning("Collider does not implement IDamageable");
        }
    }
}
