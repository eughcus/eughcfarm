using Eughc.DayNight;

namespace Eughc.Farm {
    public interface IDayPhaseSensitive {
        StatModifierBundle? GetDayPhaseModifier(DayPhaseSO phase);
    }
}