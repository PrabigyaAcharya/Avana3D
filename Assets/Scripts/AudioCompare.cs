using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using MathNet;
using System.Net.NetworkInformation;

public class AudioCompare : MonoBehaviour
{

    bool playing = false;
    bool played1 = false;
    bool played2 = false;
    bool playing1 = false;
    bool playing2 = false;
    bool comparing = false;
    bool finished_comparing = false;

    public AudioSource source1;
    public AudioSource source2;

    public float[] spec1 = new float[512];
    public float[] spec2 = new float[512];

    public List<float> peak1 = new List<float>();
    public List<float> peak2 = new List<float>();

    public TextMeshProUGUI _text;




    // Start is called before the first frame update
    void Start()
    {

    }

    public void onCompareButtonClick()
    {
        //play the original audio (its hidden in an empty object), just need to play it
        //same as the guitar strings
        comparing = true;
    }

    float[] MakeFrequencyBands(float[] sample)
    {
        float[] _freq = new float[8];
        int current_sample = 0;
        for (int i =0; i<8; i++)
        {

            float average_amplitude = 0;
            int sample_count = (int)MathF.Pow(2, i+1);

            if (i == 7)
            {
                sample_count += 2;
            }
        
            for (int j = 0; j<sample_count; j++)
            {
                average_amplitude += sample[current_sample];
                current_sample++;
            }

            average_amplitude /= current_sample;
            _freq[i] = average_amplitude * 1000;
        }

        return _freq;
    }

    // Update is called once per frame
    void Update()
    {
        if (!comparing)
        {
            _text.text = "";
            return;
        }

        if (comparing && !played1 && !playing)
        {
            _text.text = "Playing First Sound";
            Debug.Log("Playing source1");
            source1.pitch = 1;
            source1.Play();
            source1.GetSpectrumData(spec1, 0, FFTWindow.BlackmanHarris);
            peak1.Add(spec1.Max());

            playing = true;
            playing1 = true;
        }

        if (comparing && playing && source1.isPlaying)
        {
            source1.GetSpectrumData(spec1, 0, FFTWindow.BlackmanHarris);
            peak1.Add(spec1.Max());


        }

        if (!playing2 && playing && !source1.isPlaying)
        {
            played1 = true;
            source1.Stop();
            Debug.Log("Stopped source 1");
            Debug.Log("Playing source2");
            _text.text = "Playing Second Sound";
            source2.pitch = 1;
            source2.Play();
            playing1 = false;
            playing2= true;
            source2.GetSpectrumData(spec2, 0, FFTWindow.BlackmanHarris);
            peak2.Add(spec2.Max());

        }

        if (!playing1 && playing && !played2 && source2.isPlaying)
        {
            source2.GetSpectrumData(spec2, 0, FFTWindow.BlackmanHarris);
            peak2.Add(spec2.Max());

        }

        if (!playing1 && playing && !played2 && !source2.isPlaying)
        {
            Debug.Log("Stopped source 2");
            source2.Stop();
            playing = false;
            played2 = true;
            playing2= false;
            finished_comparing = true;
        }

        if (finished_comparing)
        {
            var res = calculateSimilarity(spec1, spec2);
            finished_comparing = false;
            _text.text = "Similarity: " + ((float)res).ToString("f2");
            
        }
    }

    double calculateSimilarity(float[] A, float[] B)
    {
        double fs = 50; //sampling rate, Hz
        double te = 1; //end time, seconds
        int size = (int)(fs * te); //sample size
        double[] x1 = Array.ConvertAll(A, x => (double)x);
        double[] y1 = Array.ConvertAll(B, x => (double)x);
        var r12 = StatsHelper.CrossCorrelation(x1, y1); // Y1 * Y2
        var r21 = StatsHelper.CrossCorrelation(x1, y1); // Y2 * Y1
        var r11 = StatsHelper.CrossCorrelation(x1, y1);
        //int lowerBound = Mathf.Clamp((int)convertFrequencyToIndex(30, 1024) - 5, 0, 1024);
        //int higherBound = Mathf.Clamp((int)convertFrequencyToIndex(400, 1024) + 5, 0, 1024);
        //float[] vectorA = new float[higherBound - lowerBound];
        //float[] vectorB = new float[higherBound - lowerBound];
        //for (int i = lowerBound; i < higherBound; i++)
        //{
        //    vectorA[i - lowerBound] = A[i];
        //    vectorB[i - lowerBound] = B[i];
        //}
        //float vecAMax = vectorA.Max();
        //float vecBMax = vectorB.Max();
        //float sum = 0;
        //int j = 0;
        //for (int i = lowerBound; i < higherBound; i++)
        //{
        //    vectorA[i] /= vecAMax;
        //    vectorB[i] /= vecBMax;
        //    if (vectorA[i] < 0.1 || vectorB[i] < 0.1)
        //    {
        //        continue;
        //    }
        //    float temp = vectorA[i] - vectorB[i];

        //    temp = Mathf.Abs(temp);
        //    temp = temp * 100;
        //    if (temp > 100)
        //    {
        //        temp = 100;
        //    }
        //    if (temp < 0)
        //    {
        //        temp = 0;
        //    }
        //    sum += temp;
        //    j++;

        //}

        //if (j == 0)
        //    return 0;
        //return sum / j;
        Debug.Log(r12.Corr.Max());
        Debug.Log(r21.Corr.Max());
        Debug.Log(r11.Corr.Max());
        return r21.Corr.Max();
        //var band_a = MakeFrequencyBands(A);
        //var band_b = MakeFrequencyBands(B);

        //float[] band_c = new float[8];

        //for(int i =0; i<band_a.Length; i++)
        //{
        //    band_c[i] = band_a[i] - band_b[i];
        //}


         


    }

    float convertFrequencyToIndex(float frequency, int sizeOfArray)
    {
        return 2 * sizeOfArray * frequency / (AudioSettings.outputSampleRate);
    }

    void playAudio(AudioSource src)
    {
        Debug.Log("Playing audio: " + src.name);
        src.pitch = 1;
        src.Play();
        playing = true;
        if(!src.isPlaying)
        {
            playing = false;
            Debug.Log("Stopped Playing: " + src.name);
        }
    }
}

public static class StatsHelper
{
    public static LagCorr CrossCorrelation(double[] x1, double[] x2)
    {
        if (x1.Length != x2.Length)
            throw new Exception("Samples must have same size.");

        var len = x1.Length;
        var len2 = 2 * len;
        var len3 = 3 * len;
        var s1 = new double[len3];
        var s2 = new double[len3];
        var cor = new double[len2];
        var lag = new double[len2];

        Array.Copy(x1, 0, s1, len, len);
        Array.Copy(x2, 0, s2, 0, len);

        for (int i = 0; i < len2; i++)
        {
            cor[i] = MathNet.Numerics.Statistics.Correlation.Pearson(s1, s2);
            lag[i] = i - len;
            Array.Copy(s2, 0, s2, 1, s2.Length - 1);
            s2[0] = 0;
        }

        return new LagCorr { Corr = cor, Lag = lag };
    }
}

public class LagCorr
{
    public double[] Lag { get; set; }
    public double[] Corr { get; set; }
}