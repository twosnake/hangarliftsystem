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
    class HangarManager
    {
        protected List<IHangarMechanic> hangars = new List<IHangarMechanic>();
        protected Stack<string> hangersWaiting;

        public HangarManager()
        {
            this.hangersWaiting = new Stack<string>();
        }

        public void add(IHangarMechanic hangar)
        {
            this.hangars.Add(hangar);
        }

        public void clear()
        {
            this.hangars.Clear();
        }

        public IHangarMechanic getHangar(string name)
        {
            foreach (var hangar in this.hangars)
            {
                if (hangar.getName() == name)
                {
                    return hangar;
                }
            }
            throw new Exception("No hangar Found '" + name + "'");
        }

        public List<IHangarMechanic> getAllHangar()
        {
            return this.hangars;
        }

        public void process()
        {
            foreach (var hangar in this.hangars)
            {
                if (hangar.hasCalled())
                {
                    this.hangersWaiting.Push(hangar.getName());
                    hangar.recievedCall();
                }
            }
        }

        public Stack<string> getWaiting()
        {
            return this.hangersWaiting;
        }

        public string getNextWaiting()
        {
            if (this.hangersWaiting.Count() == 0)
            {
                return "";
            }

            return this.hangersWaiting.Pop();
        }
    }
}
