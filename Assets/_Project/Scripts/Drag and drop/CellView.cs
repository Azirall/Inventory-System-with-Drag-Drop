using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class CellView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject imageObject;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image image;

    private IStorageService _currentService;
    private DragDropOrchestrator _orchestrator;
    private ItemContextMenu _itemContextMenu;
    private ItemTooltip _itemTooltip;
    private RectTransform _rectTransform;
    private Coroutine _tooltipCoroutine;
    private bool _isPointerOver;
    private int _index;

    [Inject]
    public void Construct(DragDropOrchestrator dragDropOrchestrator, ItemContextMenu itemContextMenu, ItemTooltip itemTooltip)
    {
        _orchestrator = dragDropOrchestrator;
        _itemContextMenu = itemContextMenu;
        _itemTooltip = itemTooltip;
    }

    private void Awake()
    {
        _rectTransform = transform as RectTransform;
    }

    private void ApplyNewItem(Sprite sprite, int count)
    {
        image.sprite = sprite;
        canvasGroup.alpha = 1f;
        textCount.text = count == 1 ? string.Empty : count.ToString();
        TryRefreshTooltip();
    }

    private void CleanCell()
    {
        canvasGroup.alpha = 0f;
        image.sprite = null;
        textCount.text = string.Empty;

        if (_isPointerOver)
        {
            CancelTooltipRequest();
            _itemTooltip.Hide();
        }

        if (_itemContextMenu.IsVisibleFor(_currentService, _index))
        {
            _itemContextMenu.Hide();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        ClickType click;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isShift && !_orchestrator.HasItemInHand)
            {
                TryShowContextMenu();
            }
            else
            {
                _itemContextMenu.Hide();
            }

            click = isShift ? ClickType.ShiftRight : ClickType.Right;
        }
        else
        {
            _itemContextMenu.Hide();
            click = ClickType.Left;
        }

        _orchestrator.OnClick(_currentService, _index, click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver = true;
        StartTooltipRequest();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver = false;
        CancelTooltipRequest();
        _itemTooltip.Hide();
    }

    public virtual void RegisterCell(int index, IStorageService service)
    {
        _index = index;
        _currentService = service;
        _currentService.Changed += UpdateView;
    }

    private void UpdateView(int index, ItemStack stack)
    {
        if (_index != index)
        {
            return;
        }

        if (stack == null || stack.Item == null)
        {
            CleanCell();
        }
        else
        {
            ApplyNewItem(stack.Item.Sprite, stack.Count);
        }
    }

    private void OnDisable()
    {
        CancelTooltipRequest();

        if (_itemContextMenu != null && _itemContextMenu.IsVisibleFor(_currentService, _index))
        {
            _itemContextMenu.Hide();
        }

        if (_isPointerOver)
        {
            _isPointerOver = false;
            _itemTooltip.Hide();
        }
    }

    private void OnDestroy()
    {
        if (_currentService != null)
        {
            _currentService.Changed -= UpdateView;
        }
    }

    private void StartTooltipRequest()
    {
        if (_tooltipCoroutine != null)
        {
            StopCoroutine(_tooltipCoroutine);
        }

        _tooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
    }

    private void CancelTooltipRequest()
    {
        if (_tooltipCoroutine != null)
        {
            StopCoroutine(_tooltipCoroutine);
            _tooltipCoroutine = null;
        }
    }

    private IEnumerator ShowTooltipWithDelay()
    {
        yield return new WaitForSeconds(1f);

        if (!_isPointerOver)
        {
            yield break;
        }

        ItemStack stack = GetCurrentStack();
        if (stack != null && stack.Item != null)
        {
            _itemTooltip.Show(_rectTransform, stack);
        }
    }

    private void TryRefreshTooltip()
    {
        if (_isPointerOver && _itemTooltip != null && _itemTooltip.IsVisible)
        {
            ItemStack stack = GetCurrentStack();
            if (stack != null && stack.Item != null)
            {
                _itemTooltip.Show(_rectTransform, stack);
            }
        }
    }

    private void TryShowContextMenu()
    {
        ItemStack stack = GetCurrentStack();
        if (stack == null || stack.Item == null)
        {
            _itemContextMenu.Hide();
            return;
        }

        CancelTooltipRequest();
        _itemTooltip.Hide();
        _itemContextMenu.Show(_rectTransform, _currentService, _index);
    }

    private ItemStack GetCurrentStack()
    {
        return _currentService?.Get(_index);
    }
}

