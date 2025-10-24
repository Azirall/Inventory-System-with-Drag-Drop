using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ItemContextMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Vector2 offset = new(16f, 0f);

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private readonly Vector3[] _corners = new Vector3[4];

    private IStorageService _currentService;
    private int _currentIndex = -1;
    private bool _isVisible;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        Hide();
    }

    public void Init()
    {
        Hide();
    }

    public void Show(RectTransform target, IStorageService service, int index)
    {
        if (service == null)
        {
            Hide();
            return;
        }

        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        _currentService = service;
        _currentIndex = index;

        PositionNextTo(target);
        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
        _currentService = null;
        _currentIndex = -1;
    }

    public bool IsVisibleFor(IStorageService slots, int index)
    {
        return _isVisible && ReferenceEquals(slots, _currentService) && _currentIndex == index;
    }

    public void RemoveItem()
    {
        if (_currentService == null) return;

        if (_currentService.RemoveItem(_currentIndex))
            Hide();
    }

    public void UseItem()
    {
        if (_currentService == null) return;

        ItemStack stack = _currentService.Get(_currentIndex);
        if (stack == null || stack.Item == null) return;

        ItemType itemType = stack.Item.ItemType;
        if (itemType != ItemType.Weapon && itemType != ItemType.Potion && itemType != ItemType.Armor) return;

        if (_currentService.RemoveItem(_currentIndex))
            Hide();
    }

    private void PositionNextTo(RectTransform target)
    {
        if (target == null || _canvas == null) return;

        target.GetWorldCorners(_corners);
        Vector3 rightCenter = (_corners[2] + _corners[3]) * 0.5f;

        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, rightCenter);

        RectTransform canvasRect = _canvas.transform as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, screenPos, _canvas.worldCamera, out Vector2 localPos))
        {
            _rectTransform.anchoredPosition = localPos + offset;
        }
    }

    private void SetVisible(bool visible)
    {
        _isVisible = visible;
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.blocksRaycasts = visible;
        canvasGroup.interactable = visible;
    }
}

