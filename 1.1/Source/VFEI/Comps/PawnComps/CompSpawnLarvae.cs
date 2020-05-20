﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace VFEI.PawnComps
{
    class CompSpawnLarvae : ThingComp
    {
		int nextSpawn;
		int tickToSpawn;

		private CompProperties_SpawnLarvae Props
		{
			get
			{
				return (CompProperties_SpawnLarvae)this.props;
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.nextSpawn = Find.TickManager.TicksGame + Props.ticksBetweenSpawn;
			this.tickToSpawn = Props.ticksBetweenSpawn;
		}

		public override string CompInspectStringExtra()
		{
			return "LarvaeTimeSpawn".Translate(tickToSpawn.ToStringTicksToPeriod());
		}

		public override void CompTick()
		{
			base.CompTick();
			if(this.parent is Pawn pa && (pa.GetPosture() != PawnPosture.LayingOnGroundNormal || pa.GetPosture() != PawnPosture.LayingOnGroundFaceUp) && !pa.health.Downed)
			{
				if (Find.TickManager.TicksGame == this.nextSpawn)
				{
					for (int i = 0; i < Props.numberToSpawn; i++)
					{
						Pawn p = PawnGenerator.GeneratePawn(ThingDefsVFEI.VFEI_Insectoid_Larvae, this.parent.Faction);
						GenSpawn.Spawn(p, this.parent.Position.RandomAdjacentCellCardinal(), this.parent.Map);

					}
					FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_Slime, 4);
					SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.parent));
					this.nextSpawn = Find.TickManager.TicksGame + this.Props.ticksBetweenSpawn;
					this.tickToSpawn = Props.ticksBetweenSpawn;
				}
				this.tickToSpawn--;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawn, "QueenNextSpawn");
			Scribe_Values.Look<int>(ref this.tickToSpawn, "QueenNextSpawnUtilsString");
		}
	}
}