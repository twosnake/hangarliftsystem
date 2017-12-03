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
    interface IMoveCommand
    {
        bool process(Program myProgram, int timePassed);
        bool done();
        bool hasError();

        int getSecondsDelay();
        string getBlockName();
        string getAction();
        string getValue();
    }

    class MoveCommand : MyGridProgram, IMoveCommand
    {
        protected int secondsDelay;
        protected string blockName;
        protected string action;
        protected string value;
        protected bool processed;
        protected bool haltError;
        protected Program myProgram;

        public MoveCommand(IMoveCommand command)
        {
            this.secondsDelay = command.getSecondsDelay();
            this.blockName = command.getBlockName();
            this.action = command.getAction();
            this.value = command.getValue();
            this.processed = false;
            this.haltError = false;
        }

        public MoveCommand(int secondsDelay, string blockName, string action, string value)
        {
            this.haltError = false;
            this.secondsDelay = secondsDelay;
            this.blockName = blockName;
            this.action = action;
            this.value = value;
            this.processed = false;
        }

        override public string ToString()
        {
            return "MoveCommand " + this.secondsDelay.ToString() + ", " + this.blockName + ", " + this.action + ", " + this.value;
        }

        public int getSecondsDelay()
        {
            return this.secondsDelay;
        }

        public string getBlockName()
        {
            return this.blockName;
        }

        public string getAction()
        {
            return this.action;
        }

        public string getValue()
        {
            return this.value;
        }

        public bool process(Program myProgram, int timePassed)
        {
            this.myProgram = myProgram;

            if (this.processed)
            {
                return true;
            }

            if (this.haltError)
            {
                return false;
            }

            if (timePassed >= this.secondsDelay)
            {
                this.myProgram.Echo("C: " + this.getBlockName()+" A: "+this.getAction()+" V: "+this.getValue());
                this.executeAction();
                if (!this.hasError()) {
                    this.processed = true;
                    return true;
                }                
            }
            return false;            
        }

        public bool hasError()
        {
            return this.haltError;
        }

        public bool done()
        {
            return this.processed && !this.haltError;
        }

        protected void executeAction()
        {
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            this.myProgram.GridTerminalSystem.SearchBlocksOfName(this.blockName, blocks);
            foreach (IMyTerminalBlock block in blocks)
            {
                if (block.CustomName != this.blockName)
                {
                    continue;
                }

                this.executeBlock(block);
            }
        }

        protected void executeBlock(IMyTerminalBlock block)
        {
            this.myProgram.Echo(this.blockName+" "+this.action+" "+this.value);
            if (block == null)
            {
                this.haltError = true;
                return;
            }

            switch (this.action)
            {
                case "Apply":
                    if (this.value == "On")
                    {
                        this.value = "OnOff_On";
                    }
                    if (this.value == "Off")
                    {
                        this.value = "OnOff_Off";
                    }
                    block.ApplyAction(this.value);
                    return;
                case "Check":
                    if (this.value == "On" || this.value == "Off")
                    {
                        bool onoff = block.GetValueBool("OnOff");
                        if (this.value == "On" && !onoff)
                        {
                            this.haltError = true;
                            return;
                        }
                        if (this.value == "Off" && onoff)
                        {
                            this.haltError = true;
                            return;
                        }
                    }
                    if (this.value == "Attach" || this.value == "Detach")
                    {
                        bool rotorAttached = ((IMyMotorStator)block).IsAttached;
                        if (this.value == "Attach" && !rotorAttached)
                        {
                            this.haltError = true;
                            return;
                        }
                        if (this.value == "Detach" && rotorAttached)
                        {
                            this.haltError = true;
                            return;
                        }
                    }
                    if (this.value == "Extend" || this.value == "Retract")
                    {
                        PistonStatus pistonExtended = ((IMyPistonBase)block).Status;
                        if (pistonExtended == PistonStatus.Stopped)
                        {
                            this.haltError = true;
                            return;
                        }

                        if (this.value == "Extend" && pistonExtended == PistonStatus.Retracted)
                        {
                            this.haltError = true;
                            return;
                        }
                        if (this.value == "Retract" && pistonExtended == PistonStatus.Extended)
                        {
                            this.haltError = true;
                            return;
                        }
                    }
                    return;
                case "Velocity":
                    block.SetValue<Single>("Velocity", Single.Parse(this.value));
                    return;
                case "MaxLimit":
                    block.SetValue<Single>("UpperLimit", Single.Parse(this.value));
                    return;
                case "MinLimit":
                    block.SetValue<Single>("LowerLimit", Single.Parse(this.value));
                    return;
            }
        }
    }
}
