using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStyleSetter : MonoBehaviour
{
    //Sets style and substyle
    public static Sprite[,] sprites;
    private int spriteRowCount;
    private int spriteColumnCount;
    private static int passJ, passI, blockJ, blockI;//indexes of sprite
    private const int SUB_STYLE_CHANGE_COUNT = 40;
    public enum Style { diffShape, diffColor };
    public static Style style;

    void Awake()
    {
        ObstacleManager o = new ObstacleManager();
        spriteRowCount = o.spriteRowCount;
        spriteColumnCount = o.spriteColumnCount;

        passI = 2; passJ = 1;
        blockI = 2; blockJ = 2;
    }
    public void SetSprites(int passableObstacleIndex)
    {

        passI = blockI = GameManager.level;
        passJ = Random.Range(0, spriteColumnCount);
        blockJ = Random.Range(0, spriteColumnCount);
        while (passJ == blockJ)
            passJ = Random.Range(0, spriteColumnCount);

        for (int i = 0; i < 3; i++)
        {//apply srites
            GameObject childObstacle = this.transform.GetChild(i).gameObject;
            SpriteRenderer childObstacleSpriteRenderer = childObstacle.transform.GetChild(0).GetComponent<SpriteRenderer>();

            if (i == passableObstacleIndex)
            {
                childObstacleSpriteRenderer.sprite = sprites[passI, passJ];
            }
            else
            {
                childObstacleSpriteRenderer.sprite = sprites[blockI, blockJ];

            }
        }
    }

}
