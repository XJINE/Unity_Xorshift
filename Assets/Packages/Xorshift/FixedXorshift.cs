using UnityEngine;

public static class FixedXorshift
{
    #region Field

    private const float RadianMin  = 0;
    private const float RadianMax  = 6.283185307179586f;
    private const float UintMax    = uint.MaxValue;
    private const uint  SeedT      = 123456789u;
    private const uint  T          = SeedT ^ (SeedT << 11);
    
    // NOTE:
    // Seed value should be large enough value.
    // If do not, such random show always quite similar value.

    public static uint Seed = 88675123u;

    #endregion Field

    #region Property

    public  static uint  ValueNative =>  (Seed ^ (Seed >> 19)) ^ (T ^ (T >> 8));
    public  static float Value       => ((Seed ^ (Seed >> 19)) ^ (T ^ (T >> 8))) / UintMax;
    public  static float Radian      => Range(RadianMin, RadianMax);
    public  static int   Sign        => Range(0, 2) == 0 ? -1 : 1; 

    public static Vector2 OnUnitCircle
    {
        get
        {
            var radian = Radian;
            return new Vector2(Mathf.Cos(radian), Mathf.Sign(radian));
        }
    }

    public static Vector2 InsideUnitCircle
    {
        get
        {
            var radian = Radian;
            var radius = Value;
            return new Vector2(Mathf.Cos(radian) * radius, Mathf.Sign(radian) * radius);
        }
    }

    public static Vector3 OnUnitSphere
    {
        get
        {
            var angle1 = Radian;
            var angle2 = Radian;
            return new Vector3(Mathf.Sin(angle1) * Mathf.Cos(angle2),
                               Mathf.Sin(angle1) * Mathf.Sin(angle2),
                               Mathf.Cos(angle1));
        }
    }

    public static Vector3 InsideUnitSphere
    {
        get
        {
            return OnUnitSphere * Value;
        }
    }

    #endregion Property

    #region Method

    public static int Range(in int min, in int max)
    {
        return (int)(min + (max - min) * Value);
    }

    public static float Range(in float min, in float max)
    {
        return min + (max - min) * Value;
    }

    public static float Range(in Vector2 range)
    {
        return Range(range.x, range.y);
    }

    public static Vector2 Range(in Vector2 min, in Vector2 max)
    {
        return new Vector2(Range(min.x, max.x), Range(min.y, max.y));
    }

    public static Vector2Int Range(in Vector2Int min, in Vector2Int max)
    {
        return new Vector2Int(Range(min.x, max.x), Range(min.y, max.y));
    }

    public static Vector3 Range(in Vector3 min, in Vector3 max)
    {
        return new Vector3(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
    }

    public static Vector3Int Range(in Vector3Int min, in Vector3Int max)
    {
        return new Vector3Int(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
    }

    public static Vector4 Range(in Vector4 min, in Vector4 max)
    {
        return new Vector4(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z), Range(min.w, max.w));
    }

    public static Vector2 Range(in Rect rect)
    {
        return Range(rect.min, rect.max);
    }

    public static Vector3 Range(in Bounds bounds)
    {
        return Range(bounds.min, bounds.max);
    }
    
    #endregion Method
}