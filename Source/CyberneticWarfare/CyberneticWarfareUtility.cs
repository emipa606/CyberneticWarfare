using System;
using RimWorld;
using Verse;

namespace CyberneticWarfare;

public static class CyberneticWarfareUtility
{
    public static Direction8Way Direction8WayFromAngle(float angle)
    {
        Direction8Way result;
        switch (angle)
        {
            case >= 337.5f:
            case < 22.5f:
                result = Direction8Way.North;
                break;
            case < 67.5f:
                result = Direction8Way.NorthEast;
                break;
            case < 112.5f:
                result = Direction8Way.East;
                break;
            case < 157.5f:
                result = Direction8Way.SouthEast;
                break;
            case < 202.5f:
                result = Direction8Way.South;
                break;
            case < 247.5f:
                result = Direction8Way.SouthWest;
                break;
            default:
                result = angle < 292.5f ? Direction8Way.West : Direction8Way.NorthWest;
                break;
        }

        return result;
    }

    public static IntVec3 IntVec3FromDirection8Way(Direction8Way source)
    {
        IntVec3 result;
        switch (source)
        {
            case Direction8Way.North:
                result = IntVec3.North;
                break;
            case Direction8Way.NorthEast:
                result = IntVec3.NorthEast;
                break;
            case Direction8Way.East:
                result = IntVec3.East;
                break;
            case Direction8Way.SouthEast:
                result = IntVec3.SouthEast;
                break;
            case Direction8Way.South:
                result = IntVec3.South;
                break;
            case Direction8Way.SouthWest:
                result = IntVec3.SouthWest;
                break;
            case Direction8Way.West:
                result = IntVec3.West;
                break;
            case Direction8Way.NorthWest:
                result = IntVec3.NorthWest;
                break;
            default:
                throw new NotImplementedException();
        }

        return result;
    }
}