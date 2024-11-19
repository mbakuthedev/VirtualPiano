using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

class Program
{
    static Dictionary<char, float> noteFrequencies = new Dictionary<char, float>
    {
        { 'A', 261.63f }, // C4
        { 'S', 293.66f }, // D4
        { 'D', 329.63f }, // E4
        { 'F', 349.23f }, // F4
        { 'G', 392.00f }, // G4
        { 'H', 440.00f }, // A4
        { 'J', 493.88f }, // B4
        { 'K', 523.25f }, // C5
     };

    static WaveOutEvent waveOut;
    static SignalGenerator signalGenerator;

    static void Main(string[] args)
    {
        Console.WriteLine("Press A, S, D, F, G, H, J, K to play notes. Press Escape to exit.");
        
        waveOut = new WaveOutEvent();
        signalGenerator = new SignalGenerator()
        {
            Gain = 0.5, // Adjust volume
            Type = SignalGeneratorType.Sin // Use sine wave for pure tones
        };
        waveOut.Init(signalGenerator);

        // Start listening for keypresses
        Task.Run(() => ListenForKeyPresses());

        Console.ReadKey();
    }

    static void ListenForKeyPresses()
    {
        while (true)
        {
            var key = Console.ReadKey(intercept: true).KeyChar;

            if (key == (char)27) // Escape key
            {
                waveOut.Stop();
                break;
            }

            if (noteFrequencies.TryGetValue(char.ToUpper(key), out float frequency))
            {
                PlayNote(frequency);
            }
        }
    }

    static void PlayNote(float frequency)
    {
        signalGenerator.Frequency = frequency;
        waveOut.Play();
    }
}
