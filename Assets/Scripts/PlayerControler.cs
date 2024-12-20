using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody2D;
    private Animator _playerAnimator;
    private SpriteRenderer _playerRenderer;
    public float _playerSpeed;
    private float _playerInitialSpeed;
    public float _playerRunSpeed;
    private Vector2 _playerDirection;

    private bool _isAttack = false;

    public int health = 3;
    public Color hitColor;
    public Color noHitColor;
    private bool playerInvencible;
    public bool isDead;

    public GameObject swordHitBox;
    private Collider2D swordCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDead = false;
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitBox.GetComponent<Collider2D>();

        _playerInitialSpeed = _playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_playerDirection.sqrMagnitude > 0)
        {
            _playerAnimator.SetInteger("Movimento", 1);
        }
        else
        {
            _playerAnimator.SetInteger("Movimento", 0);
        }

        Flip();

        PlayerRun();

        OnAttack();

        if (_isAttack)
        {
            _playerAnimator.SetInteger("Movimento", 2);
        }

    }

    void FixedUpdate()
    {
        _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + _playerDirection.normalized * _playerSpeed * Time.fixedDeltaTime);
    }

    void Flip()
    {
        if (_playerDirection.x > 0)
        {
            transform.eulerAngles = new Vector2(0f, 0f);
        }
        else if (_playerDirection.x < 0)
        {
            transform.eulerAngles = new Vector2(0f, 180f);
            
        }
    }

    void PlayerRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerSpeed = _playerRunSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerSpeed = _playerInitialSpeed;
        }
    }

    void OnAttack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
            _playerSpeed = 0;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetMouseButtonUp(0))
        {
            _isAttack = false;
            _playerSpeed = _playerInitialSpeed;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    switch (collision.gameObject.tag)
    //    {
    //        case "Enemy":
    //            Hurt();
    //            break;

    //        default: 
    //            break;
    //    }
    //}

    void GetHit(float damage)
    {

        if (isDead)
        {
            return;
        }

        if (playerInvencible == false && health > 1)
        {
            health--;
            playerInvencible = true;
            StartCoroutine("Damage");
            Debug.Log("Perdeu uma vida!");
        }
        else if (!playerInvencible)
        {
            health--;
            _playerAnimator.SetTrigger("Death");
            StartCoroutine("Death");
        }
    }

    void Hurt()
    {
        if (isDead)
        {
            return;
        }

        if (playerInvencible == false && health > 1)
        {
            health--;
            playerInvencible = true;
            StartCoroutine("Damage");
            Debug.Log("Perdeu uma vida!");
        }
        else if (!playerInvencible)
        {
            health--;
            _playerAnimator.SetTrigger("Death");
            StartCoroutine("Death");
        }

    }

    IEnumerator Death()
    {
        isDead = true;
        _playerSpeed = 0;
        Debug.Log("DEAD!");
        yield return new WaitForSeconds(1.3f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(2);
    }

    IEnumerator Damage()
    {
        _playerRenderer.color = noHitColor;
        yield return new WaitForSeconds(0.1f);

        for (float i = 0; i < 1; i += 0.1f)
        {
            _playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _playerRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        _playerRenderer.color = Color.white;
        playerInvencible = false;
    }

}
