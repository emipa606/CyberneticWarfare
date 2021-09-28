using System.Collections.Generic;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x02000003 RID: 3
    public class CompWargearWeapon : ThingComp
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x0000206A File Offset: 0x0000026A
        public CompProperties_WargearWeapon Props => (CompProperties_WargearWeapon)props;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
        protected virtual Pawn EquippingPawn
        {
            get
            {
                var parentHolder = ParentHolder;
                Pawn result;
                if (parentHolder is Pawn_EquipmentTracker pawn_EquipmentTracker)
                {
                    result = pawn_EquipmentTracker.pawn;
                }
                else
                {
                    result = null;
                }

                return result;
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020AA File Offset: 0x000002AA
        public virtual IEnumerable<Gizmo> EquippedGizmos()
        {
            yield break;
        }
    }
}