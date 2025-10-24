using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float _speed;
    private SpriteRenderer _spriteRenderer;
    private CloudManager _manager;
    private float _despawnX;

    public void Init(Sprite sprite, float speed, CloudManager manager)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = sprite;
        _speed = speed;
        _manager = manager;

            _despawnX = Camera.main.ViewportToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.x < _despawnX)
        {
            _manager.ReturnCloud(this);
        }
    }
}
