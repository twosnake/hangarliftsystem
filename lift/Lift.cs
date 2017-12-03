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
    class Lift : MyGridProgram
    {
        public int totalSubFloors = 5;
        public bool isSetup = false;
        public Stack<ISetupMechanic> setupList;
        public HangarManager hangarMgr;
        public MechanicManager mechMgr;
        public LiftStatusText txt;
        public LiftSystem liftSys;
        public Program myProgram;

        public Lift(Program myProgram)
        {
            this.myProgram = myProgram;

            this.mechMgr = new MechanicManager();
            this.mechMgr.add(new Climber(myProgram, this.totalSubFloors));

            this.hangarMgr = new HangarManager();
            this.hangarMgr.add(new HangarMechanic(this, 2, "1"));

            this.liftSys = new LiftSystem(myProgram, this.hangarMgr, this.mechMgr);
            this.txt = new LiftStatusText(myProgram, this.liftSys, this.hangarMgr, this.mechMgr);

            this.setupList = new Stack<ISetupMechanic>();
            this.setupList.Push(new SetupClimber(myProgram, this.mechMgr));
            this.setupList.Push(new SetupHangarLiftSlot(myProgram, mechMgr, this.hangarMgr));
        }

        public void main(string argument, UpdateType updateType)
        {
            if ((updateType & (UpdateType.Trigger | UpdateType.Terminal)) != 0)
            {
                RunCommand(argument);
                return;
            }

            if ((updateType & UpdateType.Update100) != 0)
            {
                if (this.isSetup == false)
                {
                    this.myProgram.Echo("Setup Loop");
                    this.SetupLoop();
                }
                else
                {
                    this.myProgram.Echo("Main Loop");
                    this.MainLoop();
                }
            }

            this.txt.process();
        }

        public void RunCommand(string argument)
        {
            string[] cmd = argument.Split('-');
            if (this.isSetup)
            {
                if (cmd[0] == "Activate" && cmd[1] == "Hangar")
                {
                    if (cmd[2] == null)
                    {
                        return;
                    }

                    this.myProgram.Echo("Calling for hangar " + cmd[2]);
                    IHangarMechanic hangar = this.hangarMgr.getHangar(cmd[2]);
                    hangar.setCall(true);
                }
            }
        }

        public void SetupLoop()
        {
            this.myProgram.Echo("Setup Loop");
            if (this.setupList.Count() == 0)
            {
                this.isSetup = true;
                return;
            }
            ISetupMechanic setup = this.setupList.Peek();
            if (setup.isReady())
            {
                this.txt.isReady = true;
                this.setupList.Pop();
                return;
            }
            setup.run();
        }

        public void MainLoop()
        {
            this.liftSys.process();
            this.liftSys.execute();
        }
    }
}
