using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// The projectile bullet prefab.
    /// </summary>
    [SerializeField] private GameObject bulletPrefab;
    
    /// <summary>
    /// The transform corresponding to the point at which the projectile will fire.
    /// </summary>
    [SerializeField] private Transform firePoint;

    /// <summary>
    /// The player's look direction.
    /// </summary>
    private Vector2 _lookDirection;
    
    /// <summary>
    /// The player's look angle.
    /// </summary>
    private float _lookAngle;

    /// <summary>
    /// The pivot for the projectile weapon.
    /// </summary>
    private Transform _pivot;
    
    /// <summary>
    /// The main camera.
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// Initializes the camera.
    /// </summary>
    private void Awake()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// Initialize the pivot.
    /// </summary>
    private void Start()
    {
        _pivot = new GameObject().transform;
        _pivot.name = "Pivot";
        _pivot.transform.parent = PlayerControl.Instance.transform;
        transform.parent = _pivot;
    }

    /// <summary>
    /// Instantiates a bullet at the fire point.
    /// </summary>
    public void Shoot()
    {
        _lookDirection = _camera.ScreenToWorldPoint(Input.mousePosition) - PlayerControl.Instance.transform.position;
        _lookAngle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, _lookAngle);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    /// <summary>
    /// Updates the position and rotation of the projectile weapon.
    /// </summary>
    private void Update()
    {
        HandleMouseDirection();
    }
    
    /// <summary>
    /// Handles mouse direction of the player.
    /// </summary>
    private void HandleMouseDirection()
    {
        Vector3 playerPos = transform.parent.position;
        Vector2 screenPlayerPos = _camera.WorldToScreenPoint(playerPos);
        screenPlayerPos = (Vector2)Input.mousePosition - screenPlayerPos;
        float angle = Mathf.Atan2(screenPlayerPos.y, screenPlayerPos.x) * Mathf.Rad2Deg;
        
        _pivot.position = playerPos;
        _pivot.rotation = Quaternion.Euler(0, 0, angle);
    }
}