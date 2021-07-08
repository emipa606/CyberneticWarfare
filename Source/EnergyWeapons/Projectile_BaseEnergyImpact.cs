using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EQEnergyWeapons
{
    // Token: 0x02000004 RID: 4
    public class Projectile_BaseEnergyImpact : Projectile
    {
        // Token: 0x0400000B RID: 11
        public CompInitializeDamage compED;

        // Token: 0x04000011 RID: 17
        public float drawingIntensity;

        // Token: 0x0400000E RID: 14
        public Matrix4x4 drawingMatrix;

        // Token: 0x04000010 RID: 16
        public Vector3 drawingPosition;

        // Token: 0x0400000F RID: 15
        public Vector3 drawingScale;

        // Token: 0x04000012 RID: 18
        public Material drawingTexture;

        // Token: 0x0400000A RID: 10
        public Thing hitThing;

        // Token: 0x0400000D RID: 13
        public Material postFiringTexture;

        // Token: 0x0400000C RID: 12
        public Material preFiringTexture;

        // Token: 0x04000009 RID: 9
        public int tickCounter;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000006 RID: 6 RVA: 0x0000213A File Offset: 0x0000033A
        public ThingDef_EnergyWeaponsBase Props => def as ThingDef_EnergyWeaponsBase;

        // Token: 0x06000005 RID: 5 RVA: 0x00002111 File Offset: 0x00000311
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            drawingTexture = def.DrawMatSingle;
            compED = GetComp<CompInitializeDamage>();
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002147 File Offset: 0x00000347
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref tickCounter, "tickCounter");
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002164 File Offset: 0x00000364
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
                if (!(curStance is Stance_Busy) || pawn.Dead)
                {
                    Destroy();
                }
            }

            tickCounter++;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x0000226C File Offset: 0x0000046C
        public virtual void PerformPreFiringTreatment()
        {
            DetermineImpactExactPosition();
            var vector = (destination - origin).normalized * 0.9f;
            drawingScale = new Vector3(1f, 1f, (destination - origin).magnitude - vector.magnitude);
            drawingPosition = origin + (vector / 2f) + ((destination - origin) / 2f) + (Vector3.up * def.Altitude);
            drawingMatrix.SetTRS(drawingPosition, ExactRotation, drawingScale);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002350 File Offset: 0x00000550
        public virtual void GetPreFiringDrawingParameters()
        {
            if (Props.TickOffset != 0)
            {
                drawingIntensity = Props.DrawingOffset +
                                   ((Props.DrawingOffsetSecond - Props.DrawingOffset) * tickCounter / Props.TickOffset);
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000023B0 File Offset: 0x000005B0
        public virtual void GetPostFiringDrawingParameters()
        {
            if (Props.TickOffsetSecond != 0)
            {
                drawingIntensity = Props.DrawingOffsetThird + ((Props.DrawingOffsetFourth - Props.DrawingOffsetThird) *
                                                               ((tickCounter - (float) Props.TickOffset) /
                                                                Props.TickOffsetSecond));
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002420 File Offset: 0x00000620
        protected void DetermineImpactExactPosition()
        {
            var vector = destination - origin;
            var num = (int) vector.magnitude;
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
                        var downed = pawn != null && pawn.Downed;
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

        // Token: 0x0600000D RID: 13 RVA: 0x000026C0 File Offset: 0x000008C0
        public virtual void Fire()
        {
            ApplyDamage(hitThing);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000026D0 File Offset: 0x000008D0
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

        // Token: 0x0600000F RID: 15 RVA: 0x000026FC File Offset: 0x000008FC
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

        // Token: 0x06000010 RID: 16 RVA: 0x00002A68 File Offset: 0x00000C68
        protected override void Impact(Thing thing)
        {
            var map = Map;
            base.Impact(thing);
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

        // Token: 0x06000011 RID: 17 RVA: 0x00002BE4 File Offset: 0x00000DE4
        public override void Draw()
        {
            Comps_PostDraw();
            Graphics.DrawMesh(MeshPool.plane10, drawingMatrix,
                FadedMaterialPool.FadedVersionOf(drawingTexture, drawingIntensity), 0);
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002C14 File Offset: 0x00000E14
        public static void ThrowMicroSparksRed(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            Rand.PushState();
            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDef.Named("LaserImpactThing"));
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
}