using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UDPPing
{
    internal class Logic
    {
        private static readonly byte[] header = { 0x01, 0x55 };
        private UdpClient udp;

        public Logic()
        {
            Task.Run(async () =>
            {
                using (udp = new UdpClient(0))
                {
                    while (true)
                    {
                        var received = await udp.ReceiveAsync();
                        if (received.Buffer.Take(2).SequenceEqual(header))
                        {
                            var msg = System.Text.Encoding.ASCII.GetString(received.Buffer, 2, received.Buffer.Length - 2);
                            System.Console.Write(msg);
                            System.Console.Write("|");

                        }
                        else
                        {
                            System.Console.Error.WriteLine("Bad Packet Header");
                        }
                    }
                }
            });
        }

        internal async Task SendMessageAsync(string message, string address)
        {
            IPAddress ip;
            if (IPAddress.TryParse(address, out ip))
            {
                while (udp == null)
                {
                    await Task.Delay(100);
                }
                udp.Connect(ip, 1008);
                var bytes = System.Text.Encoding.ASCII.GetBytes(message);
                var send = await udp.SendAsync(header.Concat(bytes).ToArray(), bytes.Length + header.Length);
                await Task.Yield();
            }
        }

        internal async Task SendMessageAsync(string message)
        {
            while (udp == null)
            {
                await Task.Delay(100);
            }
            var bytes = System.Text.Encoding.ASCII.GetBytes(message);
            var send = await udp.SendAsync(header.Concat(bytes).ToArray(), bytes.Length + header.Length);
            await Task.Yield();
        }
    }
}