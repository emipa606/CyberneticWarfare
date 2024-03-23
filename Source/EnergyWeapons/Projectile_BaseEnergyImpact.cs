using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EQEnergyWeapons;

public class Projectile_BaseEnergyImpact : Projectile
{
    public CompInitializeDamage compED;

    public float drawingIntensity;

    public Matrix4x4 drawingMatrix;

    public Vector3 drawingPosition;

    public Vector3 drawingScale;

    public Material drawingTexture;

    public Thing hitThing;

    public Material postFiringTexture;

    public Material preFiringTexture;

    public int tickCounter;

    public ThingDef_EnergyWeaponsBase Props => def as ThingDef_EnergyWeaponsBase;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        drawingTexture = def.DrawMatSingle;
        compED = GetComp<CompInitializeDamage>();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref tickCounter, "tickCounter");
    }

    public override void Tick()
    {
        if (tickCounter == 0)
        {
            PerformPreFiringTreatment();
        }

        if (tickCounter < Props.TickOffset)
        {
            GetPreFiringDrawingParameters();
        }
        else
        {
            if (tickCounter == Props.TickOffset)
            {
                Fire();
            }

            GetPostFiringDrawingParameters();
        }

        if (tickCounter == Props.TickOffset + Props.TickOffsetSecond)
        {
            base.Tick();
            Destroy();
        }

        var thing = launcher;
        var pawn = thing as Pawn;
        var stances = new Pawn_StanceTracker(pawn);
        bool b;
        if (pawn != null)
        {
            stances = pawn.stances;
            b = stances != null;
        }
        else
        {
            b = false;
        }

        if (b)
        {
            var curStance = stances.curStance;
            if (curStance is not Stance_Busy || pawn.Dead)
            {
                Destroy();
            }
        }

        tickCounter++;
    }

    public virtual void PerformPreFiringTreatment()
    {
        DetermineImpactExactPosition();
        var vector = (destination - origin).normalized * 0.9f;
        drawingScale = new Vector3(1f, 1f, (destination - origin).magnitude - vector.magnitude);
        drawingPosition = origin + (vector / 2f) + ((destination - origin) / 2f) + (Vector3.up * def.Altitude);
        drawingMatrix.SetTRS(drawingPosition, ExactRotation, drawingScale);
    }

    public virtual void GetPreFiringDrawingParameters()
    {
        if (Props.TickOffset != 0)
        {
            drawingIntensity = Props.DrawingOffset +
                               ((Props.DrawingOffsetSecond - Props.DrawingOffset) * tickCounter / Props.TickOffset);
        }
    }

    public virtual void GetPostFiringDrawingParameters()
    {
        if (Props.TickOffsetSecond != 0)
        {
            drawingIntensity = Props.DrawingOffsetThird + ((Props.DrawingOffsetFourth - Props.DrawingOffsetThird) *
                                                           ((tickCounter - (float)Props.TickOffset) /
                                                            Props.TickOffsetSecond));
        }
    }

    protected void DetermineImpactExactPosition()
    {
        var vector = destination - origin;
        var num = (int)vector.magnitude;
        var vector2 = vector / vector.magnitude;
        var destination1 = origin;
        var vector3 = origin;
        for (var i = 1; i <= num; i++)
        {
            vector3 += vector2;
            var intVec = vector3.ToIntVec3();
            if (!vector3.InBounds(Map))
            {
                destination = destination1;
                break;
            }

            if (!def.projectile.flyOverhead && def.projectile.alwaysFreeIntercept && i >= 5)
            {
                var list = Map.thingGrid.ThingsListAt(Position);
                foreach (var thing in list)
                {
                    if (thing.def.Fillage == FillCategory.Full)
                    {
                        destination = intVec.ToVector3Shifted() +
                                      new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
                        hitThing = thing;
                        break;
                    }

                    if (thing.def.category != ThingCategory.Pawn)
                    {
                        continue;
                    }

                    var pawn = thing as Pawn;
                    var num2 = 0.45f;
                    var downed = pawn is { Downed: true };
                    if (downed)
                    {
                        num2 *= 0.1f;
                    }

                    var num3 = (ExactPosition - origin).MagnitudeHorizontal();
                    if (num3 < 4f)
                    {
                        num2 *= 0f;
                    }
                    else
                    {
                        if (num3 < 7f)
                        {
                            num2 *= 0.5f;
                        }
                        else
                        {
                            if (num3 < 10f)
                            {
                                num2 *= 0.75f;
                            }
                        }
                    }

                    if (pawn == null)
                    {
                        continue;
                    }

                    num2 *= pawn.RaceProps.baseBodySize;
                    if (!(Rand.Value < num2))
                    {
                        continue;
                    }

                    destination = intVec.ToVector3Shifted() +
                                  new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
                    hitThing = pawn;
                    break;
                }
            }

            destination1 = vector3;
        }
    }

    public virtual void Fire()
    {
        ApplyDamage(hitThing);
    }

    protected void ApplyDamage(Thing thing)
    {
        if (thing != null)
        {
            Impact(thing);
        }
        else
        {
            ImpactSomething();
        }
    }

    private void ImpactSomething()
    {
        var flyOverhead = def.projectile.flyOverhead;
        if (flyOverhead)
        {
            var roofDef = Map.roofGrid.RoofAt(Position);
            if (roofDef != null)
            {
                var isThickRoof = roofDef.isThickRoof;
                if (isThickRoof)
                {
                    def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(Position, Map));
                    Destroy();
                    return;
                }

                if (Position.GetEdifice(Map) == null ||
                    Position.GetEdifice(Map).def.Fillage != FillCategory.Full)
                {
                    RoofCollapserImmediate.DropRoofInCells(Position, Map);
                }
            }
        }

        if (!usedTarget.HasThing || !CanHit(usedTarget.Thing))
        {
            var list = new List<Thing>();
            list.Clear();
            var thingList = Position.GetThingList(Map);
            foreach (var thing in thingList)
            {
                if ((thing.def.category == ThingCategory.Building ||
                     thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item ||
                     thing.def.category == ThingCategory.Plant) && CanHit(thing))
                {
                    list.Add(thing);
                }
            }

            list.Shuffle();
            for (var j = 0; j < list.Count; j++)
            {
                var thing2 = list[j];
                float num;
                if (thing2 is Pawn pawn)
                {
                    num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
                    if (pawn.GetPosture() != PawnPosture.Standing &&
                        (origin - destination).MagnitudeHorizontalSquared() >= 20.25f)
                    {
                        num *= 0.2f;
                    }

                    if (launcher != null && pawn.Faction != null && launcher.Faction != null &&
                        !pawn.Faction.HostileTo(launcher.Faction))
                    {
                        num *= VerbUtility.InterceptChanceFactorFromDistance(origin, Position);
                    }
                }
                else
                {
                    num = 1.5f * thing2.def.fillPercent;
                }

                if (!Rand.Chance(num))
                {
                    continue;
                }

                Impact(list.RandomElement());
                return;
            }

            Impact(null);
        }
        else
        {
            if (usedTarget.Thing is Pawn pawn2 && pawn2.GetPosture() != PawnPosture.Standing &&
                (origin - destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.2f))
            {
                Impact(null);
            }
            else
            {
                Impact(usedTarget.Thing);
            }
        }
    }

    protected override void Impact(Thing thing, bool blockedByShield = false)
    {
        var map = Map;
        base.Impact(thing, blockedByShield);
        if (thing != null)
        {
            var damageAmount = def.projectile.GetDamageAmount(DamageAmount);
            var thingDef = equipmentDef;
            var armorPenetration = def.projectile.GetArmorPenetration(ArmorPenetration);
            var dinfo = new DamageInfo(def.projectile.damageDef, damageAmount, armorPenetration,
                ExactRotation.eulerAngles.y, launcher, null, thingDef);
            thing.TakeDamage(dinfo);
            if (thing is not Pawn pawn || pawn.Downed || !(Rand.Value < compED.chanceToProc))
            {
                return;
            }

            FleckMaker.ThrowMicroSparks(destination, Map);
            thing.TakeDamage(new DamageInfo(DefDatabase<DamageDef>.GetNamed(compED.damageDef),
                compED.damageAmount, armorPenetration, ExactRotation.eulerAngles.y, launcher));
        }
        else
        {
            SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(Position, map));
            MoteMaker.MakeStaticMote(ExactPosition, map, ThingDefOf.LaserMoteWorker, 0.5f);
            ThrowMicroSparksRed(ExactPosition, Map);
        }
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        Comps_PostDraw();
        Graphics.DrawMesh(MeshPool.plane10, drawingMatrix,
            FadedMaterialPool.FadedVersionOf(drawingTexture, drawingIntensity), 0);
    }

    public static void ThrowMicroSparksRed(Vector3 loc, Map map)
    {
        if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
        {
            return;
        }

        Rand.PushState();
        var moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("LaserImpactThing"));
        moteThrown.Scale = Rand.Range(1f, 1.2f);
        moteThrown.rotationRate = Rand.Range(-12f, 12f);
        moteThrown.exactPosition = loc;
        moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
        moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
        moteThrown.SetVelocity(Rand.Range(35, 45), 1.2f);
        GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        Rand.PopState();
    }
}