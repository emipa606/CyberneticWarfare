using System.Collections.Generic;
using Verse;

namespace CyberneticWarfare;

public class CompWargearWeapon : ThingComp
{
    public CompProperties_WargearWeapon Props => (CompProperties_WargearWeapon)props;

    protected virtual Pawn EquippingPawn
    {
        get
        {
            var parentHolder = ParentHolder;
            Pawn result;
            if (parentHolder is Pawn_EquipmentTracker pawnEquipmentTracker)
            {
                result = pawnEquipmentTracker.pawn;
            }
            else
            {
                result = null;
            }

            return result;
        }
    }

    public virtual IEnumerable<Gizmo> EquippedGizmos()
    {
        yield break;
    }
}