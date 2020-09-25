using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class speak : MonoBehaviour
{
    [SerializeField] private Image _imageSound;
    [SerializeField] private Text TextVol;
    public float sensitivity = 100;
    public float loudness = 0;
    public float pitch = 0;
    AudioSource _audio;


    public float RmsValue;
    public float DbValue;
    public float PitchValue;
    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;
    float[] _samples;
    private float[] _spectrum;
    private float _fSample;
    //Written in part by Benjamin Outram 
    //option to toggle the microphone listenter on startup or not 
    public bool startMicOnStartup = true;
    //allows start and stop of listener at run time within the unity editor 
    public bool stopMicrophoneListener = false;
    public bool startMicrophoneListener = false;
    private bool microphoneListenerOn = false;
    //public to allow temporary listening over the speakers if you want of the mic output 
    //but internally it toggles the output sound to the speakers of the audiosource depending 
    //on if the microphone listener is on or off 
    public bool disableOutputSound = false;
    //an audio source also attached to the same object as this script is 
    AudioSource src;
    public AudioMixer masterMixer;
    float timeSinceRestart = 0;


    public Text decibel_text;
    void Start()
    {
        //start the microphone listener 
        if (startMicOnStartup)
        {
            RestartMicrophoneListener();
            StartMicrophoneListener();
            _audio = GetComponent<AudioSource>();
            _audio.clip = Microphone.Start(null, true, 10, 44100);
            _audio.loop = true;
            while (!(Microphone.GetPosition(null) > 0)) { }
            _audio.Play();
            _samples = new float[QSamples];
            _spectrum = new float[QSamples];
            _fSample = AudioSettings.outputSampleRate;
            //유니티 5.x 부터는 audio source에서 mute를 하면 정상적으로 음성이 안나온다. 
            //audio mixer에서 master volume의 db를 -80으로 하여 소리 출력만 안되도록 하면 된다. 
            //_audio.mute = true; 
        }
    }
    void Update()
    {
        //can use these variables that appear in the inspector, or can call the public functions directly from other scripts 
        if (stopMicrophoneListener)
        {
            StopMicrophoneListener();
        }
        if (startMicrophoneListener)
        {
            StartMicrophoneListener();
        } //reset paramters to false because only want to execute once 
        stopMicrophoneListener = false;
        startMicrophoneListener = false;
        //must run in update otherwise it doesnt seem to work 
        MicrophoneIntoAudioSource(microphoneListenerOn);
        //can choose to unmute sound from inspector if desired 
        DisableSound(!disableOutputSound);
        loudness = GetAveragedVolume() * sensitivity;
        GetPitch();

        // decibel_text.text = "데시벨 : " + (loudness * 100000).ToString();
        //아래는 커스텀 한 소스 //소리가 
        //if (loudness > 5f) _imageSound.fillAmount = 1f; 
        //else { _imageSound.fillAmount = 0.65f; } 
        //FindObjectOfType<GameManager>().currentLoud = loudness; 
        //TextVol.text = "vol:" + loudness; 
    }
    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        _audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }

    void GetPitch()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 0);
        // fill array with samples 
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i];
            // sum squared samples 
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average 
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB 
        if (DbValue < -160) DbValue = -160;
        // clamp it to -160dB min // get sound spectrum 
        GetComponent<AudioSource>().GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max 
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold)) continue;
            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max 
        }
        float freqN = maxN; // pass the index to a float variable 
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours 
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency 
    } //stops everything and returns audioclip to null 

    public void StopMicrophoneListener()
    { //stop the microphone listener 
        microphoneListenerOn = false; //reenable the master sound in mixer 
        disableOutputSound = false; //remove mic from audiosource clip 
        src.Stop();
        src.clip = null;
        Microphone.End(null);
    }

    public void StartMicrophoneListener()
    { //start the microphone listener 
        microphoneListenerOn = true; //disable sound output (dont want to hear mic input on the output!) 
        disableOutputSound = true; //reset the audiosource 
        RestartMicrophoneListener();
    }
    //controls whether the volume is on or off, use "off" for mic input (dont want to hear your own voice input!) //and "on" for music input 

    public void DisableSound(bool SoundOn)
    {
        float volume = 0;
        if (SoundOn) { volume = 0.0f; }
        else { volume = -80.0f; }
        masterMixer.SetFloat("MasterVolume", volume);
    }

    // restart microphone removes the clip from the audiosource 
    public void RestartMicrophoneListener()
    {
        src = GetComponent<AudioSource>(); //remove any soundfile in the audiosource 
        src.clip = null;
        timeSinceRestart = Time.time;
    } //puts the mic into the audiosource 

    void MicrophoneIntoAudioSource(bool MicrophoneListenerOn)
    {
        if (MicrophoneListenerOn)
        {
            //pause a little before setting clip to avoid lag and bugginess 
            if (Time.time - timeSinceRestart > 0.5f && !Microphone.IsRecording(null))
            {
                src.clip = Microphone.Start(null, true, 10, 44100);
                //wait until microphone position is found (?)
                while (!(Microphone.GetPosition(null) > 0)) { }
                src.Play(); // Play the audio source 
            }
        }
    }
}