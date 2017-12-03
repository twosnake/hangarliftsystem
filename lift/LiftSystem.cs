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
    class LiftDirection
    {
        public const int Stopped = 0;
        public const int Down = 1;
        public const int Up = 2;
    }

    class LiftAction
    {
        public const int Stopped = 0;
        public const int MovingToHangar = 1;
        public const int MovingToAirlock = 2;
    }

    class LiftSystem
    {
        Program myProgram;
        HangarManager hangarMgr;
        MechanicManager mechMgr;

        protected Stack<string> hangarsWaiting;
        protected string currentHangar;
        protected int action;
        protected int direction;
        List<MechanicAction> actionQueue;
        protected bool hasStarted;

        public LiftSystem(Program myProgram, HangarManager hangarMgr, MechanicManager mechMgr)
        {
            this.myProgram = myProgram;
            this.hangarMgr = hangarMgr;
            this.mechMgr = mechMgr;
            this.hangarsWaiting = new Stack<string>();
            this.currentHangar = "";
            this.direction = LiftDirection.Stopped;
            this.actionQueue = new List<MechanicAction>();
        }

        public int getDirection()
        {
            return this.direction;
        }

        public string getTargetHangar()
        {
            return this.currentHangar;
        }

        public int getAction()
        {
            return this.action;
        }

        public void process()
        {
            this.hangarMgr.process();
            if (this.currentHangar != "")
            {
                return;
            }
            this.currentHangar = this.hangarMgr.getNextWaiting();
        }

        public void execute()
        {
            IHangarMechanic hangar;

            if (this.currentHangar == "")
            {
                return;
            }

            if (this.hasStarted == true && this.actionQueue.Count() == 0)
            {
                this.myProgram.Echo("Lift is done");
                this.direction = LiftDirection.Stopped;
                this.action = LiftAction.Stopped;
                this.hasStarted = false;
                this.currentHangar = "";
                return;
            }

            if (this.actionQueue.Count() != 0)
            {
                IMechanicAction action = this.actionQueue.First();
                this.myProgram.Echo(action.getName());

                if (action.hasError())
                {
                    this.myProgram.Echo("Action Error "+action.getName());
                    this.actionQueue.Clear();
                    this.direction = LiftDirection.Stopped;
                    this.action = LiftAction.Stopped;
                    hangar = this.hangarMgr.getHangar(this.currentHangar);
                    hangar.setReturned();
                    this.hasStarted = true;
                    this.currentHangar = "";
                    return;
                }

                if (!action.hasStarted())
                {
                    this.myProgram.Echo("Processing action");
                    action.process();
                    return;
                }

                if (action.hasStarted() && !action.isDone())
                {
                    this.myProgram.Echo("Action running");
                    action.execute();
                    return;
                }

                if (action.isDone())
                {
                    this.myProgram.Echo("Popping Action "+action.getName());
                    this.actionQueue.RemoveAt(0);
                    return;
                }
            }

            hangar = this.hangarMgr.getHangar(this.currentHangar);
            HangarLiftSlot hangerliftslot = (HangarLiftSlot)this.mechMgr.getMechanic("hangerlift" + hangar.getName());
            Climber climber = (Climber) this.mechMgr.getMechanic("climber");
            if (climber.hasActions())
            {
                return;
            }

            this.action = LiftAction.MovingToHangar;
            int moveFloors = climber.getSubFloor() - hangar.getSubFloor();

            string cmd = "";
            if (moveFloors > 0)
            {
                this.direction = LiftDirection.Up;
                cmd = "up";
            } else
            {
                this.direction = LiftDirection.Down;
                cmd = "down";
                moveFloors = moveFloors * -1;
            }

            List<String> climberList = new List<string>();
            for (int i = 0; i < moveFloors; i++) {
                climberList.Add(cmd);
            }

            // Move hangar piston out
            this.actionQueue.Add(new MechanicAction(this.myProgram, hangerliftslot, new List<string> {
                "moveOut",
            }));
            // Move Climber in place
            this.actionQueue.Add(new MechanicAction(this.myProgram, climber, climberList));
            // Lock lift to climber
            this.actionQueue.Add(new MechanicAction(this.myProgram, hangerliftslot, new List<string> {
                "lockClimber",
                "unlockGrabber",
                "moveInNoLift"
            }));
            // Move climber up
            climberList = new List<string>();
            for (int i = hangar.getSubFloor(); i != 0; i--)
            {
                climberList.Add("up");
            }
            climberList.Add("upAirLock");
            this.actionQueue.Add(new MechanicAction(this.myProgram, climber, climberList));

            // move down actions
            this.actionQueue.Add(new MechanicAction(this.myProgram, climber, new List<string> {
                "downAirLock",
                "down",
                "down",
            }));
            this.actionQueue.Add(new MechanicAction(this.myProgram, hangerliftslot, new List<string> {
                "moveOutNoLift",
                "lockGrabber",
            }));
            this.actionQueue.Add(new MechanicAction(this.myProgram, climber, new List<string> {
                "down",
            }));
            this.actionQueue.Add(new MechanicAction(this.myProgram, hangerliftslot, new List<string> {
                "moveIn"
            }));

            this.hasStarted = true;
        }
    }
}
