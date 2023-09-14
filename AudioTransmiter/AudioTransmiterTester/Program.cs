using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AudioTransmiterTester
{
    internal class Program
    {
        static void Main()
        {
            const int listenPort = 3000; // Port to listen for UDP audio data

            // Initialize NAudio's WaveOutEvent to play audio
            WaveOutEvent waveOut = new WaveOutEvent();

            try
            {
                // Create a UDP listener to receive audio data
                UdpClient udpListener = new UdpClient(listenPort);
                IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, listenPort);

                Console.WriteLine($"Listening for audio data on port {listenPort}");

                // Initialize NAudio's WaveFormat to match the audio format you expect
                var waveFormat = new WaveFormat(48000, 16, 1); // Adjust sample rate, bit depth, and channels as needed

                // Create a BufferedWaveProvider for real-time audio playback
                BufferedWaveProvider waveProvider = new BufferedWaveProvider(waveFormat);
                waveOut.Init(waveProvider);
                waveOut.Play();

                while (true)
                {
                    byte[] audioData = udpListener.Receive(ref senderEndPoint);

                    // Write the received audio data to the playback buffer
                    waveProvider.AddSamples(audioData, 0, audioData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Dispose of resources when done
                waveOut.Dispose();
            }
        }
    }
}
