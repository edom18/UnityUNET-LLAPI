using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LLAPINetworkTest : MonoBehaviour
{
    [SerializeField]
    private QosType _qosType;
    
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private int _serverPort = 10080;

    [SerializeField]
    private int _localPort = 10081;

    [SerializeField]
    private string _serverAddress = "127.0.0.1";

    [SerializeField]
    private string _localAddress = "127.0.0.1";

    private int _hostId;
    private int _connectionId;
    private int _channelId;
    private bool _isServer = false;
    private bool _isStarted = false;
    private bool _hasConnected = false;
    private bool _isConnecting = false;

    // バッファの最大サイズ
    private readonly int _maxBufferSize = 65535;

    /// <summary>
    /// ネットワークのセットアップ
    /// </summary>
    private void NetworkSetup()
    {
        Debug.Log("Setup network.");

        _isStarted = true;

        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        _channelId = config.AddChannel(_qosType);

        int maxConnections = 10;
        HostTopology topology = new HostTopology(config, maxConnections);

        _hostId = NetworkTransport.AddHost(topology, _localPort);
    }

    /// <summary>
    /// データ送信
    /// </summary>
    /// <param name="data">送信するデータ配列</param>
    public void SendData(byte[] data)
    {
        byte error;
        NetworkTransport.Send(_hostId, _connectionId, _channelId, data, data.Length, out error);
    }

    /// <summary>
    /// サーバへコネクションを張る
    /// </summary>
    private void Connect()
    {
        byte error;
        _connectionId = NetworkTransport.Connect(_hostId, _serverAddress, _serverPort, 0, out error);
    }

    /// <summary>
    /// 位置を同期する
    /// </summary>
    private void SyncPosition()
    {
        byte[] x = System.BitConverter.GetBytes(_target.position.x);
        byte[] y = System.BitConverter.GetBytes(_target.position.y);
        byte[] z = System.BitConverter.GetBytes(_target.position.z);

        byte[] pos = new byte[x.Length + y.Length + z.Length];
        System.Array.Copy(x, 0, pos, 0, x.Length);
        System.Array.Copy(y, 0, pos, x.Length, y.Length);
        System.Array.Copy(z, 0, pos, x.Length + y.Length, z.Length);

        SendData(pos);
    }

    /// <summary>
    /// 送られてきた位置情報をパースしてターゲットを移動させる
    /// </summary>
    /// <param name="pos"></param>
    private void Move(byte[] data)
    {
        Vector3 pos = Parse(data);
        _target.position = pos;
    }

    /// <summary>
    /// データ配列からpositionをパースする
    /// </summary>
    /// <param name="data">データ配列</param>
    /// <returns>パースしたVector3の位置データ</returns>
    private Vector3 Parse(byte[] data)
    {
        byte[] xbyte = new byte[4];
        System.Array.Copy(data, 0, xbyte, 0, 4);
        float x = System.BitConverter.ToSingle(xbyte, 0);

        byte[] ybyte = new byte[4];
        System.Array.Copy(data, 4, ybyte, 0, 4);
        float y = System.BitConverter.ToSingle(ybyte, 0);

        byte[] zbyte = new byte[4];
        System.Array.Copy(data, 8, zbyte, 0, 4);
        float z = System.BitConverter.ToSingle(zbyte, 0);

        return new Vector3(x, y, z);
    }

    #region ### MonoBehaviour ###
    private void Update()
    {
        if (!_isStarted)
        {
            return;
        }

        if (!_isServer && _hasConnected)
        {
            SyncPosition();
        }

        HandleEvent();
    }

    /// <summary>
    /// ネットワークイベントのハンドリング
    /// </summary>
    private void HandleEvent()
    {
        int receiveHostId;
        int connectionId;
        int channelId;
        byte[] receiveBuffer = new byte[_maxBufferSize];
        int bufferSize = _maxBufferSize;
        int dataSize;
        byte error;

        NetworkEventType receiveType = NetworkTransport.Receive(out receiveHostId, out connectionId, out channelId, receiveBuffer, bufferSize, out dataSize, out error);

        switch (receiveType)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("Connected.");

                _connectionId = connectionId;
                _hasConnected = true;
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnected.");

                _hasConnected = false;
                break;
            case NetworkEventType.DataEvent:
                if (_isServer)
                {
                    Move(receiveBuffer);
                }
                break;
            //case NetworkEventType.BroadcastEvent:
            //    break;
        }
    }

    private void OnGUI()
    {
        if (!_isStarted)
        {
            _isServer = GUI.Toggle(new Rect(10, 10, 150, 30), _isServer, "Is Server");

            GUI.Label(new Rect(10, 50, 100, 30), "LocalAddress: ");
            _localAddress = GUI.TextField(new Rect(110, 50, 150, 20), _localAddress);

            GUI.Label(new Rect(10, 90, 100, 30), "LocalPort: ");
            _localPort = int.Parse(GUI.TextField(new Rect(110, 90, 150, 20), _localPort.ToString()));

            if (GUI.Button(new Rect(10, 130, 150, 30), "Start"))
            {
                NetworkSetup();
            }
        }
        else
        {
            if (!_hasConnected)
            {
                if (!_isServer)
                {
                    GUI.Label(new Rect(10, 10, 100, 30), "ServerAddress: ");

                    _serverAddress = GUI.TextField(new Rect(110, 10, 150, 20), _serverAddress);

                    GUI.Label(new Rect(10, 50, 100, 30), "ServerPort: ");
                    _serverPort = int.Parse(GUI.TextField(new Rect(110, 50, 150, 20), _serverPort.ToString()));

                    if (GUI.Button(new Rect(10, 90, 150, 30), "Connect"))
                    {
                        Connect();
                    }
                }
            }
        }
    }
    #endregion ### MonoBehaviour ###
}
