using System;
using Eughc.DayNight;

namespace Eughc.Farm {
    [Serializable]
    public struct DayPhaseModEntry {
        public DayPhaseSO Phase;
        public StatModifierBundle Bundle;
    }
}