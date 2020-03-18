using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class StatsSystem : MonoBehaviour
    {
        public static StatsSystem INSTANCE;

        void Awake()
        {
            INSTANCE = this;
        }


        [HideInInspector]
        public int combo;

        [HideInInspector]
        public int maxCombo;

        [HideInInspector]
        public int missed;

        [HideInInspector]
        public int score;

        public List<HitLevel> levels;

        [Header("[Events]")]
        [CollapsedEvent]
        public StringEvent onComboStatusUpdate;
        [CollapsedEvent]
        public StringEvent onScoreUpdate;
        [CollapsedEvent]
        public StringEvent onMaxComboUpdate;
        [CollapsedEvent]
        public StringEvent onMissedUpdate;


        [Serializable]
        public class HitLevel
        {
            public string name;
            public float threshold;
            [HideInInspector]
            public int count;
            public float scorePrecentage = 1;
            public StringEvent onCountUpdate;
        }

        public void AddMissed(int addMissed)
        {
            missed += addMissed;
            onMissedUpdate.Invoke(missed.ToString());
        }

        void Start()
        {
            UpdateScoreDisplay();
        }

        public void AddCombo(int addCombo, float deltaDiff, int addScore)
        {
            // print(deltaDiff);
            combo += addCombo;
            if (combo > maxCombo)
            {
                maxCombo = combo;
                onMaxComboUpdate.Invoke(maxCombo.ToString());
            }

            for (int i = 0; i < levels.Count; i++)
            {
                var x = levels[i];
                if (deltaDiff <= x.threshold)
                {
                    x.count++;
                    score += (int)(addScore * x.scorePrecentage);
                    x.onCountUpdate.Invoke(x.count.ToString());
                    UpdateScoreDisplay();
                    onComboStatusUpdate.Invoke(x.name);
                    // print(x.name);
                    return;
                }
            }

            //When no level matched
            onComboStatusUpdate.Invoke("");

        }

        public void UpdateScoreDisplay()
        {
            onScoreUpdate.Invoke(score.ToString());
        }
    }
}