using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

public class NetworkBufferTest
{
    private byte[] ByteCopy(NetworkBuffer buffer)
    {
        byte[] data = buffer.GetByte();
        byte[] copy = new byte[data.Length];
        Array.Copy(data, copy, data.Length);
        return copy;
    }

    [Test]
    public void ReadByteTest()
    {
        NetworkBuffer buffer = new NetworkBuffer();

        byte a = 1;
        buffer.Write(a);

        byte[] copy = ByteCopy(buffer);
        buffer.SetBytes(copy);

        byte b = buffer.ReadByte();

        Assert.AreEqual(a, b);
    }
}
