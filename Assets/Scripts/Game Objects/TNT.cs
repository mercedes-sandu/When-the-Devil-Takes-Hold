using UnityEngine;

public class TNT : MonoBehaviour, IInteractable
{
    /// <summary>
    /// The explosion prefab.
    /// </summary>
    [SerializeField] private GameObject explosionPrefab;

    /// <summary>
    /// The range sprite (child of the TNT).
    /// </summary>
    [SerializeField] private SpriteRenderer rangeSprite;

    /// <summary>
    /// The range collider (child of the TNT).
    /// </summary>
    [SerializeField] private CircleCollider2D rangeCollider;

    /// <summary>
    /// The white range circle sprite. Indicates that nothing is within range of the TNT>
    /// </summary>
    [SerializeField] private Sprite normalRange;

    /// <summary>
    /// The red range circle sprite. Indicates that something is within range of the TNT.
    /// </summary>
    [SerializeField] private Sprite redRange;

    /// <summary>
    /// The radius of explosion.
    /// </summary>
    [SerializeField] private float radius = 2.5f;

    /// <summary>
    /// The amount of damage the TNT does.
    /// </summary>
    [SerializeField] private int damage = -50;

    /// <summary>
    /// The animator component.
    /// </summary>
    private Animator _anim;

    /// <summary>
    /// The sprite renderer component.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// The collider component.
    /// </summary>
    private CircleCollider2D _collider;

    /// <summary>
    /// True if nothing is in the explosion range, false otherwise.
    /// </summary>
    private bool _nothingInRange = true;

    /// <summary>
    /// Initialize components and subscribes to game events.
    /// </summary>
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();

        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        rangeSprite.enabled = true;
        rangeCollider.enabled = true;

        rangeSprite.sprite = normalRange;

        GameEvent.OnPlayerDetonate += DisableInteraction;
    }

    /// <summary>
    /// Checks if something is in range. If so, changes the range sprite to red.
    /// </summary>
    private void Update()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, radius);
        if (inRange.Length == 0)
        {
            _nothingInRange = true;
        }
        else
        {
            foreach (Collider2D collider in inRange)
            {
                var player = collider.GetComponent<PlayerControl>();
                var tnt = collider.GetComponent<TNT>();
                var npc = collider.GetComponent<NPC>();

                if (player || (tnt && tnt != this) || npc ||
                    (collider != rangeCollider && collider.CompareTag("TNTRange")))
                {
                    _nothingInRange = false;
                    break;
                }
                else
                {
                    _nothingInRange = true;
                }
            }
        }

        rangeSprite.sprite = _nothingInRange ? normalRange : redRange;
    }

    /// <summary>
    /// Causes the TNT to detonate.
    /// </summary>
    /// <param name="interactor">The interactor.</param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        if (!PlayerControl.Instance.CanDetonateTNT) return false;
        Detonate();
        return true;
    }

    /// <summary>
    /// Cues the detonation animation to play.
    /// </summary>
    private void Detonate()
    {
        PlayerControl.Instance.CanDetonateTNT = false;
        GameEvent.PlayerDetonate();
        _anim.Play("TNTExplode");
    }

    /// <summary>
    /// Causes the TNT to explode. Called by the animator.
    /// </summary>
    private void Explode()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        rangeSprite.enabled = false;
        rangeCollider.enabled = false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Collider2D[] toBeDestroyed = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in toBeDestroyed)
        {
            var player = collider.GetComponent<PlayerControl>();
            var tnt = collider.GetComponent<TNT>();
            var npc = collider.GetComponent<NPC>();

            if (player)
            {
                player.UpdateHealth(damage);
            }

            if ((tnt && tnt != this))
            {
                tnt.Detonate();
            }
            
            if (collider != rangeCollider && collider.CompareTag("TNTRange"))
            {
                collider.transform.parent.GetComponent<TNT>().Detonate();
            }

            if (npc)
            {
                npc.UpdateHealth(damage);
            }
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Disables interaction with the TNT.
    /// </summary>
    private void DisableInteraction()
    {
        _collider.enabled = false;
    }

    /// <summary>
    /// Draws the explosion radius in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnPlayerDetonate -= DisableInteraction;
    }
}