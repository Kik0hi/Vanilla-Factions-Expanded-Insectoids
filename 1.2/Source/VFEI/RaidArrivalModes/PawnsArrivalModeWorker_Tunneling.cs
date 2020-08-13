﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using RimWorld;
using UnityEngine;

namespace VFEI.RaidArrivalModes
{
    class PawnsArrivalModeWorker_Tunneling : PawnsArrivalModeWorker
    {
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			InsectTunnel(parms, pawns);
		}

		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = CellFinderLoose.RandomCellWith((i) => i.Walkable(map) == true, map);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		public static void InsectTunnel(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefsVFEI.VFEI_Mote, parms.spawnCenter, map);			
			foreach (Pawn pawn in pawns)
			{
				GenSpawn.Spawn(pawn, parms.spawnCenter, map);
			}
		}
	}
}