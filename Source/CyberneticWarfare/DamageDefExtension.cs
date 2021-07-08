using Verse;

namespace CyberneticWarfare
{
    // Token: 0x02000006 RID: 6
    public class DamageDefExtension : DefModExtension
    {
        // Token: 0x04000001 RID: 1
        public static readonly DamageDefExtension defaultValues = new DamageDefExtension();

        // Token: 0x04000004 RID: 4
        public float hitBuildingStunDurationFactor;

        // Token: 0x04000002 RID: 2
        public float knockbackDistancePerDamageDealt;

        // Token: 0x04000003 RID: 3
        public bool scaleKnockbackWithBodySize;

        // Token: 0x04000005 RID: 5
        public int stunDuration;
    }
}