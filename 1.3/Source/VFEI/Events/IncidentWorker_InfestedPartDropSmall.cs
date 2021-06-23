﻿using RimWorld;
using RimWorld.Planet;
using Verse;

namespace VFEI
{
    internal class IncidentWorker_InfestedPartDropSmall : IncidentWorker
    {
        protected const float HivePoints = 300f;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 intVec;
            return base.CanFireNowSub(parms) && this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec);
        }

        protected int RandomCountToDrop(IncidentParms parms)
        {
            int count = Rand.RangeInclusive(2, 8);
            return count;
        }

        protected void SpawnChunk(IntVec3 pos, Map map)
        {
            SkyfallerMaker.SpawnSkyfaller(VFEI_DefsOf.VFEI_InfestedShipChunkIncoming, VFEI_DefsOf.VFEI_InfestedShipChunk, pos, map);
        }

        protected void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
        {
            this.SpawnChunk(firstChunkPos, map);
            for (int i = 0; i < count - 1; i++)
            {
                IntVec3 pos;
                bool flag = this.TryFindShipChunkDropCell(firstChunkPos, map, 20, out pos);
                if (flag)
                {
                    this.SpawnChunk(pos, map);
                }
            }
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 intVec;
            bool flag = !this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                this.SpawnShipChunks(intVec, map, this.RandomCountToDrop(parms));
                Find.LetterStack.ReceiveLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, new GlobalTargetInfo(intVec, map, false), null, null);
                result = true;
            }
            return result;
        }

        protected bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
        {
            ThingDef infestedShipChunkIncoming = VFEI_DefsOf.VFEI_InfestedShipChunkIncoming;
            return CellFinderLoose.TryFindSkyfallerCell(infestedShipChunkIncoming, map, out pos, 40, nearLoc, maxDist, true, false, false, false, true, false, null);
        }
    }
}