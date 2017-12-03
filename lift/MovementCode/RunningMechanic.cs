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
    interface IRunningMechanic
    {
        bool queueAction(string action);
        bool execAction(string action);
        void clearActionQueue();
        bool hasActions();
        void reset();
        bool hasStopped();
        void runActionQueue();
        string getName();
    }

    class RunningMechanic : MyGridProgram, IRunningMechanic
    {
        protected List<IMoveAction> actions;
        protected List<IMoveCommand> runningCommands;
        protected List<String> actionQueue;
        protected int timedelay = 0;
        protected bool hasError = false;
        protected Program myProgram;
        protected string name = "";

        public RunningMechanic(Program myProgram)
        {
            this.reset();
            this.myProgram = myProgram;
            this.actions = new List<IMoveAction>();
        }

        public string getName()
        {
            return this.name;
        }

        virtual public IMoveAction getAction(string name)
        {
            foreach (var action in this.actions)
            {
                if (action.getName() == name)
                {
                    return action;
                }
            }
            throw new Exception("No Action Found '"+name+"'");
        }

        virtual public bool queueAction(string action)
        {
            if (this.getAction(action) == null)
            {
                return false;
            }
            this.actionQueue.Add(action);
            return true;
        }

        virtual public bool execAction(string action)
        {
            if (this.runningCommands.Count() != 0)
            {
                return false;
            }
            this.timedelay = 0;
            this.runningCommands = new List<IMoveCommand>();
            this.getAction(action).getCommands().ToList().ForEach((command) =>
            {
                this.runningCommands.Add(new MoveCommand(command));
            });
            return true;
        }

        virtual public void clearActionQueue()
        {
            this.runningCommands.Clear();
        }

        virtual public bool hasActions()
        {
            return this.runningCommands.Count() != 0 || this.actionQueue.Count() != 0;
        }

        virtual public void reset()
        {
            this.actionQueue = new List<string>();
            this.runningCommands = new List<IMoveCommand>();
            this.hasError = false;
        }

        virtual public bool hasStopped()
        {
            return this.hasError;
        }

        virtual public void runActionQueue()
        {
            if (this.hasError)
            {
                foreach (var command in this.runningCommands)
                {
                    if (command.hasError())
                    {
                        this.myProgram.Echo("Command Error: " + this.getName());
                        this.myProgram.Echo(command.getBlockName() + " - " + command.getAction() + " - " + command.getValue());
                    }
                }
                this.runningCommands.Clear();
                this.actionQueue.Clear();
                return;
            }

            if (this.runningCommands.Count() == 0)
            {
                if (this.actionQueue.Count() == 0 || this.hasError)
                {
                    return;
                }
                
                if (!this.execAction(this.actionQueue.ElementAt(0)))
                {
                    this.hasError = true;
                }
                this.actionQueue.RemoveAt(0);
            }

            this.timedelay += this.myProgram.Runtime.TimeSinceLastRun.Seconds;
            this.myProgram.Echo("Delay: " + this.timedelay.ToString());
            foreach (var command in this.runningCommands)
            {
                command.process(this.myProgram, this.timedelay);
                if (command.hasError())
                {
                    this.hasError = true;
                    return;
                }
            }
            this.runningCommands.RemoveAll(command => command.done() == true);
        }
    }
}

