using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace TcgEngine
{
    //Just a wrapper of UnityTransport to make it easier to replace with WebSocketTransport if planning to build for WebGL

    public class TcgTransport : MonoBehaviour
    {
        private UnityTransport transport;

        private const string listen_all = "0.0.0.0";

        public virtual void Init()
        {
            transport = GetComponent<UnityTransport>();
        }

        public virtual void SetServer(ushort port)
        {
            transport.ConnectionData.ServerListenAddress = listen_all;
            transport.SetConnectionData(listen_all, port);
        }

        public virtual void SetClient(string address, ushort port)
        {
            string ip = NetworkTool.HostToIP(address);
            transport.SetConnectionData(ip, port);
        }

        public virtual string GetAddress() { return transport.ConnectionData.Address; }
        public virtual ushort GetPort() { return transport.ConnectionData.Port; }
    }
}
