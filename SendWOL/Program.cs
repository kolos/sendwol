using System;
using System.Globalization;
using System.Net.Sockets;

namespace SendWOL
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1 || args[0].Length < 6)
            {
                Console.WriteLine("Usage: sendwol target_mac_address [broadcast_address]");
                return;
            }

            var macAddr = args[0].Replace("-", "").Replace(":", "");
            var bcastAddr = args.Length > 1 ? args[1] : "255.255.255.255";

            byte[] targetAddr = new byte[6];
            for (var i = 0; i < targetAddr.Length; i++)
            {
                targetAddr[i] = byte.Parse(macAddr.Substring(i*2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            byte[] magicPacket = new byte[102];
            for (byte i = 0; i < 6; i++)
            {
                magicPacket[i] = 0xFF;
            }
            for (byte i = 1; i <= 16; i++)
            {
                Array.Copy(targetAddr, 0, magicPacket, i * 6, targetAddr.Length);
            }

            using (UdpClient udp = new UdpClient())
            {
                udp.Send(magicPacket, magicPacket.Length, bcastAddr, 9);
            }
        }
    }
}
