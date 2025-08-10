using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _timeDelay = 1.0f;

    private ObjectPool<Cube> _cubePool;
    private BoxCollider _spawnArea;

    private void Awake()
    {
        _spawnArea = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _cubePool = new ObjectPool<Cube>(
            CreateFunc,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy,
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );

        StartCoroutine(SpawnCubes());
    }

    protected Cube CreateFunc()
    {
        Cube cube = Instantiate(_cubePrefab, transform);

        
        
        cube.Initialize(GetRandomPosition(), _cubePool);

        Debug.Log($"CreateFunc {cube.GetInstanceID()}");
        return cube;
    }

    private void ActionOnGet(Cube cube)
    {
        Debug.Log($"ActionOnGet {cube.GetInstanceID()}");
        cube.Initialize(GetRandomPosition(), _cubePool);
        cube.gameObject.SetActive(true);
    }
    private void ActionOnRelease(Cube cube)
    {

        Debug.Log($"ActionOnRelease {cube.GetInstanceID()}");
        cube.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(Cube cube)
    {

        Debug.Log($"ActionOnDestroy {cube.GetInstanceID()}");
        Destroy(cube.gameObject);
    }

    private IEnumerator SpawnCubes()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_timeDelay);
            _cubePool.Get();
        }
    }

    private Vector3 GetRandomPosition()
    {
        float xPosition = Random.Range(_spawnArea.size.x / 2 * -1, _spawnArea.size.x / 2);
        return new Vector3(xPosition, transform.position.y, transform.position.z);
    }
}
