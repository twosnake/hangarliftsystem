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
    class MechanicManager
    {
        protected List<IRunningMechanic> mechanisms = new List<IRunningMechanic>();

        public void add(IRunningMechanic mech)
        {
            this.mechanisms.Add(mech);
        }

        public void clear()
        {
            this.mechanisms.Clear();
        }

        public IRunningMechanic getMechanic(string name)
        {
            foreach (var mech in this.mechanisms)
            {
                if (mech.getName() == name)
                {
                    return mech;
                }
            }
            throw new Exception("No mechanism Found '" + name + "'");
        }
    }
}
