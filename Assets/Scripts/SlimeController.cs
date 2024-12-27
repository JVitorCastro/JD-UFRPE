using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour, IDamageable
{

    public float        _moveSpeedSlime = 3.5f;
    public Vector2      _slimeDirection;
    private Rigidbody2D _slimeRB2D;
    [SerializeField] private float health = 3f;
    private Animator slimeAnimator;

    public DetectionController  _detectionArea;
    private SpriteRenderer      _spriteRenderer;
    private CapsuleCollider2D _slimeCollider;
    public float slimeDamage = 1;
    public GameObject apple;
    public GameObject cheese;
    public int dropRate;

    public float Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _slimeRB2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        slimeAnimator = GetComponent<Animator>();
        _slimeCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _slimeDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (_detectionArea.detectedObjs.Count > 0)
        {
            _slimeDirection = (_detectionArea.detectedObjs[0].transform.position - transform.position).normalized;

            _slimeRB2D.MovePosition(_slimeRB2D.position + _slimeDirection * _moveSpeedSlime * Time.fixedDeltaTime);

            if (_slimeDirection.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_slimeDirection.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }

    public void GetHit(float damage)
    {
        if (health > 1 && damage < health)
        {
            Debug.Log("Slime Hit for: " + damage);
            health--;           
        }
        else
        {
            _moveSpeedSlime = 0f;
            _slimeCollider.enabled = false;
            slimeAnimator.SetTrigger("Death");
            Invoke("Death", 1.8f);
            DropCollectible();
        }
    }

    public void GetHit(float damage, Vector2 knockBack)
    {
        _slimeRB2D.AddForce(knockBack);

        if (health > 1)
        {
            Debug.Log("Slime Hit for: " + damage);
            Debug.Log("Force: " + knockBack);
            health-=damage;
        }
        else
        {
            _moveSpeedSlime = 0f;
            _slimeCollider.enabled = false;
            slimeAnimator.SetTrigger("Death");
            Invoke("Death", 1.8f);
            DropCollectible();
        }
    }

    public void DropCollectible()
    {
        int randomNumber = Random.Range(0, 100);

        if (randomNumber <= dropRate && randomNumber % 2 == 0)
        {
            Instantiate(apple, transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else if (randomNumber <= dropRate && randomNumber % 2 != 0)
        {
            Instantiate(cheese, transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", slimeDamage);
        }
    }


}
