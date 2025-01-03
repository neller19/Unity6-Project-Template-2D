using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ExtensionMethods
{
    #region Collections (array, list, etc)

    public static void Clear<T>(this T[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = default;
        }
    }

    public static T RandomItem<T>(this IList<T> collection)
    {
        return collection[Random.Range(0, collection.Count)];
    }

    #endregion

    #region LayerMask
    //If the mask contains the layer, the bit representation of the & operation will not be 0
    //the layer provided is the number of the unity layer (eg the number in the layers list) - it is not the bit value
    public static bool Contains(this LayerMask mask, int layer)
    {
        int layerBit = 1 << layer; //convert unity's layer index to the bitwise value
        //if the mask and the layer bit contain the same bit, the layer is included in the mask
        int result = mask.value & layerBit;
        return result != 0;
    }

    #endregion

    #region GameObjects

    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        if (!obj.TryGetComponent(out T comp))
            comp = obj.AddComponent<T>();

        return comp;
    }

    public static void SetLayerRecursively(this GameObject obj, int layerID)
    {
        Transform objTransform = obj.transform;
        obj.layer = layerID;

        for (int i = 0; i < objTransform.childCount; i++)
        {
            objTransform.GetChild(i).gameObject.SetLayerRecursively(layerID);
        }
    }

    #endregion

    #region Transforms

    public static void AddRotation2D(this Transform t, float rotation)
    {
        t.rotation = Quaternion.Euler(0, 0, t.GetRotation2D() + rotation);
    }
    public static void SetLocalRotation2D(this Transform t, float rotation)
    {
        t.localRotation = Quaternion.Euler(0, 0, rotation);
    }
    public static void SetRotation2D(this Transform t, float rotation)
    {
        t.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public static float GetRotation2D(this Transform t) => t.rotation.eulerAngles.z;

    public static Vector3 With(this Vector3 vec, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vec.x, y ?? vec.y, z ?? vec.z);
    }

    public static void CopyLocal(this Transform target, Transform source)
    {
        target.localPosition = source.localPosition;
        target.localRotation = source.localRotation;
        target.localScale = source.localScale;
    }

    public static void CopyWorld(this Transform target, Transform source)
    {
        target.position = source.position;
        target.rotation = source.rotation;
        target.localScale = source.localScale;
    }

    //transform.childcount doesnt include grandchildren etc. So this loops through all children and recursively sums
    //up all their children (including their childrens children, etc). 
    public static int GetChildCountRecursive(this Transform target)
    {
        int count = 0;
        for (int i = 0; i < target.childCount; i++)
        {
            count += 1 + target.GetChild(i).GetChildCountRecursive();
        }
        return count;
    }

    #endregion

    #region Vectors and Colors

    //conversion matrix for any point to a 45 degree isometric perspective
    static Matrix4x4 IsoConversionMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 vector)
    {
        return IsoConversionMatrix.MultiplyPoint3x4(vector);
    }

    public static float DistanceSquared(this Vector2 vec, Vector2 target)
    {
        return (target - vec).sqrMagnitude;
    }

    public static float DistanceSquared(this Vector3 vec, Vector3 target)
    {
        return (target - vec).sqrMagnitude;
    }

    public static bool CloseTo(this Vector2 vec, Vector2 target, float range = 0.05f)
    {
        float distanceLimit = range * range;
        return (target - vec).sqrMagnitude < distanceLimit;
    }

    public static bool CloseTo(this Vector3 vec, Vector3 target, float range = 0.05f)
    {
        float distanceLimit = range * range;
        return (target - vec).sqrMagnitude < distanceLimit;
    }

    public static Vector2 With(this Vector2 vec, float? x = null, float? y = null)
    {
        return new Vector2(x ?? vec.x, y ?? vec.y);
    }

    public static Vector3 ToVector3(this Vector2Int vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }

    public static Color With(this Color c, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        return new Color(r ?? c.r, g ?? c.g, b ?? c.b, a ?? c.a);
    }

    #endregion

    #region Float

    public static bool AlmostEqual(this float f, float target, float margin = 0.000001f)
    {
        return Mathf.Abs(f - target) < margin;
    }

    public static float MoveTowards(this float f, float target, float moveSpeed)
    {
        return f > target ? Mathf.Max(f - moveSpeed, target) : Mathf.Min(f + moveSpeed, target);
    }
    #endregion


}
