using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool AlreadyUse => _alreadyUse;
    private IInteractable _currentObject;
    private bool _alreadyUse = false;

    public void Interact(out bool needStopPlayer,IInteractable currentObject)
    {
        needStopPlayer = false;

        if (currentObject == null)
            return;
        if (!_alreadyUse)
        {
            if(currentObject.BeginUse())
            {
                if (currentObject is BaseInteractableObject obj1 && obj1.NeedStopPlayer())
                {
                    _alreadyUse = true;
                    needStopPlayer = true;
                }
                return;
            }
        }
        currentObject.EndUse();
        _alreadyUse = false;
    }

    public void EndInteraction()
    {
        if (_currentObject is BaseInteractableObject obj2 && obj2.NeedStopPlayer())
        {
            _alreadyUse = false;
        }
    }

}
