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
    interface IHangarMechanic
    {
        string getName();
        int getSubFloor();
        void setCall(bool isCalled);
        void setReturn();
        void setReturned();
        bool hasCalled();
        void recievedCall();
        bool wantsReturned();
        HangarLiftSlot getLiftSlotMech();
    }

    class HangarMechanic :  MyGridProgram, IHangarMechanic
    {
        protected int subfloor;
        protected string name;
        protected Lift myLift;
        protected bool isCalled  = false;
        protected bool isWaiting = false;
        protected bool isReturn  = false;
        protected HangarLiftSlot liftSlotMech;

        public HangarMechanic(Lift myLift, int subfloor, string name)
        {
            this.myLift = myLift;
            this.subfloor = subfloor;
            this.name = name;

            this.liftSlotMech = new HangarLiftSlot(myLift.myProgram, name);
            this.myLift.mechMgr.add(this.liftSlotMech);
        }

        public HangarLiftSlot getLiftSlotMech()
        {
            return this.liftSlotMech;
        }

        public string getName()
        {
            return this.name;
        }

        public int getSubFloor()
        {
            return this.subfloor;
        }

        public void setCall(bool isCalled)
        {
            if (this.isWaiting)
            {
                return;
            }
            this.isCalled = isCalled;
        }

        public void setReturn()
        {
            this.isWaiting = false;
            this.isCalled  = false;
            this.isReturn = true;
        }

        public void setReturned()
        {
            this.isWaiting = false;
            this.isCalled = false;
            this.isReturn = false;
        }

        public bool hasCalled()
        {
            return this.isCalled;
        }

        public bool wantsReturned()
        {
            return this.isReturn;
        }

        public void recievedCall()
        {
            this.isCalled = false;
            this.isWaiting = true;
        }
    }
}
