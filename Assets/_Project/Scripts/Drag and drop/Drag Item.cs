using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class DragItem : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textCount;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }
    public void Init(Sprite sprite, int count) {
        _image.sprite = sprite;
        _textCount.text = count > 1 ? count.ToString() : "";
        _canvasGroup.alpha = 1;
    }
    public void ClearDragItem() {
        _canvasGroup.alpha = 0;
        _image.sprite = null;
        _textCount.text = "";
    }

}
