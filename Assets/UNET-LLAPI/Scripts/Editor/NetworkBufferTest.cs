using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using UnetNetworkAPI;

public class NetworkBufferTest
{
    private byte[] ByteCopy(NetworkBuffer buffer)
    {
        byte[] data = buffer.GetByte();
        byte[] copy = new byte[data.Length];
        Array.Copy(data, copy, data.Length);
        return copy;
    }

    private void SendEmulate(NetworkBuffer buffer)
    {
        byte[] copy = ByteCopy(buffer);
        buffer.SetBytes(copy);
    }

    [Test]
    public void ByteTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        byte a = 1;
        buffer.Write(a);

        SendEmulate(buffer);

        byte b = buffer.ReadByte();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void IntTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        int a = 13412;
        buffer.Write(a);

        SendEmulate(buffer);

        int b = buffer.ReadInt();

        Assert.AreEqual(a, b);

        buffer.Clear();

        int c = -23412;
        buffer.Write(c);

        SendEmulate(buffer);

        int d = buffer.ReadInt();

        Assert.AreEqual(c, d);
    }

    [Test]
    public void ShortTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        short a = 23413;
        buffer.Write(a);

        SendEmulate(buffer);

        short b = buffer.ReadShort();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void UShortTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        ushort a = 13515;
        buffer.Write(a);

        SendEmulate(buffer);

        ushort b = buffer.ReadUShort();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void LongTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        long a = 1349134138921849124;
        buffer.Write(a);

        SendEmulate(buffer);

        long b = buffer.ReadLong();

        Assert.AreEqual(a, b);

        buffer.Clear();

        long c = -1343124123841;
        buffer.Write(c);

        SendEmulate(buffer);

        long d = buffer.ReadLong();

        Assert.AreEqual(c, d);
    }

    [Test]
    public void ULongTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        ulong a = 3349134138921849124;
        buffer.Write(a);

        SendEmulate(buffer);

        ulong b = buffer.ReadULong();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void FloatTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        float a = 1343.33934983141f;
        buffer.Write(a);

        SendEmulate(buffer);

        float b = buffer.ReadFloat();

        Assert.AreEqual(a, b);

        buffer.Clear();

        float c = -101343.33934983141f;
        buffer.Write(c);

        SendEmulate(buffer);

        float d = buffer.ReadFloat();

        Assert.AreEqual(c, d);
    }

    [Test]
    public void DoubleTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        double a = 539241343.33934983141;
        buffer.Write(a);

        SendEmulate(buffer);

        double b = buffer.ReadDouble();

        Assert.AreEqual(a, b);

        buffer.Clear();

        double c = -101343.33934983141;
        buffer.Write(c);

        SendEmulate(buffer);

        double d = buffer.ReadDouble();

        Assert.AreEqual(c, d);
    }

    [Test]
    public void BoolTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        bool a = true;
        buffer.Write(a);

        SendEmulate(buffer);

        bool b = buffer.ReadBool();

        Assert.AreEqual(a, b);

        buffer.Clear();

        bool c = false;
        buffer.Write(c);

        SendEmulate(buffer);

        bool d = buffer.ReadBool();

        Assert.AreEqual(c, d);
    }

    [Test]
    public void StringTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        string a = "hogehoge";

        buffer.Write(a);

        SendEmulate(buffer);

        string b = buffer.ReadString();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void Vector2Test()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        Vector2 a = new Vector2(1f, 3f);

        buffer.Write(a);

        SendEmulate(buffer);

        Vector2 b = buffer.ReadVector2();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void Vector3Test()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        Vector3 a = new Vector3(1f, 3f, 8f);

        buffer.Write(a);

        SendEmulate(buffer);

        Vector3 b = buffer.ReadVector3();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void Vector4Test()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        Vector4 a = new Vector4(1f, 3f, 8f, 35f);

        buffer.Write(a);

        SendEmulate(buffer);

        Vector4 b = buffer.ReadVector4();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void QuaternionTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        Quaternion a = new Quaternion(5f, 3.5f, 45f, 128.3f);

        buffer.Write(a);

        SendEmulate(buffer);

        Quaternion b = buffer.ReadQuaternion();

        Assert.AreEqual(a, b);
    }

    [Test]
    public void Matrix4x4Test()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        Vector4 c1 = new Vector4(1f, 2f, 3f, 4f);
        Vector4 c2 = new Vector4(1f, 2f, 3f, 4f);
        Vector4 c3 = new Vector4(1f, 2f, 3f, 4f);
        Vector4 c4 = new Vector4(1f, 2f, 3f, 4f);

        Matrix4x4 a = new Matrix4x4(c1, c2, c3, c4);

        buffer.Write(a);

        SendEmulate(buffer);

        Matrix4x4 b = buffer.ReadMatrix4x4();

        Assert.AreEqual(a, b);
    }
}
