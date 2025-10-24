using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SortButton : MonoBehaviour
{
    private ChestService _chestService;
    private Button _button;

    [Inject]
    public void Construct(ChestService chestService)
    {
        _chestService = chestService;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(_chestService.SortItemsByName);
    }
}
