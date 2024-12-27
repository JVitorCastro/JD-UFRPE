using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private int valHeal = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHeal", valHeal);
            Destroy(this.gameObject);
        }
    }
}
