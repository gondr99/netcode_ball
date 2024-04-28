using UnityEngine;

public enum ProjectileRole
{
    Server,
    Client
}
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime;
    
    private ProjectileRole _role;
    private Collider2D _projectileCollider;
    private Rigidbody2D _rbCompo;
    private int _damage;
    
    private float _currentLifetime = 0;
    private SpriteRenderer _spriteVisual;

    private void Awake()
    {
        _rbCompo = GetComponent<Rigidbody2D>();
        _projectileCollider = GetComponent<Collider2D>();
        _spriteVisual = transform.Find("Visual").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _currentLifetime += Time.deltaTime;
        if (_currentLifetime >= _lifetime)
            Destroy(gameObject);
    }


    public void Fire(Collider2D playerCollider, Vector2 force, int damage, ProjectileRole role)
    {
        Physics2D.IgnoreCollision(playerCollider, _projectileCollider, true);
        _damage = damage;
        _role = role;
        _rbCompo.AddForce(force, ForceMode2D.Impulse);
        if(_role == ProjectileRole.Server)
        {
            _spriteVisual.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_role == ProjectileRole.Server)
        {
            if(collision.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damage, collision.contacts[0].normal, 2f);
            }
            //server projectile will make real damage to health logic        
            //and add force to ball
        }
        
        Destroy(gameObject);
    }
}
