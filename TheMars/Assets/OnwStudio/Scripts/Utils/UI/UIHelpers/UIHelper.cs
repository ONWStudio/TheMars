using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Onw.UI
{
    public static class UIHelper
    {
        private static readonly Vector3[] _corners = new Vector3[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };

        public static bool CheckInClickPointer(this RectTransform rectTransform, Camera camera, Vector2 mousePosition)
            => rectTransform.rect.Contains(GetScreenToLocalPoint(rectTransform, camera, mousePosition));

        public static Vector2 ToScreenSize(this RectTransform rectTransform)
            => rectTransform.rect.size * rectTransform.lossyScale;

        public static Vector2 GetScreenToLocalPoint(this RectTransform rectTransform, Camera camera, Vector2 screenPoint)
        {
            Vector2 worldPosition = camera.ScreenToWorldPoint(screenPoint);
            Vector2 localPosition = rectTransform.InverseTransformPoint(worldPosition);

            return localPosition;
        }

        public static Vector2 GetWorldRectSize(this RectTransform rectTransform)
        {
            Vector3[] corners = rectTransform.GetWorldCorners();
            Vector2 worldSize = new(Vector2.Distance(corners[0], corners[3]),
                                    Vector2.Distance(corners[0], corners[1]));

            return worldSize;
        }

        public static float GetWorldWidth(this RectTransform rectTransform)
        {
            Vector3[] corners = rectTransform.GetWorldCorners();
            float worldWidth = Vector2.Distance(corners[0], corners[3]);

            return worldWidth;
        }

        public static float GetWorldHeight(this RectTransform rectTransform)
        {
            Vector3[] corners = rectTransform.GetWorldCorners();
            float worldHeight = Vector2.Distance(corners[0], corners[1]);

            return worldHeight;
        }

        public static Vector2 GetScreenCenterWorldPoint(this Camera camera)
        {
            Vector3 worldScreenCenterPoint = camera.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

            return new(worldScreenCenterPoint.x, worldScreenCenterPoint.y);
        }

        public static float GetScreenWidth(this RectTransform rectTransform, Camera camera)
        {
            Vector3[] corners = rectTransform.GetWorldCorners();

            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
            Vector2 bottomRight = RectTransformUtility.WorldToScreenPoint(camera, corners[3]);

            float screenWidth = Vector2.Distance(bottomLeft, bottomRight);
            return screenWidth;
        }

        public static float GetScreenHeight(this RectTransform rectTransform, Camera camera)
        {
            Vector3[] corners = rectTransform.GetWorldCorners();

            Vector2 topLeft = RectTransformUtility.WorldToScreenPoint(camera, corners[1]);
            Vector2 bottomRight = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);

            float screenHeight = Vector2.Distance(topLeft, bottomRight);
            return screenHeight;
        }

        public static Vector2 GetWorldToScreenSize(this RectTransform rectTransform, Camera camera)
        {
            Vector3[] corners = rectTransform.GetWorldCorners();

            Vector2 topLeft = RectTransformUtility.WorldToScreenPoint(camera, corners[1]);
            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
            Vector2 bottomRight = RectTransformUtility.WorldToScreenPoint(camera, corners[3]);

            Vector2 screenSize = new(Vector2.Distance(bottomLeft, bottomRight),
                                     Vector2.Distance(bottomLeft, topLeft));

            return screenSize;
        }

        public static Vector3[] GetWorldCorners(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_corners);
            return _corners;
        }
    }
}
