﻿using Sandbox.Game.EntityComponents;
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
    class LiftStatusText : MyGridProgram
    {
        Program myProgram;
        LiftSystem lift;
        MechanicManager mechMgr;
        HangarManager hangarMgr;
        List<string> screen;

        public List<string> hangarIconWhite = new List<String>(){
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };

        public List<string> hangarIconRed = new List<String>()
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
        };

        public List<string> hangarIconGreen = new List<String>()
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
        };

        public List<string> hangarAirlockIconRed = new List<String>()
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };

        public List<string> hangarAirlockIconGreen = new List<String>()
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
        };

        public bool isReady = false;

        public LiftStatusText(Program myProgram, LiftSystem lift, HangarManager hangarMgr, MechanicManager mechMgr)
        {
            this.screen = new List<string>();
            this.myProgram = myProgram;
            this.lift = lift;
            this.hangarMgr = hangarMgr;
            this.mechMgr = mechMgr;
        }

        public void process()
        {
            this.getLiftStatus();
            this.displayText();
        }

        public void getLiftStatus() { 
            if (!this.isReady)
            {
                //this.addLine("Lift is booting please wait");
                return;
            }

            int action = this.lift.getAction();
            if (action == LiftAction.Stopped)
            {
                //this.addLine("Lift is ready");
            }
            if (action == LiftAction.MovingToHangar)
            {
                //this.addLine("Lift is moving to hanger '" +lift.getTargetHangar()+"'");
            }

            int direction = this.lift.getDirection();
            if (direction == LiftDirection.Down)
            {
                //this.addLine("Lift is going down");
            }

            if (direction == LiftDirection.Up)
            {
                //this.addLine("Lift is going up");
            }
        }

        public void displayText()
        {
            IMyTextPanel block = (IMyTextPanel) this.myProgram.GridTerminalSystem.GetBlockWithName("LCD - Hangar 1 - Outer Airlock");
            if (block == null)
            {
                throw new Exception("Can not find LCD block");
            }
            this.screen = new List<string>(block.CustomData.Split('\n'));

            
            this.writeSquare(this.hangarAirlockIconGreen, 10, 100);
            this.writeSquare(this.hangarIconRed, 20, 40);
            this.writeSquare(this.hangarIconWhite, 50, 40);
            this.writeSquare(this.hangarIconGreen, 80, 40);
            this.writeSquare(this.hangarIconGreen, 110, 40);
            
            string str = "";
            foreach(var txt in this.screen)
            {
                str += txt + "\n";
            }
            block.WritePublicText(str);
            block.ShowPublicTextOnScreen();
        }

        protected void writeSquare(List<string> data, int x, int y)
        {
            int i = 0;
            foreach(var row in data)
            {
                StringBuilder sb = new StringBuilder(this.screen.ElementAt<string>(x + i));
                StringBuilder sbc = new StringBuilder(row);
                var j = 0;
                foreach(var col in row)
                {
                    sb[j + y] = sbc[j];
                    j++;
                }
                i++;
                this.screen[x + i] = sb.ToString();
            }
        }
    }
}