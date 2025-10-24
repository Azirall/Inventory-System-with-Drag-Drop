using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Vector2 offset = new(16f, 0f);

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private readonly Vector3[] _corners = new Vector3[4];
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

    public bool IsVisible => _isVisible;

    public void Show(RectTransform target, ItemStack stack)
    {
        if (stack?.Item == null)
        {
            Hide();
            return;
        }

        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        if (titleText != null)
            titleText.text = stack.Item.Name;

        if (descriptionText != null)
            descriptionText.text = stack.Item.ItemDescription;

        PositionNextTo(target);
        SetVisible(true);
    }

    public void Hide()
    {
        if (titleText != null)
            titleText.text = string.Empty;

        if (descriptionText != null)
            descriptionText.text = string.Empty;

        SetVisible(false);
    }

    private void PositionNextTo(RectTransform target)
    {
        if (target == null || _canvas == null)
            return;

        target.GetWorldCorners(_corners);
        Vector3 rightCenter = (_corners[2] + _corners[3]) * 0.5f;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, rightCenter);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            _canvas.worldCamera,
            out Vector2 localPos
        );

        _rectTransform.anchoredPosition = localPos + offset;
    }

    private void SetVisible(bool visible)
    {
        _isVisible = visible;
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.blocksRaycasts = visible;
        canvasGroup.interactable = visible;
    }
}

