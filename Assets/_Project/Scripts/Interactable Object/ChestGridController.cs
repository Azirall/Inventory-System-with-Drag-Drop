using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChestGridController : MonoBehaviour
{
    private List<CellView> _cellList = new();
    private IStorageService _storage;
    [Inject]
    public void Construct(ChestService chestService)
    {
        _storage = chestService;
    }

    public void Init()
    {
        AddCellInDict();
        RegisterCell();
    }

    private void AddCellInDict()
    {

        _cellList.AddRange(GetComponentsInChildren<CellView>());
    }
    private void RegisterCell()
    {
        for (int i = 0; i < _cellList.Count; i++)
        {
            _cellList[i].RegisterCell(i, _storage);
        }
    }
}