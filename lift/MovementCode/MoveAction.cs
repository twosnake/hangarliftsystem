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
    interface IMoveAction
    {
        string getName();
        List<IMoveCommand> getCommands();
    }

    class MoveAction : IMoveAction
    {
        protected string name;
        protected List<IMoveCommand> commands = new List<IMoveCommand>();
        Program myProgram;

        public MoveAction(Program myProgram, string name, List<IMoveCommand> commands)
        {
            this.name = name;
            this.myProgram = myProgram;
            this.commands.AddRange(commands);
        }

        public List<IMoveCommand> getCommands()
        {
            return this.commands;
        }

        public string getName()
        {
            return this.name;
        }
    }
}
