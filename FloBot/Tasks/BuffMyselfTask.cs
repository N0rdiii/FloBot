﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloBot.MemoryClass;
using FloBot.Model;
using System.Threading;
using System.Windows.Forms;

namespace FloBot.Tasks
{
    class BuffMyselfTask : ITask
    {
        public bool doTask(MemoryRW mc)
        {
            foreach (Skill skill in DataNeededCrossTaskUtil.BuffArray)
            {
                Thread.Sleep(1500);
                if (AddressUtil.getTargetCurrentHP() > 0 && !AddressUtil.getTargetName().Contains(AddressUtil.getCharName().Substring(16)))
                    return true;
                if (skill.skillCanBeUsed())
                {
                    Console.WriteLine("Should buff");
                    mc.sendKeystroke(skill.Hotkey);
                    skill.LastTimeUsed = DateTime.Now;
                    //DataNeededCrossTaskUtil.Buffed = true;
                    Thread.Sleep(1500);
                }

            }

            String charName = AddressUtil.getCharName().Replace( ((char)0).ToString(),string.Empty);
            
            while (AddressUtil.getTargetName().Contains(charName)&&AddressUtil.getTargetCurrentHP() > 0)           
            {
              
                mc.sendKeystroke(Keys.Escape);
                Thread.Sleep(500);
            }
           
            return true;
        }

        public bool doTask(mainForm main_form, MemoryRW mc)
        {
            throw new NotImplementedException();
        }
    }
}
