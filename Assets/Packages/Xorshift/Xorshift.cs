// NOTE:
// https://www.jstatsoft.org/article/view/v008i14
// This is implement of xor128 case.
// There is a little biased in a small number of trials.

using UnityEngine;

public class Xorshift
{
    #region Field

    private const float RadianMin = 0;
    private const float RadianMax = 6.283185307179586f;

    private const float UintMax = uint.MaxValue;

    private const uint SeedX = 123456789u;
    private const uint SeedY = 362436069u;
    private const uint SeedZ = 521288629u;
    private const uint SeedW = 88675123u;

    private uint _x, _y, _z, _w;

    public static readonly Xorshift Instance = new (SeedW);

    #endregion Field

    #region Property

    public uint ValueNative
    {
        // NOTE:
        // Return 0 ~ 2^32-1(uint.MaxValue) value.

        get
        {
            var t = _x ^ (_x << 11);

            _x = _y;
            _y = _z;
            _z = _w;
            _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));

            return _w;
        }
    }

    public float Value
    {
        // NOTE:
        // Do not use ValueNative to make this fast.

        get
        {
            var t = _x ^ (_x << 11);

            _x = _y;
            _y = _z;
            _z = _w;
            _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));

            return _w / UintMax;
        }
    }

    public float Radian => Range(RadianMin, RadianMax);
    public int   Sign   => Range(0, 2) == 0 ? -1 : 1; 

    public Vector2 OnUnitCircle
    {
        get
        {
            var radian = Radian;
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }

    public Vector2 InsideUnitCircle
    {
        get
        {
            var radian = Radian;
            var radius = Value;
            return new Vector2(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius);
        }
    }

    public Vector3 OnUnitSphere
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

    public Vector3 InsideUnitSphere
    {
        get
        {
            return OnUnitSphere * Value;
        }
    }

    #endregion Property

    #region Constructor

    public Xorshift(int seed) : this((uint)seed) { }

    public Xorshift(uint seed)
    {
        _x = SeedX;
        _y = SeedY;
        _z = SeedZ;
        _w = seed;
    }

    #endregion Constructor

    #region Method

    // NOTE:
    // Range function returns min ~ max range value.
    // If argument type is int, the result excludes max value.
    // If argument type is float, the result includes max value.

    public int Range(in int min, in int max)
    {
        return (int)(min + (max - min) * Value - 0.5f);
    }

    public float Range(in float min, in float max)
    {
        return min + (max - min) * Value;
    }

    public float Range(in Vector2 range)
    {
        return Range(range.x, range.y);
    }

    public Vector2 Range(in Vector2 min, in Vector2 max)
    {
        return new Vector2(Range(min.x, max.x), Range(min.y, max.y));
    }

    public Vector2Int Range(in Vector2Int min, in Vector2Int max)
    {
        return new Vector2Int(Range(min.x, max.x), Range(min.y, max.y));
    }

    public Vector3 Range(in Vector3 min, in Vector3 max)
    {
        return new Vector3(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
    }

    public Vector3Int Range(in Vector3Int min, in Vector3Int max)
    {
        return new Vector3Int(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
    }

    public Vector4 Range(in Vector4 min, in Vector4 max)
    {
        return new Vector4(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z), Range(min.w, max.w));
    }

    public Vector2 Range(in Rect rect)
    {
        return Range(rect.min, rect.max);
    }

    public Vector3 Range(in Bounds bounds)
    {
        return Range(bounds.min, bounds.max);
    }
    
    #endregion Method
}