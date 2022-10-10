using Verse;

namespace CyberneticWarfare;

public class DamageDefExtension : DefModExtension
{
    public static readonly DamageDefExtension defaultValues = new DamageDefExtension();

    public float hitBuildingStunDurationFactor;

    public float knockbackDistancePerDamageDealt;

    public bool scaleKnockbackWithBodySize;

    public int stunDuration;
}