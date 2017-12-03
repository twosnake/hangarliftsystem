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
    class Climber : RunningMechanic
    {
        int subfloor = 0;
        int subfloorLimit = 0;

        public Climber(Program myProgram, int subfloorLimit) : base(myProgram)
        {
            this.name = "climber";
            this.subfloorLimit = subfloorLimit-2;
            this.actions.Add(new MoveAction(this.myProgram, "test", new List<IMoveCommand>() {
                new MoveCommand(1,  "RT - Climber - Lift Grab",   "Check", "Attach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "upAirLock", new List<IMoveCommand>() {
                new MoveCommand(2,  "RT - Climber Grab - Up",     "Apply", "Attach"),
                new MoveCommand(2,  "RT - Climber Grab - Up",     "Check", "Attach"),
                new MoveCommand(3,  "RT - Climber Grab - Bottom", "Apply", "Detach"),
                new MoveCommand(3,  "RT - Climber Grab - Bottom", "Check", "Detach"),
                new MoveCommand(4,  "PT - Climber",               "Velocity", "-5"),
                new MoveCommand(4,  "PT - Climber",               "Apply", "Retract"),

                new MoveCommand(7,  "RT - Climber Grab - Bottom", "Apply", "Attach"),
                new MoveCommand(7,  "RT - Climber Grab - Bottom", "Check", "Attach"),

                new MoveCommand(9,  "RT - Climber Grab - Up",     "Apply", "Detach"),
                new MoveCommand(9,  "RT - Climber Grab - Up",     "Check", "Detach"),
                new MoveCommand(10,  "PT - Climber",              "MaxLimit", "2.3"),
                new MoveCommand(10,  "PT - Climber",              "Velocity", "1.5"),
                new MoveCommand(10,  "PT - Climber",              "Apply", "Extend"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "downAirLock", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Climber",              "Velocity", "-2.5"),
                new MoveCommand(0,  "PT - Climber",              "Apply", "Retract"),

                new MoveCommand(4,  "RT - Climber Grab - Up",     "Apply", "Attach"),
                new MoveCommand(4,  "RT - Climber Grab - Up",     "Check", "Attach"),

                new MoveCommand(4,  "PT - Climber",               "MaxLimit", "10"),
                new MoveCommand(4,  "RT - Climber Grab - Bottom", "Apply", "Detach"),
                new MoveCommand(4,  "RT - Climber Grab - Bottom", "Check", "Detach"),
                new MoveCommand(4,  "PT - Climber",               "Velocity", "5"),
                new MoveCommand(4,  "PT - Climber",               "Apply", "Extend"),

                new MoveCommand(10,  "RT - Climber Grab - Bottom", "Apply", "Attach"),
                new MoveCommand(10,  "RT - Climber Grab - Bottom", "Check", "Attach"),
                new MoveCommand(10,  "PT - Climber",               "Velocity", "-2.5"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "up", new List<IMoveCommand>() {
                new MoveCommand(0,  "RT - Climber Grab - Up",     "Apply", "Attach"),
                new MoveCommand(0,  "RT - Climber Grab - Up",     "Check", "Attach"),
                new MoveCommand(0,  "RT - Climber Grab - Bottom", "Apply", "Detach"),
                new MoveCommand(0,  "RT - Climber Grab - Bottom", "Check", "Detach"),
                new MoveCommand(0,  "PT - Climber",               "Velocity", "5"),
                new MoveCommand(0,  "PT - Climber",               "Apply", "Retract"),

                new MoveCommand(3,  "RT - Climber Grab - Bottom", "Apply", "Attach"),
                new MoveCommand(3,  "RT - Climber Grab - Bottom", "Check", "Attach"),
                new MoveCommand(3,  "PT - Climber",               "Velocity", "2.5"),

                new MoveCommand(4,  "RT - Climber Grab - Up",     "Apply", "Detach"),
                new MoveCommand(4,  "RT - Climber Grab - Up",     "Check", "Detach"),
                new MoveCommand(4,  "PT - Climber",               "Apply", "Extend"),

                new MoveCommand(8, "RT - Climber Grab - Up",     "Apply", "Attach"),
                new MoveCommand(8, "RT - Climber Grab - Up",     "Check", "Attach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "down", new List<IMoveCommand>() {
                new MoveCommand(0,  "RT - Climber Grab - Bottom", "Apply","Attach"),
                new MoveCommand(0,  "RT - Climber Grab - Bottom", "Check", "Attach"),
                new MoveCommand(0,  "RT - Climber Grab - Up",     "Apply", "Detach"),
                new MoveCommand(0,  "RT - Climber Grab - Up",     "Check", "Detach"),
                new MoveCommand(0,  "PT - Climber",               "Velocity", "2.5"),
                new MoveCommand(0,  "PT - Climber",               "Apply", "Retract"),

                new MoveCommand(5,  "RT - Climber Grab - Up",     "Apply", "Attach"),
                new MoveCommand(5,  "RT - Climber Grab - Up",     "Check", "Attach"),

                new MoveCommand(5,  "RT - Climber Grab - Bottom", "Apply", "Detach"),
                new MoveCommand(5,  "RT - Climber Grab - Bottom", "Check", "Detach"),
                new MoveCommand(5,  "PT - Climber",               "Velocity", "-5"),
                new MoveCommand(5,  "PT - Climber",               "Apply", "Extend"),

                new MoveCommand(8, "RT - Climber Grab - Bottom", "Apply", "Attach"),
                new MoveCommand(8, "RT - Climber Grab - Bottom", "Check", "Attach"),
                new MoveCommand(8, "PT - Climber",               "Velocity", "-2.5"),
            }));
        }

        public void setSubFloor(int subfloor)
        {
            this.subfloor = subfloor;
        }

        public int getSubFloor()
        {
            return this.subfloor;
        }

        public int getSubFloorLimit()
        {
            return this.subfloorLimit;
        }

        override public bool execAction(string action)
        {
            if (action == "up")
            {
                this.subfloor++;
            }
            if (action == "down")
            {
                this.subfloor--;
            }
            return base.execAction(action);
        }
    }
}
