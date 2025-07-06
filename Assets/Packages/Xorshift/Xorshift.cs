using System.Runtime.CompilerServices;
using UnityEngine;

public class Xorshift
{
    // NOTE:
    // https://www.jstatsoft.org/article/view/v008i14
    // This is implement of xor128 case.
    // There is a little biased in a small number of trials.

    #region Field

    public static readonly Xorshift Instance = new (SeedW);

    private const float RadianMin = 0;
    private const float RadianMax = 6.283185307179586f;

    private const float UintMax = uint.MaxValue;

    private const uint SeedX = 123456789u;
    private const uint SeedY = 362436069u;
    private const uint SeedZ = 521288629u;
    private const uint SeedW = 88675123u;

    private uint _x, _y, _z, _w;

    #endregion Field

    #region Property

    public uint ValueNative
    {
        // NOTE:
        // Return 0 ~ 2^32-1(uint.MaxValue) value.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ValueNative / UintMax;
    }

    public float Radian
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Range(RadianMin, RadianMax);
    }

    public int Sign
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Range(0, 2) == 0 ? -1 : 1;
    }

    public Vector2 OnUnitCircle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var radian = Radian;
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }

    public Vector2 InsideUnitCircle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var radian = Radian;
            var radius = Value;
            return new Vector2(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius);
        }
    }

    public Vector3 OnUnitSphere
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => OnUnitSphere * Value;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Range(int min, int max)
    {
        return (int)(min + (max - min) * Value - 0.5f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Range(float min, float max)
    {
        return min + (max - min) * Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Range(Vector2 range)
    {
        return Range(range.x, range.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Range(Vector2 min, Vector2 max)
    {
        return new Vector2(Range(min.x, max.x), Range(min.y, max.y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Range(Vector2Int min, Vector2Int max)
    {
        return new Vector2Int(Range(min.x, max.x), Range(min.y, max.y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Range(Vector3 min, Vector3 max)
    {
        return new Vector3(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3Int Range(Vector3Int min, Vector3Int max)
    {
        return new Vector3Int(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 Range(Vector4 min, Vector4 max)
    {
        return new Vector4(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z), Range(min.w, max.w));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Range(Rect rect)
    {
        return Range(rect.min, rect.max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Range(Bounds bounds)
    {
        return Range(bounds.min, bounds.max);
    }
    
    #endregion Method
}