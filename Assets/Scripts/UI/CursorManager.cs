using UnityEngine;

namespace Project.Scripts.UI
{
    public class CursorManager : MonoBehaviour
    {
        public Texture2D cursorTexture;
        public Vector2 hotspot;

        private void Start()
        {
            if (cursorTexture != null)
            {
                Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
            }
        }
    }
}
