
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class syncButtons : UdonSharpBehaviour
{
    public SataTrioPlayer VideoPlayer;
    public SataTrioPlayer AudioPlayer;
    public SataTrioPlayer AudioPlayer_2;

    void Start()
    {
        
    }

    public override void OnDeserialization()
    {

    }

    public void GlobalSync()
    {
        VideoPlayer.GlobalSync();
        AudioPlayer.GlobalSync();
        AudioPlayer_2.GlobalSync();
    }

    public void VideoGlobalSync()
    {
        VideoPlayer.GlobalSync();
    }

    public void AudioGlobalSync()
    {
        AudioPlayer.GlobalSync();
    }

    public void Audio_2GlobalSync()
    {
        AudioPlayer_2.GlobalSync();
    }

    public void LocalSync()
    {
        VideoPlayer.Resync();
        AudioPlayer.Resync();
        AudioPlayer_2.Resync();
    }

    public void VideoURLGlobalSync()
    {
        VideoPlayer.SendUrl();
    }

    public void AudioURLGlobalSync()
    {
        AudioPlayer.SendUrl();
    }

    public void Audio_2URLGlobalSync()
    {
        AudioPlayer_2.SendUrl();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LocalSync();
        }
    }

    public override void OnPlayerJoined( VRCPlayerApi player )
    {
        if ( Networking.IsOwner(this.gameObject) )
            RequestSerialization();
    }
}
