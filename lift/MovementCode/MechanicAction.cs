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
    interface IMechanicAction
    {
        string getName();
        bool isDone();
        bool hasError();
        bool hasStarted();
        void execute();
        void process();
        List<string> getActions();
    }

    class MechanicAction : IMechanicAction
    {
        protected IRunningMechanic mech;
        protected List<string> actions;
        Program myProgram;
        protected bool isProcessed;

        public MechanicAction(Program myProgram, IRunningMechanic mech, List<string> actions)
        {
            this.mech = mech;
            this.isProcessed = false;
            this.myProgram = myProgram;
            this.actions = new List<string>(actions);
        }

        public void execute()
        {
            this.mech.runActionQueue();
        }

        public void process()
        {
            if (this.isProcessed)
            {
                return;
            }

            foreach (var action in this.actions)
            {
                this.mech.queueAction(action);
            }
            this.isProcessed = true;
        }

        public bool hasStarted()
        {
            return this.isProcessed;
        }

        public bool isDone()
        {
            return !this.mech.hasActions() && !this.mech.hasStopped();
        }

        public bool hasError()
        {
            return this.mech.hasStopped();
        }

        public List<string> getActions()
        {
            return this.actions;
        }

        public string getName()
        {
            return this.mech.getName();
        }
    }
}
