﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using AudioTransmiter;

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
                float[] audioData = e.Buffer.Select(i => (float)i).ToArray();
                Complex32[] complexData = audioData.Select(x => new Complex32(x, 0)).ToArray();
                Fourier.Forward(complexData);
                float[] magnitude = complexData.Select(c => c.Magnitude).ToArray();

                int sampleRate = 48000;
                int numSamples = audioData.Length;

                double frequencyResolution = (double)sampleRate / numSamples;
                double[] frequencies = new double[numSamples / 2];
                int j = 0;
                for (int i = 0; i < frequencies.Length; i++)
                {
                    frequencies[i] = i * frequencyResolution;
                }

                float maxMagnitude = 0;
                int maxIndex = 0;
                for (int i = 5; i < magnitude.Length / 2; i++)
                {
                    if (maxMagnitude < magnitude[i])
                    {
                        maxMagnitude = magnitude[i];
                        maxIndex = i;
                    }
                }

                float[] norm = FrequencyProcessor.NormalizedFrequencyArray(20, 24000, new float[] { (float)frequencies[maxIndex] });
                Console.WriteLine($"{frequencies[maxIndex]} {magnitude[maxIndex]}: RGB({FrequencyProcessor.R(norm)}, {FrequencyProcessor.G(norm)}, {FrequencyProcessor.B(norm)})");
                byte[] message = Encoding.ASCII.GetBytes($"{FrequencyProcessor.R(norm)}/{FrequencyProcessor.G(norm)}/{FrequencyProcessor.B(norm)}");
                udpClient.Send(message, message.Length);
            };
            waveIn.StartRecording();

            while (true);
        }
	}
}
