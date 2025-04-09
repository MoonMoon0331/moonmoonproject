using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Prefab 與容器")]
    public Transform tileParent;
    public GameObject tilePrefab;

    [Header("格子設定")]
    public Vector2 tileSize = new Vector2(100, 100);
    public float tileDelay = 0.02f;

    private List<GameObject> tiles = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartSceneTransition(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        yield return StartCoroutine(SpawnTilesWithDelay());

        // 等一點點再換場景（確保最後一格也出現）
        yield return new WaitForSeconds(tileDelay);

        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return null;

        yield return StartCoroutine(DestroyTilesWithDelay());
    }

    private IEnumerator SpawnTilesWithDelay()
    {
        ClearTiles();

        RectTransform rect = tileParent.GetComponent<RectTransform>();
        float width = rect.rect.width;
        float height = rect.rect.height;

        int columns = Mathf.CeilToInt(width / tileSize.x);
        int rows = Mathf.CeilToInt(height / tileSize.y);

        GridLayoutGroup grid = tileParent.GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.cellSize = tileSize;

        for (int i = 0; i < columns * rows; i++)
        {
            GameObject tile = Instantiate(tilePrefab, tileParent);
            tiles.Add(tile);
            yield return new WaitForSeconds(tileDelay); // 延遲一格
        }
    }

    private IEnumerator DestroyTilesWithDelay()
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
            yield return new WaitForSeconds(tileDelay); // 延遲一格
        }
        tiles.Clear();
    }

    private void ClearTiles()
    {
        foreach (var tile in tiles)
        {
            Destroy(tile);
        }
        tiles.Clear();
    }
}