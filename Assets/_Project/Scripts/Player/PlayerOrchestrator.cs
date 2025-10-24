using System;
using UnityEngine;
using Zenject;

public class PlayerOrchestrator : MonoBehaviour
{
    public bool IsMove => _isMoving;
    private bool _canMove = true;
    
    private PlayerInteraction _playerInteraction;
    private PlayerMovement _playerMovment;
    private IInteractable _currentObject;
    private PlayerView _playerView;
    
    private float _moveInput;
    private bool _isMoving = false;
    
    public void SetInteractableObject(IInteractable obj) {
        _currentObject = obj;
    }
    private void Start()
    {
        _playerMovment = GetComponent<PlayerMovement>();
        _playerInteraction = GetComponent<PlayerInteraction>();
        _playerView = GetComponent<PlayerView>();
    }
    private void Update()
    {
        _moveInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.E))
        {
            _playerInteraction.Interact(out bool needStopPlayer, _currentObject);
            _canMove = !needStopPlayer;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_playerInteraction.AlreadyUse)
            {
                _playerInteraction.Interact(out bool needStopPlayer,_currentObject);
                _canMove = !needStopPlayer;
            }
        }
    }
    private void FixedUpdate()
    {
        if (_canMove && _playerMovment.Move(_moveInput))
        {
            if (_moveInput != 0)
            {
                _isMoving = true;
                _playerView.WalkAnim();
            }
            if (_moveInput > 0)
            {
                _playerView.ChangeDirection(1);
            }
            else if (_moveInput < 0)
            {
                _playerView.ChangeDirection(-1);
            }
        }
        else
        {
            _isMoving = false;
            _playerView.IdleAnim();
            _playerMovment.Move(0);
        }
    }

    public void EndInteraction()
    {
        _playerInteraction.EndInteraction();
        _canMove = true;
    }
}
