using UnityEngine;

public class Chest : BaseInteractableObject
{
    [SerializeField] private GameObject _chestPanel;
    public override bool BeginUse()
    {
        _chestPanel.SetActive(true);
        return true;
    }

    public override void EndUse()
    {
        _chestPanel.SetActive(false);
    }
}
