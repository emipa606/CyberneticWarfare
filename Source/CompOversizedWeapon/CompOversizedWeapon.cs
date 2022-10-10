using Verse;

namespace CompOversizedWeapon;

public class CompOversizedWeapon : ThingComp
{
    private bool isEquipped;

    public CompOversizedWeapon()
    {
        if (props is not CompProperties_OversizedWeapon)
        {
            props = new CompProperties_OversizedWeapon();
        }
    }

    public CompProperties_OversizedWeapon Props => props as CompProperties_OversizedWeapon;

    public CompEquippable GetEquippable
    {
        get
        {
            var thingWithComps = parent;
            return thingWithComps?.GetComp<CompEquippable>();
        }
    }

    public Pawn GetPawn
    {
        get
        {
            var getEquippable = GetEquippable;
            Pawn result;
            if (getEquippable == null)
            {
                result = null;
            }
            else
            {
                var verbTracker = getEquippable.verbTracker;
                if (verbTracker == null)
                {
                    result = null;
                }
                else
                {
                    var primaryVerb = verbTracker.PrimaryVerb;
                    result = primaryVerb?.CasterPawn;
                }
            }

            return result;
        }
    }

    public bool IsEquipped
    {
        get
        {
            bool result;
            if (Find.TickManager.TicksGame % 60 != 0)
            {
                result = isEquipped;
            }
            else
            {
                isEquipped = GetPawn != null;
                result = isEquipped;
            }

            return result;
        }
    }

    public bool FirstAttack { get; set; } = false;
}