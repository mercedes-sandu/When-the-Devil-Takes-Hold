public interface IInteractable
{
    /// <summary>
    /// The interaction that will be performed when the player interacts with this object.
    /// </summary>
    /// <param name="interactor">The interactor for this interaction.</param>
    /// <returns>True if the interaction was successful, false otherwise.</returns>
    public bool Interact(Interactor interactor);
}