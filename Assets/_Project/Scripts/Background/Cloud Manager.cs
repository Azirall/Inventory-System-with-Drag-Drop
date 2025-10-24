using UnityEngine;
using System.Collections.Generic;

public class CloudManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Sprite[] cloudSprites;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;

    private Queue<Cloud> _cloudPool = new Queue<Cloud>();
    private float _timer;

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(cloudPrefab, transform);
            Cloud cloud = obj.GetComponent<Cloud>();
            obj.SetActive(false);
            _cloudPool.Enqueue(cloud);
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            SpawnCloud();
            _timer = 0f;
        }
    }

    private void SpawnCloud()
    {
        if (_cloudPool.Count > 0)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnCenter.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            Cloud cloud = _cloudPool.Dequeue();
            cloud.gameObject.SetActive(true);
            cloud.transform.position = spawnPosition;

            cloud.Init(
                cloudSprites[Random.Range(0, cloudSprites.Length)],
                Random.Range(minSpeed, maxSpeed),
                this
            );
        }
    }
        
    public void ReturnCloud(Cloud cloud)
    {
        cloud.gameObject.SetActive(false);
        _cloudPool.Enqueue(cloud);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(spawnCenter.position, spawnRadius);
        }
    }
}
