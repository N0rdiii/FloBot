﻿using FloBot.MemoryClass;
using FloBot.Model;
using FloBot.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloBot.Main
{
    class StateMachine
    {
        private mainForm main_form;
        private MemoryClass.MemoryRW mc;
        private Player player;
        private Thread botThread;
        private Thread abortThread;
        private bool runBot = true;
        public StateMachine(mainForm main_form,Player player)
        {
            this.main_form = main_form;
            this.mc = new MemoryRW();
            this.player = player;
            currentState = new FindGameState();
            AddressUtil.setMemoryRW(this.mc);
            botThread = new Thread(start_Method);
            botThread.Start();
            abortThread = new Thread(check_Abort_Bot);
            abortThread.Start();
        }

        public void endBotThread()
        {
            botThread.Abort();
            abortThread.Abort();
        }

        private IState currentState;
        private void start_Method()
        {
            while (!main_form.IsDisposed && runBot)
            {
                currentState = currentState.doTasks(main_form, mc, player);
                Thread.Sleep(50);
            }
                
        }

        private void check_Abort_Bot()
        {
            while(true)
            {
                Thread.Sleep(500);
                if (mc.isGameInForeground() && main_form.cbDisableBot.Checked)
                {
                    runBot = false;
                    botThread.Join();
                }
                if (!botThread.IsAlive && !mc.isGameInForeground() && main_form.cbDisableBot.Checked)
                {
                    runBot = true;
                    botThread = new Thread(start_Method);
                    botThread.Start();
                }
            }

        }
    }
}
