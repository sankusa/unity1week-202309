using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using SankusaLib.SoundLib;

[System.Serializable]
public class PlaySe : PlayableAsset
{
    [SerializeField, SoundId] private string seId;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        PlaySeBehaviour playSe = new PlaySeBehaviour();
        playSe.seId = seId;
        return ScriptPlayable<PlaySeBehaviour>.Create(graph, playSe);
    }
}
