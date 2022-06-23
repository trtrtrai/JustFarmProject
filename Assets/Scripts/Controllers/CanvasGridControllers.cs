using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    public class CanvasGridControllers : MonoBehaviour
    {
        [SerializeField] GridLayoutGroup grid;
        [SerializeField] RectTransform listItemPage;
        [SerializeField] float sizeX;
        [SerializeField] float sizeY;
        [SerializeField] int col;
        [SerializeField] int row;
        [SerializeField] float ratio = 1.0f;
        [SerializeField] float spacing = 20.0f;

        // Update is called once per frame
        void LateUpdate()
        {
            if (row == 0)
            {
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = col;
                var width = listItemPage.rect.width;
                sizeX = width / col - spacing;
                sizeY = sizeX * ratio;
            }
            else if (col == 0)
            {
                grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                grid.constraintCount = row;
                var heigth = listItemPage.rect.height;
                sizeY = heigth / row - spacing;
                sizeX = sizeY * ratio;
            }

            grid.cellSize = new Vector2(sizeX, sizeY);
        }
    }

}
