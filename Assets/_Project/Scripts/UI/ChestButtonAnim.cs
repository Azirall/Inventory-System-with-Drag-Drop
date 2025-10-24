using DG.Tweening;
using UnityEngine;

public class ChestButtonAnim : MonoBehaviour
{
     [SerializeField] private GameObject button;
     [SerializeField] private float scale = 1.1f;
     [SerializeField] private float duration = 0.5f;
     [SerializeField] private CanvasGroup canvasGroup;
      private Tween _pulse;
      
      private void OnTriggerEnter2D(Collider2D other)
     {
          canvasGroup.alpha = 1;
          _pulse = button.transform.DOScale(scale, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
     }

     private void OnTriggerExit2D(Collider2D other)
     {    
          canvasGroup.alpha = 0;
          _pulse.Kill();
     }
}
