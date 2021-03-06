﻿using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
public struct UIntFloat
{
    [FieldOffset(0)]
    public uint intValue;

    [FieldOffset(0)]
    public float floatValue;

    [FieldOffset(0)]
    public double doubleValue;

    [FieldOffset(0)]
    public ulong longValue;
}

/// <summary>
/// 値をByte列に、Byte列を値に変換する
/// </summary>
public class ConversionUtil
{
    static public float ToFloat(uint value)
    {
        UIntFloat uif = new UIntFloat();
        uif.intValue = value;
        return uif.floatValue;
    }

    static public float ToFloat(byte[] values)
    {
        uint x = 0;

        if (System.BitConverter.IsLittleEndian)
        {
            x |= ((uint)values[3] <<  0);
            x |= ((uint)values[2] <<  8);
            x |= ((uint)values[1] << 16);
            x |= ((uint)values[0] << 24);
        }
        else
        {
            x |= ((uint)values[0] <<  0);
            x |= ((uint)values[1] <<  8);
            x |= ((uint)values[2] << 16);
            x |= ((uint)values[3] << 24);
        }

        UIntFloat uif = new UIntFloat();
        uif.intValue = x;
        return uif.floatValue;
    }

    static public double ToDouble(ulong value)
    {
        UIntFloat uif = new UIntFloat();
        uif.longValue = value;
        return uif.doubleValue;
    }

    static public double ToDouble(byte[] values)
    {
        ulong x = 0;

        if (System.BitConverter.IsLittleEndian)
        {
            x |= ((ulong)values[7] <<  0);
            x |= ((ulong)values[6] <<  8);
            x |= ((ulong)values[5] << 16);
            x |= ((ulong)values[4] << 24);
            x |= ((ulong)values[3] << 32);
            x |= ((ulong)values[2] << 40);
            x |= ((ulong)values[1] << 48);
            x |= ((ulong)values[0] << 56);
        }
        else
        {
            x |= ((ulong)values[0] <<  0);
            x |= ((ulong)values[1] <<  8);
            x |= ((ulong)values[2] << 16);
            x |= ((ulong)values[3] << 24);
            x |= ((ulong)values[4] << 32);
            x |= ((ulong)values[5] << 40);
            x |= ((ulong)values[6] << 48);
            x |= ((ulong)values[7] << 56);
        }

        UIntFloat uif = new UIntFloat();
        uif.longValue = x;
        return uif.doubleValue;
    }

    /// <summary>
    /// Floatの値をByte列に変換、ビッグエンディアンとして返す
    /// </summary>
    /// <param name="value">変換するfloat値</param>
    /// <returns>ビッグエンディアンのByte配列</returns>
    static public byte[] ToBytes(float value)
    {
        UIntFloat uif = new UIntFloat();
        uif.floatValue = value;

        uint x = uif.intValue;
        byte a = (byte)((x >>  0) & 0xff);
        byte b = (byte)((x >>  8) & 0xff);
        byte c = (byte)((x >> 16) & 0xff);
        byte d = (byte)((x >> 24) & 0xff);

        if (System.BitConverter.IsLittleEndian)
        {
            return new[] { d, c, b, a };
        }
        else
        {
            return new[] { a, b, c, d };
        }
    }

    /// <summary>
    /// Doubleの値をByte列に変換、ビッグエンディアンとして返す
    /// </summary>
    /// <param name="value">変換するdouble値</param>
    /// <returns>ビッグエンディアンのByte配列</returns>
    static public byte[] ToBytes(double value)
    {
        UIntFloat uif = new UIntFloat();
        uif.doubleValue = value;

        ulong x = uif.longValue;
        byte a = (byte)((x >>  0) & 0xff);
        byte b = (byte)((x >>  8) & 0xff);
        byte c = (byte)((x >> 16) & 0xff);
        byte d = (byte)((x >> 24) & 0xff);
        byte e = (byte)((x >> 32) & 0xff);
        byte f = (byte)((x >> 40) & 0xff);
        byte g = (byte)((x >> 48) & 0xff);
        byte h = (byte)((x >> 56) & 0xff);

        if (System.BitConverter.IsLittleEndian)
        {
            return new[] { h, g, f, e, d, c, b, a };
        }
        else
        {
            return new[] { a, b, c, d, e, f, g, h };
        }
    }

    static public byte[] Serialize(params byte[][] bytesArray)
    {
        int total = 0;
        for (int i = 0; i < bytesArray.Length; i++)
        {
            total += bytesArray[i].Length;
        }

        byte[] result = new byte[total];
        int index = 0;
        for (int i = 0; i < bytesArray.Length; i++)
        {
            System.Array.Copy(bytesArray[i], 0, result, index, bytesArray[i].Length);
            index += bytesArray[i].Length;
        }

        return result;
    }

    static public float Deserialize(byte[] bytes, int start, int end)
    {
        int num = end - start;
        byte[] data = new byte[num];

        for (int i = 0; i < num; i++)
        {
            data[i] = bytes[start + i];
        }

        return ToFloat(data);
    }
}
