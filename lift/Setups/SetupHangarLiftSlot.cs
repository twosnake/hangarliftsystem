using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;


namespace IngameScript
{
    class SetupHangarLiftSlot : ISetupMechanic
    {
        protected bool retractingPistons = false;
        protected bool checkAttached = false;
        protected bool inReadyState = false;
        protected Program myProgram;
        protected HangarManager hangarMgr;

        public SetupHangarLiftSlot(Program myProgram, MechanicManager mechMgr, HangarManager hangarMgr)
        {
            this.myProgram = myProgram;
            this.hangarMgr = hangarMgr;

            List<IHangarMechanic> hangars = this.hangarMgr.getAllHangar();
            foreach(var hangar in hangars)
            {
                HangarLiftSlot liftSlotMech = hangar.getLiftSlotMech();
                liftSlotMech.execAction("checkAttached");
            }
        }

        public bool isReady()
        {
            return this.inReadyState;
        }

        public void run()
        {
            if (!checkAttached)
            {
                this.checkAttachedSlots();
                return;
            }

            List<IHangarMechanic> hangars = this.hangarMgr.getAllHangar();
            if (!retractingPistons)
            {
                foreach (var hangar in hangars)
                {
                    HangarLiftSlot liftSlotMech = hangar.getLiftSlotMech();
                    liftSlotMech.reset();
                    liftSlotMech.queueAction("setupRetract");
                 //   liftSlotMech.queueAction("mergeOn");
                }
                this.retractingPistons = true;
                return;
            }

            foreach (var hangar in hangars)
            {
                HangarLiftSlot liftSlotMech = hangar.getLiftSlotMech();
                liftSlotMech.runActionQueue();

                if (liftSlotMech.hasStopped())
                {
                    throw new Exception("Hangar lift slot in error state.");
                }

                if (liftSlotMech.hasActions())
                {
                    return;
                }
            }
            this.inReadyState = true;
        }

        protected void checkAttachedSlots() { 
            List<IHangarMechanic> hangars = this.hangarMgr.getAllHangar();
            foreach (var hangar in hangars)
            {
                HangarLiftSlot liftSlotMech = hangar.getLiftSlotMech();
                liftSlotMech.runActionQueue();

                if (liftSlotMech.hasStopped())
                {
                    // still attached to lift, set command to return back
                    hangar.setReturn();
                    liftSlotMech.reset();
                    return;
                }

                if (liftSlotMech.hasActions())
                {
                    return;
                }
            }

            this.checkAttached = true;
        }
    }
}
