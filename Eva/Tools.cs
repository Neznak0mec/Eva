using NAudio.Wave;
using Serilog;
using Serilog.Core;
using Vosk;

namespace Eva;


class Tools
{
    private static readonly int sampleRate = 16000;
    private static readonly int bufferSize = 4096;
    private static readonly int volumeThreshold = 20;
    private VoskRecognizer rec;
    private String Message;

    public Tools(Model model)
    {
        rec = new VoskRecognizer(model, sampleRate);
    }

    public void Start()
    {
        Thread Recognizer = new Thread(obj =>
        {
            using (var waveIn = new WaveInEvent())
            {
                waveIn.DeviceNumber = 0;
                waveIn.WaveFormat = new WaveFormat(sampleRate, 1);
                waveIn.BufferMilliseconds = (int)((double)bufferSize / sampleRate * 1000.0);
                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.StartRecording();
                
                while (true)
                {
                    Thread.Sleep(200);
                }
            }
        });
        Recognizer.IsBackground = true;
        Recognizer.Start();
        Console.WriteLine("Say something");

        string prew = "";
        while (true)
        {
            Thread.Sleep(1000);
            if (rec.PartialResult() == prew) continue;
            prew = rec.PartialResult();
            Console.WriteLine(prew);
        }
    }

    private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
    {
        var buffer = e.Buffer;

        if (CalculateVolume(buffer) < volumeThreshold)
        {
            return;
        }

        // short[] values = new short[e.Buffer.Length / 2];
        // Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);
        //
        // // determine the highest value as a fraction of the maximum possible value
        // float fraction = (float)values.Max() / 32768;
        //
        // // print a level meter using the console
        // string bar = new('#', (int)(fraction * 70));
        // string meter = "[" + bar.PadRight(60, '-') + "]";
        // Console.CursorLeft = 0;
        // Console.CursorVisible = false;
        // Console.Write($"{meter} {fraction * 100:00.0}%");

        rec.AcceptWaveform(buffer, buffer.Length);
        

    }
    
    
    private float CalculateVolume(byte[] buffer)
    {
        float max = 0;
        for (int i = 0; i < buffer.Length; i += 2)
        {
            var sample = BitConverter.ToInt16(buffer, i);
            var sampleAbs = Math.Abs(sample);
            if (sampleAbs > max)
            {
                max = sampleAbs;
            }
        }

        return max / short.MaxValue * 100;
    }
}