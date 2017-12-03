using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    class HangarLiftSlot : RunningMechanic
    {
        string hangarName;

        public HangarLiftSlot(Program myProgram, string hangarName) : base(myProgram)
        {
            this.name = "hangerlift"+hangarName;
            this.hangarName = hangarName;

            this.actions.Add(new MoveAction(this.myProgram, "mergeOn", new List<IMoveCommand>() {
                new MoveCommand(1,  "MB - Hangar "+hangarName+" - Wall Lock",   "Apply",    "Off"),
                new MoveCommand(1,  "MB - Hangar "+hangarName+" - Lift Lock",   "Apply",    "Off"),

                new MoveCommand(1,  "MB - Hangar "+hangarName+" - Wall Lock",   "Check",    "Off"),
                new MoveCommand(1,  "MB - Hangar "+hangarName+" - Lift Lock",   "Check",    "Off"),
                new MoveCommand(1,  "MB - Hangar "+hangarName+" - Outer Lock",  "Apply",    "On"),
                new MoveCommand(1,  "MB - Hangar "+hangarName+" - Outer Lock",  "Check",    "On"),

                new MoveCommand(2,  "PT - Hangar "+hangarName+" - Lock",         "Apply",    "Extend"),
                new MoveCommand(2,  "RT - Hangar "+hangarName+" - Lock",         "Velocity", "3"),
                new MoveCommand(6,  "PT - Hangar "+hangarName+" - Lock",         "Check",    "Extend"),
                new MoveCommand(7,  "MB - Hangar "+hangarName+" - Wall Lock",    "Apply",    "On"),
                new MoveCommand(8,  "MB - Hangar "+hangarName+" - Lift Lock",    "Apply",    "On"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "moveOut", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "-5"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "5"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Check", "Retract"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Apply", "Extend"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Apply", "Retract"),
                new MoveCommand(2,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "2"),
                new MoveCommand(2,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "-2"),
                new MoveCommand(3,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "0.8"),
                new MoveCommand(3,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "-1.3"),
                new MoveCommand(7, "RT - Hangar "+hangarName+" - Lift",         "Check", "Extend"),
                new MoveCommand(7,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "0.5"),
                new MoveCommand(7,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "-0.5"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "moveIn", new List<IMoveCommand>() {
                new MoveCommand(1,  "PT - Hangar "+hangarName+" - Lift",         "Check", "Extend"),
                new MoveCommand(1,  "PT - Hangar "+hangarName+" - Lift",         "Apply", "Retract"),
                new MoveCommand(1,  "PT - Hangar "+hangarName+" - Lift Reverse", "Apply", "Extend"),
                new MoveCommand(14, "RT - Hangar "+hangarName+" - Lift",         "Check", "Retract"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "moveInNoLift", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Check", "Extend"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Apply", "Retract"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Apply", "Extend"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "-5"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "5"),
                new MoveCommand(5, "RT - Hangar "+hangarName+" - Lift",         "Check", "Retract"),
                new MoveCommand(5,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "-0.5"),
                new MoveCommand(5,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "0.5"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "moveOutNoLift", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Check", "Retract"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Apply", "Extend"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Apply", "Retract"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "5"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "-5"),
                new MoveCommand(4,  "PT - Hangar "+hangarName+" - Lift",         "Velocity", "0.5"),
                new MoveCommand(4,  "PT - Hangar "+hangarName+" - Lift Reverse", "Velocity", "-0.5"),
                new MoveCommand(5,  "RT - Hangar "+hangarName+" - Lift",         "Check", "Extend"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "lockClimber", new List<IMoveCommand>() {
                new MoveCommand(0,  "RT - Climber - Lift Grab", "Apply", "Attach"),
                new MoveCommand(0,  "RT - Climber - Lift Grab", "Check", "Attach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "lockGrabber", new List<IMoveCommand>() {
                new MoveCommand(2,  "RT - Hangar "+hangarName+" - Grab Lift", "Apply", "Attach"),
                new MoveCommand(2,  "RT - Hangar "+hangarName+" - Grab Lift", "Check", "Attach"),
                new MoveCommand(3,  "RT - Climber - Lift Grab",               "Apply", "Detach"),
                new MoveCommand(3,  "RT - Climber - Lift Grab",               "Check", "Detach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "unlockGrabber", new List<IMoveCommand>() {
                new MoveCommand(0,  "RT - Climber - Lift Grab",               "Check", "Attach"),
                new MoveCommand(0,  "RT - Hangar "+hangarName+" - Grab Lift", "Apply", "Detach"),
                new MoveCommand(0,  "RT - Hangar "+hangarName+" - Grab Lift", "Check", "Detach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "checkAttached", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Check", "Attach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "setupRetract", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift",         "Apply", "Retract"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Lift Reverse", "Apply", "Extend"),
                new MoveCommand(13, "RT - Hangar "+hangarName+" - Lift",         "Check", "Retract"),
                new MoveCommand(13, "RT - Hangar "+hangarName+" - Lift Reverse", "Check", "Extend"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "attach", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Check", "Detach"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Apply", "Attach"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Check", "Attach"),
            }));

            this.actions.Add(new MoveAction(this.myProgram, "detach", new List<IMoveCommand>() {
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Check", "Attach"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Apply", "Detach"),
                new MoveCommand(0,  "PT - Hangar "+hangarName+" - Grab Lift",    "Check", "Detach"),
            }));
        }
    }
}
