using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if( _whiteTexture == null )
            {
                _whiteTexture = new Texture2D( 1, 1 );
                _whiteTexture.SetPixel( 0, 0, Color.white );
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
    {
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
        var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );

        return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
    }

    public static Bounds GetViewportBounds( Camera camera, Vector3 screenPosition1, Vector3 screenPosition2 )
    {
        var v1 = camera.ScreenToViewportPoint( screenPosition1 );
        var v2 = camera.ScreenToViewportPoint( screenPosition2 );
        var min = Vector3.Min( v1, v2 );
        var max = Vector3.Max( v1, v2 );
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax( min, max );
        return bounds;
    }

    public static void DrawScreenRect( Rect rect, Color color )
    {
        GUI.color = color;
        GUI.DrawTexture( rect, WhiteTexture );
        GUI.color = Color.white;
    }

    public static void DrawLine(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        //draw box
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static GameObject[] FindGameObjectsWithTag(string tag, Transform container)
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (Transform transform in container) {
            if (transform.gameObject.tag == tag) { returnList.Add(transform.gameObject); }
        }
        return returnList.ToArray();
    }

    public static GameObject[] FindGameObjectsWithLayer(int layer, Transform container) {
        List<GameObject> returnList = new List<GameObject>();
        foreach (Transform transform in container)
        {
            if (transform.gameObject.layer == layer) { returnList.Add(transform.gameObject); }
        }
        return returnList.ToArray();
    }

    public static GameObject[] FindGameObjectsWithName(string name, Transform container)
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (Transform transform in container)
        {
            if (transform.gameObject.name == name) { returnList.Add(transform.gameObject); }
        }
        return returnList.ToArray();
    }

    public static GameObject[] FindGameObjectsInTransform(Transform container)
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (Transform transform in container)
        {
            returnList.Add(transform.gameObject);
        }
        return returnList.ToArray();
    }
}
