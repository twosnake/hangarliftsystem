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
    partial class Program : MyGridProgram
    {
        Lift lift;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            this.lift = new Lift(this);
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateType)
        {
/*
            var b = (IMySensorBlock)GridTerminalSystem.GetBlockWithName("SN - Lift 1");
            Echo("Name "+b.LastDetectedEntity.Name);
            Echo("Velo " + b.LastDetectedEntity.Velocity.ToString());
            return;
*/
            try
            {
                this.lift.main(argument, updateType);
            }
            catch (Exception e)
            {
                Echo(e.Message);
                Echo(e.StackTrace.ToString());
                Echo("------");
                Echo(e.ToString());
            }
        }

        
    }
}