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
    class SetupClimber : ISetupMechanic
    {
        bool hasHitBottom = false;
        bool inReadyState = false;
        Climber climber;
        Program myProgram;

        public SetupClimber(Program myProgram, MechanicManager mechMgr)
        {
            this.myProgram = myProgram;
            this.climber = (Climber) mechMgr.getMechanic("climber");
            this.climber.queueAction("down");
        }

        public bool isReady()
        {
            return this.hasHitBottom && this.inReadyState;
        }

        public void run()
        {
            this.climber.runActionQueue();

            if (this.hasHitBottom)
            {
                if (!this.climber.hasActions())
                {
                    this.inReadyState = true;
                    this.climber.setSubFloor(this.climber.getSubFloorLimit());
                    this.myProgram.Echo("Going to ready state on floor: " + this.climber.getSubFloor().ToString());
                }
                return;
            }

            if (!this.climber.hasActions())
            {
                this.climber.queueAction("down");
            }

            if (this.climber.hasStopped())
            {
                // Climber stopped on the end of the rail
                this.climber.clearActionQueue();
                this.climber.reset();
                this.climber.queueAction("up");
                this.climber.runActionQueue();
                this.hasHitBottom = true;
            }
        }
    }
}
