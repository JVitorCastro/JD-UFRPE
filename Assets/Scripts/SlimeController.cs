using System.Collections;
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

    public float slimeDamage = 1;

    public float Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _slimeRB2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        slimeAnimator = GetComponent<Animator>();
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

    void IDamageable.GetHit(float damage)
    {
        if (health > 1)
        {
            Debug.Log("Slime Hit for: " + damage);
            health--;           
        }
        else
        {
            _moveSpeedSlime = 0f;
            slimeAnimator.SetTrigger("Death");
            Invoke("Death", 5f);
        }
    }

    void IDamageable.GetHit(float damage, Vector2 knockBack)
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
            slimeAnimator.SetTrigger("Death");
            Invoke("Death", 2f);
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
