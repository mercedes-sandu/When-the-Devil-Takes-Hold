using UnityEngine;

public class Interactor : MonoBehaviour
{
    /// <summary>
    /// The interaction icon.
    /// </summary>
    [SerializeField] private GameObject interactIcon;
    
    /// <summary>
    /// The transform of the player's interaction point.
    /// </summary>
    [SerializeField] private Transform interactionPoint;
    
    /// <summary>
    /// The radius around the interaction point that will be checked for interactable objects.
    /// </summary>
    [SerializeField] private float interactionPointRadius = 0.34f;
    
    /// <summary>
    /// The layer(s) that interactable objects are on.
    /// </summary>
    [SerializeField] private LayerMask interactableMask;

    /// <summary>
    /// The list of interactable objects the player can access.
    /// </summary>
    private readonly BoxCollider2D[] _colliders = new BoxCollider2D[3];
    
    /// <summary>
    /// The number of interactable objects found.
    /// </summary>
    [SerializeField] private int numFound;

    /// <summary>
    /// Sets the interact icon to disabled.
    /// </summary>
    void Start()
    {
        interactIcon.SetActive(false);
    }
    
    /// <summary>
    /// Consistently checks for interactable objects.
    /// </summary>
    void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionPointRadius, 
            _colliders, interactableMask);

        if (numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactIcon.SetActive(true);
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact(this);
                }
            }
        }
        else
        {
            interactIcon.SetActive(false);
        }
    }

    /// <summary>
    /// Draws the interaction radius in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}