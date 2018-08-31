using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBuffer
{
    protected byte[] _buffer;

    protected int _pos;
    private const int _defaultSize = 1024;
    private const float GrowthFactor = 2f;
    private const int _buffersizeWarning = 1024 * 1024 * 128;

    private static UIntFloat s_FloatConverter;

    public int Position { get { return _pos; } }

    public NetworkBuffer()
    {
        _buffer = new byte[_defaultSize];
    }

    public NetworkBuffer(int bufferSize)
    {
        _buffer = new byte[bufferSize];
    }

    public NetworkBuffer(byte[] buffer)
    {
        _buffer = buffer;
    }

    public byte ReadByte()
    {
        if (_pos >= _buffer.Length)
        {
            throw new IndexOutOfRangeException("NetworkReader:ReadByte out of range: " + ToString());
        }

        return _buffer[_pos++];
    }

    public void ReadBytes(byte[] outBuffer, int count)
    {
        if (_pos + count > _buffer.Length)
        {
            throw new IndexOutOfRangeException("NetworkReader:ReadBytes out of range: (" + count + ") " + ToString());
        }

        for (ushort i = 0; i < count; i++)
        {
            outBuffer[i] = _buffer[_pos + i];
        }

        _pos += count;
    }

    public void ReadChars(char[] outBuffer, int count)
    {
        if (_pos + count > _buffer.Length)
        {
            throw new IndexOutOfRangeException("NetworkReader:ReadChars out of range: (" + count + ") " + ToString());
        }

        for (ushort i = 0; i < count; i++)
        {
            outBuffer[i] = (char)_buffer[_pos + i];
        }

        _pos += count;
    }

    public sbyte ReadSByte()
    {
        return (sbyte)ReadByte();
    }

    public short ReadShort()
    {
        return ReadInt16();
    }

    public ushort ReadUShort()
    {
        return ReadUInt16();
    }

    public short ReadInt16()
    {
        return (short)ReadUInt16();
    }

    public ushort ReadUInt16()
    {
        ushort value = 0;
        value |= ReadByte();
        value |= (ushort)(ReadByte() << 8);
        return value;
    }

    public int ReadInt()
    {
        return ReadInt32();
    }

    public uint ReadUInt()
    {
        return ReadUInt32();
    }

    public int ReadInt32()
    {
        return (int)ReadUInt32();
    }

    public uint ReadUInt32()
    {
        uint value = 0;
        value |= ReadByte();
        value |= (uint)(ReadByte() <<  8);
        value |= (uint)(ReadByte() << 16);
        value |= (uint)(ReadByte() << 24);
        return value;
    }

    public long ReadLong()
    {
        return ReadInt64();
    }

    public ulong ReadULong()
    {
        return ReadUInt64();
    }

    public long ReadInt64()
    {
        return (long)ReadUInt64();
    }

    public ulong ReadUInt64()
    {
        ulong value = 0;
        value |= ReadByte();
        value |= ((ulong)ReadByte() <<  8);
        value |= ((ulong)ReadByte() << 16);
        value |= ((ulong)ReadByte() << 24);
        value |= ((ulong)ReadByte() << 32);
        value |= ((ulong)ReadByte() << 40);
        value |= ((ulong)ReadByte() << 48);
        value |= ((ulong)ReadByte() << 56);
        return value;
    }

    public float ReadFloat()
    {
        uint value = ReadUInt32();
        return ConversionUtil.ToFloat(value);
    }

    public double ReadDouble()
    {
        ulong value = ReadUInt64();
        return ConversionUtil.ToDouble(value);
    }

    public char ReadChar()
    {
        return (char)ReadByte();
    }

    public string ReadString()
    {
        int length = ReadInt();
        var value = System.Text.Encoding.UTF8.GetString(_buffer, _pos, length);
        _pos += length;
        return value;
    }

    public bool ReadBool()
    {
        int value = ReadByte();
        return value == 1;
    }

    public Vector2 ReadVector2()
    {
        return new Vector2(ReadFloat(), ReadFloat());
    }

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Vector4 ReadVector4()
    {
        return new Vector4(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Color ReadColor()
    {
        return new Color(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Color32 ReadColor32()
    {
        return new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());
    }

    public Quaternion ReadQuaternion()
    {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Rect ReadRect()
    {
        return new Rect(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Plane ReadPlane()
    {
        return new Plane(ReadVector3(), ReadFloat());
    }

    public Ray ReadRay()
    {
        return new Ray(ReadVector3(), ReadVector3());
    }

    public Matrix4x4 ReadMatrix4x4()
    {
        Matrix4x4 m = new Matrix4x4();
        m.m00 = ReadFloat();
        m.m01 = ReadFloat();
        m.m02 = ReadFloat();
        m.m03 = ReadFloat();
        m.m10 = ReadFloat();
        m.m11 = ReadFloat();
        m.m12 = ReadFloat();
        m.m13 = ReadFloat();
        m.m20 = ReadFloat();
        m.m21 = ReadFloat();
        m.m22 = ReadFloat();
        m.m23 = ReadFloat();
        m.m30 = ReadFloat();
        m.m31 = ReadFloat();
        m.m32 = ReadFloat();
        m.m33 = ReadFloat();
        return m;
    }

    public void WriteByte(byte value)
    {
        WriteCheckForSpace(1);
        _buffer[_pos] = value;
        _pos += 1;
    }

    protected void WriteByte2(byte value0, byte value1)
    {
        WriteCheckForSpace(2);
        _buffer[_pos + 0] = value0;
        _buffer[_pos + 1] = value1;
        _pos += 2;
    }

    protected void WriteByte4(byte value0, byte value1, byte value2, byte value3)
    {
        WriteCheckForSpace(4);
        _buffer[_pos + 0] = value0;
        _buffer[_pos + 1] = value1;
        _buffer[_pos + 2] = value2;
        _buffer[_pos + 3] = value3;
        _pos += 4;
    }

    protected void WriteByte8(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7)
    {
        WriteCheckForSpace(8);
        _buffer[_pos + 0] = value0;
        _buffer[_pos + 1] = value1;
        _buffer[_pos + 2] = value2;
        _buffer[_pos + 3] = value3;
        _buffer[_pos + 4] = value4;
        _buffer[_pos + 5] = value5;
        _buffer[_pos + 6] = value6;
        _buffer[_pos + 7] = value7;
        _pos += 8;
    }

    protected void WriteBytesAtOffset(byte[] buffer, ushort targetOffset, ushort count)
    {
        uint newEnd = (uint)(count + targetOffset);

        WriteCheckForSpace((ushort)newEnd);

        if (targetOffset == 0 && count == buffer.Length)
        {
            buffer.CopyTo(_buffer, _pos);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                _buffer[targetOffset + i] = buffer[i];
            }
        }

        if (newEnd > _pos)
        {
            _pos = (int)newEnd;
        }
    }

    protected void WriteBytes(byte[] buffer, ushort count)
    {
        WriteCheckForSpace(count);

        if (count == buffer.Length)
        {
            buffer.CopyTo(_buffer, _pos);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                _buffer[_pos + i] = buffer[i];
            }
        }

        _pos += count;
    }

    public void Write(char value)
    {
        WriteByte((byte)value);
    }

    public void Write(string data)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(data);
        Write(bytes.Length);
        WriteBytes(bytes, (ushort)bytes.Length);
    }

    public void Write(byte value)
    {
        WriteByte(value);
    }

    public void Write(sbyte value)
    {
        WriteByte((byte)value);
    }

    public void Write(short value)
    {
        WriteByte2((byte)(value & 0xff), (byte)((value >> 8) & 0xff));
    }

    public void Write(int value)
    {
        Write((uint)value);
    }

    public void Write(uint value)
    {
        WriteByte4(
            (byte)((value >>  0) & 0xff),
            (byte)((value >>  8) & 0xff),
            (byte)((value >> 16) & 0xff),
            (byte)((value >> 24) & 0xff)
        );
    }

    public void Write(long value)
    {
        Write((ulong)value);
    }

    public void Write(ulong value)
    {
        WriteByte8(
            (byte)((value >>  0) & 0xff),
            (byte)((value >>  8) & 0xff),
            (byte)((value >> 16) & 0xff),
            (byte)((value >> 24) & 0xff),
            (byte)((value >> 32) & 0xff),
            (byte)((value >> 40) & 0xff),
            (byte)((value >> 48) & 0xff),
            (byte)((value >> 56) & 0xff)
        );
    }

    public void Write(float value)
    {
        s_FloatConverter.floatValue = value;
        Write(s_FloatConverter.intValue);
    }

    public void Write(double value)
    {
        s_FloatConverter.doubleValue = value;
        Write(s_FloatConverter.longValue);
    }

    public void Write(bool value)
    {
        if (value)
        {
            WriteByte(1);
        }
        else
        {
            WriteByte(0);
        }
    }

    public void Write(byte[] buffer, int count)
    {
        if (count > UInt16.MaxValue)
        {
            Debug.LogError("NetworkWriter Write: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes.");
            return;
        }

        WriteBytes(buffer, (UInt16)count);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        if (count > UInt16.MaxValue)
        {
            Debug.LogError("NetworkWriter Write: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes.");
            return;
        }

        WriteBytesAtOffset(buffer, (ushort)offset, (ushort)count);
    }

    protected void WriteBytesAndSize(byte[] buffer, int count)
    {
        if (buffer == null || count == 0)
        {
            Write((UInt16)0);
            return;
        }

        if (count > UInt16.MaxValue)
        {
            Debug.LogError("NetworkWriter WriteBytesAndSize: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes.");
            return;
        }

        Write((UInt16)count);
        WriteBytes(buffer, (UInt16)count);
    }

    protected void WriteBytesFull(byte[] buffer)
    {
        if (buffer == null)
        {
            Write((UInt16)0);
            return;
        }

        if (buffer.Length > UInt16.MaxValue)
        {
            Debug.LogError("NetworkWriter WriteBytes: buffer is too large (" + buffer.Length + ") bytes. The maximum buffer size is 64K bytes.");
            return;
        }

        Write((UInt16)buffer.Length);
        WriteBytes(buffer, (UInt16)buffer.Length);
    }

    public void Write(Vector2 value)
    {
        Write(value.x);
        Write(value.y);
    }

    public void Write(Vector3 value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
    }

    public void Write(Vector4 value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
        Write(value.w);
    }

    public void Write(Color value)
    {
        Write(value.r);
        Write(value.g);
        Write(value.b);
        Write(value.a);
    }

    public void Write(Color32 value)
    {
        Write(value.r);
        Write(value.g);
        Write(value.b);
        Write(value.a);
    }

    public void Write(Quaternion value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
        Write(value.w);
    }

    public void Write(Rect value)
    {
        Write(value.xMin);
        Write(value.yMin);
        Write(value.width);
        Write(value.height);
    }

    public void Write(Plane value)
    {
        Write(value.normal);
        Write(value.distance);
    }

    public void Write(Ray value)
    {
        Write(value.direction);
        Write(value.origin);
    }
    public void Write(Matrix4x4 value)
    {
        Write(value.m00);
        Write(value.m01);
        Write(value.m02);
        Write(value.m03);
        Write(value.m10);
        Write(value.m11);
        Write(value.m12);
        Write(value.m13);
        Write(value.m20);
        Write(value.m21);
        Write(value.m22);
        Write(value.m23);
        Write(value.m30);
        Write(value.m31);
        Write(value.m32);
        Write(value.m33);
    }

    public void WriteCheckForSpace(ushort count)
    {
        if (_pos + count <= _buffer.Length)
        {
            return;
        }

        int newLen = (int)(_buffer.Length * GrowthFactor);
        while (_pos + count <= newLen)
        {
            newLen = (int)(newLen * GrowthFactor);
            if (newLen > _buffersizeWarning)
            {
                Debug.LogWarning("NetworkBuffer size is " + newLen + " bytes!");
            }
        }

        byte[] tmp = new byte[newLen];
        _buffer.CopyTo(tmp, 0);
        _buffer = tmp;
    }
}
