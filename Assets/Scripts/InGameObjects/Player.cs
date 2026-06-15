using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 1;
    public float runSpeed = 1;
    public float acceleration = 0.1f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public Animator _animator;
    public ParticleSystem _trail;

    private Dependencies _dep;
    private Rigidbody2D _rb;
    private bool _isGrounded;
    
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        _dep = Dependencies.Instance;
        _dep.RegisterDependency<Player>(this);
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log(_isGrounded);
        _animator.SetBool(IsGrounded, _isGrounded);

        if (transform.position.y < -5) Dependencies.Instance.GetDependancy<PointsManager>()?.Death();
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (_isGrounded) _trail.Pause();
        if (!_isGrounded) _trail.Play();

    }

    void FixedUpdate()
    {
        runSpeed += acceleration * Time.fixedDeltaTime;
        _rb.linearVelocity = new Vector2(runSpeed, _rb.linearVelocity.y);
    }
}