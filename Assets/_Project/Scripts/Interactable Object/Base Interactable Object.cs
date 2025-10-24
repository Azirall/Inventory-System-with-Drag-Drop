using UnityEngine;
public abstract class BaseInteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _needStopPlayer = false;
    protected PlayerOrchestrator _player;
    
    public abstract bool BeginUse();

    public abstract void EndUse();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player = collision.GetComponent<PlayerOrchestrator>();
            _player.SetInteractableObject(this);
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player.SetInteractableObject(null);
        }
    }

    public bool NeedStopPlayer() => _needStopPlayer;
    public void Use()
    {
        throw new System.NotImplementedException();
    }
}
