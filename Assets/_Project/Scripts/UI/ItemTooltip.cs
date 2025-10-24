using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Vector2 offset = new(16f, 0f);

    private RectTransform _rectTransform;
    private readonly Vector3[] _corners = new Vector3[4];
    private bool _isVisible;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

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
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        if (titleText != null)
        {
            titleText.text = stack.Item.Name;
        }

        if (descriptionText != null)
        {
            descriptionText.text = stack.Item.ItemDescription;
        }

        PositionNextTo(target);
        SetVisible(true);
    }

    public void Hide()
    {
        if (titleText != null)
        {
            titleText.text = string.Empty;
        }

        if (descriptionText != null)
        {
            descriptionText.text = string.Empty;
        }

        SetVisible(false);
    }

    private void PositionNextTo(RectTransform target)
    {
        if (target == null)
        {
            return;
        }

        target.GetWorldCorners(_corners);
        Vector3 rightCenter = (_corners[2] + _corners[3]) * 0.5f;
        Vector3 worldPos = rightCenter + (Vector3)offset;
        _rectTransform.position = worldPos;
    }

    private void SetVisible(bool visible)
    {
        _isVisible = visible;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.blocksRaycasts = visible;
        canvasGroup.interactable = visible;
    }
}
