using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public class State
    {
        public string StateName = "Kazakhstan";
        public short ID = 0;


        /// <summary>
        /// List of other states IDs that is permited to trade with this state
        /// </summary>
        public List<short> TradingEnabledWith = new List<short>();

        public State(string Name, short ID)
        {
            this.StateName = Name;
            this.ID = ID;

            TradingEnabledWith.Add(ID);
        }
    }
}
