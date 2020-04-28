using UnityEngine;

namespace RhythmGameStarter
{
    public class ComboSystem : MonoBehaviour
    {
        public static ComboSystem INSTANCE;

        private bool isShowing;

        [Header("[Events]")]
        [CollapsedEvent]
        public StringEvent onComboUpdate;
        [CollapsedEvent]
        public BoolEvent onVisibilityChange;

        void Awake()
        {
            INSTANCE = this;
        }

        void Start()
        {
            UpdateComboDisplay();
        }

        public void AddCombo(int addCombo, float deltaDiff, int score)
        {
            StatsSystem.INSTANCE.AddCombo(addCombo, deltaDiff, score);

            if (!isShowing)
            {
                isShowing = true;
                onVisibilityChange.Invoke(isShowing);
            }

            UpdateComboDisplay();
        }

        public void BreakCombo()
        {
            StatsSystem.INSTANCE.AddMissed(1);
            StatsSystem.INSTANCE.combo = 0;

            isShowing = false;
            onVisibilityChange.Invoke(isShowing);
        }

        public void UpdateComboDisplay()
        {
            onComboUpdate.Invoke(StatsSystem.INSTANCE.combo.ToString());
        }
    }
}