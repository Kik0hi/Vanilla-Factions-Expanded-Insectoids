﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Verse.Sound;

namespace VFEI
{
    class Hediff_JellyWithdraw : Hediff_Addiction
    {

        bool firstTime = true;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<bool>(ref this.firstTime, "firstTime");
        }

        public override void Tick()
        {
            base.Tick();
            Need_Chemical need = this.Need;
            if (need != null && need.CurCategory == DrugDesireCategory.Withdrawal)
            {
                Faction colonistFac = this.pawn.Faction;
                IntVec3 colonistLoc = this.pawn.Position;
                Name colonistName = this.pawn.Name;
                Map map = this.pawn.Map;
                this.pawn.Destroy(DestroyMode.Vanish);
                PawnKindDef pawnKindDef = PawnKindDefOf.Megaspider;
                Pawn megaspider = PawnGenerator.GeneratePawn(pawnKindDef, colonistFac);
                megaspider.Name = colonistName;
                GenSpawn.Spawn(megaspider, colonistLoc, map, WipeMode.Vanish);
                SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.pawn));
                for (int i = 0; i < 20; i++)
                {
                    IntVec3 c;
                    CellFinder.TryFindRandomReachableCellNear(colonistLoc, map, 4, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                    int flag2 = Rand.RangeInclusive(0, 2);
                    if (flag2 == 1)
                    {
                        FilthMaker.TryMakeFilth(c, map, ThingDefsVFEI.Filth_BloodInsect);
                    }
                    else
                    {
                        FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Blood);
                    }
                }
                #pragma warning disable 0618
                string text = "TransformationLetter".Translate(colonistName).CapitalizeFirst();
                Find.LetterStack.ReceiveLetter("TransformationLLabel".Translate(colonistName).CapitalizeFirst(), text, LetterDefOf.NegativeEvent, megaspider, null, null);
            }
            this.firstTime = false;
        }
    }
}
