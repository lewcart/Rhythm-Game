using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace RhythmGameStarter
{
    public class SongManager : MonoBehaviour
    {
        //public string songList; 
        public string[] songList;
        public int songQueue;
        public int playlistLength;

        public static SongManager INSTANCE;

        void Awake()
        {
            INSTANCE = this;
        }

        public AudioSource audioSource;

        [Header("[Properties]")]
        [Space]
        public bool playOnAwake = true;
        public SongItem defaultSong;
        public float delay;

        [Header("[Display]")]
        public bool progressAsPercentage = true;

        [HideInInspector]
        public float secPerBeat;
        [HideInInspector]
        public float songPosition;
        [HideInInspector]
        public IEnumerable<SongItem.MidiNote> currnetNotes;

        [Header("[Events]")]
        [CollapsedEvent]
        public FloatEvent onSongProgress;
        [CollapsedEvent]
        public StringEvent onSongProgressDisplay;
        [CollapsedEvent]
        public UnityEvent onSongStart;
        [CollapsedEvent]
        public UnityEvent onSongFinished;

        private bool songHasStarted;
        private bool songStartEventInvoked;

        [HideInInspector]
        public bool songPaused;

        private double dspStartTime;
        private double dspPausedTime;
        private double accumulatedPauseTime;

        void Start()
        {
            TrackManager.INSTANCE.Init();

            if (GameObject.Find("songSaver"))
            {
                songList = GameObject.Find("songSaver").GetComponent<songSaver>().songList;
                //songList = GameObject.Find("songSaver").GetComponent<songSaver>().songName;
                Debug.Log("Found Song Name");
                Destroy(GameObject.Find("songSaver"));
                defaultSong = Resources.Load<SongItem>(songList[0]);
                //defaultSong = Resources.Load<SongItem>(songList);
                playlistLength = songList.Length;
                songQueue = 0;
            }

            else
            {
                Debug.Log("Didn't find Song Name");
            }


            if (playOnAwake && defaultSong)
            {
                PlaySong(defaultSong);
            }
        }

        public void PlaySong(SongItem songItem)
        {
            songQueue += 1;
            songPaused = false;
            songHasStarted = true;
            accumulatedPauseTime = 0;
            dspPausedTime = 0;
            songPosition = -1;

            audioSource.clip = songItem.clip;

            songItem.ResetNotesState();
            currnetNotes = songItem.notes;
            secPerBeat = 60f / songItem.bpm;

            //Starting the audio play back
            dspStartTime = AudioSettings.dspTime;
            audioSource.PlayScheduled(AudioSettings.dspTime + delay);

            TrackManager.INSTANCE.SetupForNewSong();
        }

        public void PauseSong()
        {
            if (songPaused) return;

            songPaused = true;
            audioSource.Pause();

            dspPausedTime = AudioSettings.dspTime;
        }

        public void ResumeSong()
        {
            if (!songPaused) return;

            songPaused = false;
            audioSource.Play();

            accumulatedPauseTime += AudioSettings.dspTime - dspPausedTime;
        }

        public void StopSong(bool dontInvokeEvent = false)
        {
            audioSource.Stop();
            songHasStarted = false;
            songStartEventInvoked = false;

            if (!dontInvokeEvent)
                onSongFinished.Invoke();

            TrackManager.INSTANCE.ClearAllTracks();
        }

        void Update()
        {
            if (!songStartEventInvoked && songHasStarted && songPosition >= 0)
            {
                songStartEventInvoked = true;
                onSongStart.Invoke();
            }

            //Sync the tracks position with the audio
            if (!songPaused && songHasStarted)
            {
                songPosition = (float)(AudioSettings.dspTime - dspStartTime - delay - accumulatedPauseTime);

                TrackManager.INSTANCE.UpdateTrack(songPosition, secPerBeat);

                onSongProgress.Invoke(songPosition);
                if (songPosition >= 0)
                {
                    if (progressAsPercentage)
                        onSongProgressDisplay.Invoke(Math.Truncate(songPosition / audioSource.clip.length * 100) + "%");
                    else
                    {
                        var now = new DateTime((long)songPosition * TimeSpan.TicksPerSecond);
                        onSongProgressDisplay.Invoke(now.ToString("mm:ss"));
                    }
                }
            }

            if (songHasStarted && audioSource.clip && songPosition >= audioSource.clip.length)
            {
                songHasStarted = false;
                songStartEventInvoked = false;
                
                if (playlistLength == songQueue)
                {
                    onSongFinished.Invoke();
                }
                
                else
                {
                    defaultSong = Resources.Load<SongItem>(songList[songQueue]);
                    PlaySong(defaultSong);
                }
            }
        }

        
    }
}