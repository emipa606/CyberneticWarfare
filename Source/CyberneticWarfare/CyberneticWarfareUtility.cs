using System;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x02000005 RID: 5
	public static class CyberneticWarfareUtility
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002264 File Offset: 0x00000464
		public static Direction8Way Direction8WayFromAngle(float angle)
		{
			bool flag = angle >= 337.5f || angle < 22.5f;
			Direction8Way result;
			if (flag)
			{
				result = Direction8Way.North;
			}
			else
			{
				bool flag2 = angle < 67.5f;
				if (flag2)
				{
					result = Direction8Way.NorthEast;
				}
				else
				{
					bool flag3 = angle < 112.5f;
					if (flag3)
					{
						result = Direction8Way.East;
					}
					else
					{
						bool flag4 = angle < 157.5f;
						if (flag4)
						{
							result = Direction8Way.SouthEast;
						}
						else
						{
							bool flag5 = angle < 202.5f;
							if (flag5)
							{
								result = Direction8Way.South;
							}
							else
							{
								bool flag6 = angle < 247.5f;
								if (flag6)
								{
									result = Direction8Way.SouthWest;
								}
								else
								{
									bool flag7 = angle < 292.5f;
									if (flag7)
									{
										result = Direction8Way.West;
									}
									else
									{
										result = Direction8Way.NorthWest;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002304 File Offset: 0x00000504
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
}
