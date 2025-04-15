using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortByPosition : MonoBehaviour
{
    public int sortingOffset = 0;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if(GameManager.Instance.currentSceneName == "Office"){return;}
        sr.sortingOrder = -(int)(transform.position.y * 100) + sortingOffset;
    }
}