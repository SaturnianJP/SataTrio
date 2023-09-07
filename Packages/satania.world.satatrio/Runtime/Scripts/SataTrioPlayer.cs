
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDK3.Components.Video;
using VRC.SDK3.Video.Components.AVPro;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SataTrioPlayer : UdonSharp.UdonSharpBehaviour
{
    [UdonSynced]
    [SerializeField] public VRCUrl _url;

    [SerializeField] public Toggle _toggle;
    [SerializeField] public VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer VideoPlayer;
    [SerializeField] public UnityEngine.Animator Animator;
    [SerializeField] public VRCUrlInputField urlField;

    [SerializeField] private Text Status_Text;

    private int _resyncParameterHash;
    private int _videoStartParameterHash;

    public override void OnPlayerJoined( VRCPlayerApi player )
    {
        if ( Networking.IsOwner(this.gameObject) )
            RequestSerialization();
    }

    public override void OnDeserialization()
    {
        urlField.SetUrl(_url);

        Resync();
    }

    public void SendUrl()
    {
        UrlChanged();

        RequestSerialization();
    }

    public void UrlChanged()
    {
        //オーナ権限を委譲
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

        _url = urlField.GetUrl();
        //RequestSerialization();
    }

    public void GlobalSync()
    {
        //this.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(Resync));
        Animator.ResetTrigger(_videoStartParameterHash);
        Animator.SetTrigger(_resyncParameterHash);

        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        RequestSerialization();
    }

    public void Toggle_readOnly()
    {
        if ( _toggle == null )
            return;

        urlField.readOnly = !_toggle.isOn;
    }

    public void Stop()
    {
        VideoPlayer.Stop();
    }

    public void Play()
    {
        if ( urlField == null )
            return;

        VRCUrl url = urlField.GetUrl();

        VideoPlayer.PlayURL(url);
    }

    public void Start()
    {
        urlField.SetUrl(_url);

        _resyncParameterHash = UnityEngine.Animator.StringToHash("Resync");
        _videoStartParameterHash = UnityEngine.Animator.StringToHash("VideoStart");
        Animator.ResetTrigger(_videoStartParameterHash);
        Animator.SetTrigger(_resyncParameterHash);

        if (Status_Text != null)
            Status_Text.text = $"{this.name} Status: <color=red>stop</color>";
    }

    public void Resync()
    {
        Animator.ResetTrigger(_videoStartParameterHash);
        Animator.SetTrigger(_resyncParameterHash);
    }

    public override void OnVideoStart()
    {
        Animator.SetTrigger(_videoStartParameterHash);

        if (Status_Text != null)
            Status_Text.text = $"{this.name} Status: <color=green>playing</color>";
    }

    public override void OnVideoError(VideoError videoError)
    {
        if (Status_Text != null)
            Status_Text.text = $"{this.name} Status: <color=red>stop</color>";
    }

    public override void OnVideoEnd()
    {
        if (Status_Text != null)
            Status_Text.text = $"{this.name} Status: <color=red>stop</color>";
    }
}

