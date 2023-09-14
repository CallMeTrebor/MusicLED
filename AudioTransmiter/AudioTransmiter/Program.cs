using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AudioTransmiter
{
    internal class Program
    {
        static void Main()
        {
            const int serverPort = 3000;
            UdpClient udpClient = new UdpClient();
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, serverPort);

            WaveInEvent waveIn = new WaveInEvent() { WaveFormat = new WaveFormat(48000, 16, 1) };
            waveIn.DataAvailable += (sender, e) =>
            {
                byte[] audioData = e.Buffer;
                udpClient.Send(audioData, audioData.Length, broadcastEndPoint);
            };
            waveIn.StartRecording();

            while (true) ;
        }
    }
}
