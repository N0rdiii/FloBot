﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloBot.MemoryClass;
using System.Windows.Forms;
using System.Threading;
using FloBot.Model;

namespace FloBot.Tasks
{
    class GetRestTask : ITask
    {
        
        public bool doTask(mainForm main_form, Player player)
        {
            throw new NotImplementedException();
        }
        private static int oldHP = -1;
        public bool doTask(mainForm main_form, MemoryRW mc, Player player)
        {
            
            //Check if in combat yes -> return false (not resting) incase you were resting, stand up and set resting to false
            if (player.inCombat)
                if (player.Resting)
                {
                    mc.sendKeystroke(Keys.Z);
                    player.Resting = false;
                    oldHP = -1;
                    return false;
                }
                else
                    return false;

            if(player.Resting && oldHP > player.PlayerCurrentHP)
            {
               
               //layer.Resting = false;
               //c.sendKeystroke(Keys.Z);
               //thread.Sleep(100);
                mc.sendKeystroke(Keys.Tab);
                oldHP = -1;
                return false;
            }

            //Check if player is not resting but needs to rest
            if(!player.Resting && (getRestHP(main_form,player) || getRestMP(main_form, player)))
            {
                while (player.Pos.moved())
                    Thread.Sleep(100);
                //int count = 0;
                //while (count++ < 15 && !player.inCombat) Thread.Sleep(100);

                if (player.inCombat)
                    return false;
                //int counter = 0;
                //while ((oldHP = player.PlayerCurrentHP) <= 0 && counter++ < 10) Thread.Sleep(100);
                oldHP = player.PlayerCurrentHP;
                player.Resting = true;
                mc.sendKeystroke(Keys.Z);
                
                return true;
            }
            //Check if player has finished resting
            if(player.Resting)
            {
                if (player.PlayerMaxHP == player.PlayerCurrentHP && player.PlayerMaxMP == player.PlayerCurrentMP)
                {
                    oldHP = -1;
                    mc.sendKeystroke(Keys.Z);
                    player.Resting = false;
                    Thread.Sleep(1500);
                    return false;
                }
                else
                {
                    if (oldHP < player.PlayerCurrentHP)
                        oldHP = player.PlayerCurrentHP;
                    Thread.Sleep(200);
                    return true;
                }
            }
            return false;

                  
        }

        private bool getRestHP(mainForm main_form, Player player)
        {
           return ( (player.PlayerMaxHP) / 100 * main_form.tbRestHP.Value) > player.PlayerCurrentHP;
        }
        private bool getRestMP(mainForm main_form, Player player)
        {
            return ((player.PlayerMaxMP) / 100 * main_form.tbRestMP.Value) > player.PlayerCurrentMP;
        }

    }
}
