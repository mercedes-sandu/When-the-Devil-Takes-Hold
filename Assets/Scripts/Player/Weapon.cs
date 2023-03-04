using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    /// The initial amount of bullets the player receives.
    /// </summary>
    [SerializeField] private static readonly int InitialBullets = 50;
    
    /// <summary>
    /// The bullet count text.
    /// </summary>
    [SerializeField] private TextMeshProUGUI bulletText;

    /// <summary>
    /// The bullet UI image.
    /// </summary>
    [SerializeField] private Image bullet;

    /// <summary>
    /// The current number of bullets the player has.
    /// </summary>
    private int _currentBullets = InitialBullets;

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
    /// The bullet UI image animator.
    /// </summary>
    private Animator _bulletAnim;

    /// <summary>
    /// Initializes the camera and subscribes to game events.
    /// </summary>
    private void Awake()
    {
        _camera = Camera.main;
        _bulletAnim = bullet.GetComponent<Animator>();
        
        GameEvent.OnAmmoGain += GainAmmo;
    }

    /// <summary>
    /// Initialize the pivot.
    /// </summary>
    private void Start()
    {
        _currentBullets = MainManager.Instance.ammo;
        _pivot = new GameObject().transform;
        _pivot.name = "Pivot";
        _pivot.transform.position = PlayerControl.Instance.transform.position;
        _pivot.transform.parent = PlayerControl.Instance.transform;
        transform.parent = _pivot;
    }

    /// <summary>
    /// Instantiates a bullet at the fire point.
    /// </summary>
    public void Shoot()
    {
        if (_currentBullets <= 0) return;
        _lookDirection = _camera.ScreenToWorldPoint(Input.mousePosition) - PlayerControl.Instance.transform.position;
        _lookAngle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, _lookAngle);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        _currentBullets--;
        bulletText.text = _currentBullets.ToString();
        MainManager.Instance.ammo = _currentBullets;
    }

    /// <summary>
    /// Sets the current amount of ammo of the weapon to the specified amount. Called at the start of a new scene.
    /// </summary>
    /// <param name="ammo">The amount of ammo the player will have.</param>
    public void SetAmmo(int ammo)
    {
        _currentBullets = ammo;
        bulletText.text = _currentBullets.ToString();
    }
    
    /// <summary>
    /// Adds the specified amount of ammo to the weapon.
    /// </summary>
    /// <param name="ammo">The amount of ammo the player gained.</param>
    private void GainAmmo(int ammo)
    {
        _currentBullets += ammo;
        bulletText.text = _currentBullets.ToString();
        MainManager.Instance.ammo = _currentBullets;
        Debug.Log("set ammo to " + _currentBullets + " in weapon.cs gainammo");
    }

    /// <summary>
    /// Updates the position and rotation of the projectile weapon.
    /// </summary>
    private void Update()
    {
        if (!PlayerControl.Instance.CanUseWeapon()) return;
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

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnAmmoGain -= GainAmmo;
    }
}