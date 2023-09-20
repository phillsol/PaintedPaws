using UnityEngine;
using UnityEngine.U2D;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [SerializeField] int sortOrderDensity = 100;

    public bool FoundHammer;
    public bool FoundKey;
    public bool FoundSandwich;

    void Start()
    {
        current = this;

        SpriteRenderSort();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpriteRenderSort()
    {
        SpriteShapeRenderer[] spriteShapes = FindObjectsOfType<SpriteShapeRenderer>();
        foreach (var sprite in spriteShapes)
        {
            sprite.sortingOrder = Mathf.RoundToInt(sprite.transform.position.z * -sortOrderDensity + 30000);
        }

        TextMeshPro[] TMProText = FindObjectsOfType<TextMeshPro>();
        foreach (var text in TMProText)
        {
            text.sortingOrder = Mathf.RoundToInt(text.transform.position.z * -sortOrderDensity + 30000);
        }

        
    }
}
