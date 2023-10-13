using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class SoundManager 
{
    #region Enums & Variables
    public enum SoundType
    {
        Bgm,
        Sfx
    }
    public enum Bgms
    {
        Bgm_Main
    }
    public enum Sfxs
    {
        Sfx_Attach,
        Sfx_Button,
        Sfx_GameOver,
        Sfx_LevelUp_A,
        Sfx_LevelUp_B,
        Sfx_LevelUp_C,
        Sfx_Next
    }

    AudioSource[] _sources;
    string[] bgmNames;
    string[] sfxNames;
    #endregion

    #region Initialize
    public void Init()
    {
        GameObject sources = new GameObject() { name = "sources" };
        UnityEngine.Object.DontDestroyOnLoad(sources);

        string[] names = Enum.GetNames(typeof(SoundType));
        _sources = new AudioSource[names.Length];
        for (int i = 0; i < names.Length; i++) 
        {
            GameObject s = new GameObject() { name = names[i] + "Source" };
            _sources[i] = s.AddComponent<AudioSource>();
        }
        // Bgm 소스 루프 설정
        _sources[0].loop = true;

        bgmNames = Enum.GetNames(typeof(Bgms));
        sfxNames = Enum.GetNames(typeof(Sfxs));
    }
    #endregion

    #region Play & Stop
    void Play(SoundType type, int idx)
    {
        AudioSource source = _sources[(int)type];

        switch(type)
        {
            case SoundType.Bgm:
                Managers.Resc.Load<AudioClip>(bgmNames[idx], (clip) =>
                {
                    source.clip = clip;
                    source.Play();
                });
                break;
            case SoundType.Sfx:
                Managers.Resc.Load<AudioClip>(sfxNames[idx], (clip) =>
                {
                    source.PlayOneShot(clip);
                });
                break;
        }
    }
    public void PlayBgm(int idx) { Play(SoundType.Bgm, idx); }
    public void PlaySfx(int idx) { Play(SoundType.Sfx, idx); }
    public void PlayRandomUpgradeSfx() 
    { 
        int idx = Random.Range((int)Sfxs.Sfx_LevelUp_A, (int)Sfxs.Sfx_LevelUp_C + 1);
        Play(SoundType.Sfx, idx); 
    }

    public void Pause()
    {
        for(int i = 0; i < _sources.Length; i++)
            _sources[i].Pause();
    }

    public void PauseOff()
    {
        for (int i = 0; i < _sources.Length; i++)
            _sources[i].UnPause();
    }
    #endregion
}