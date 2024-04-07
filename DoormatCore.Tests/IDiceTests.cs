using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoormatCore.Tests
{
    public class DiceTests
    {
        //Tests for dice?
        //bet with negative amount: -0.00001 // should trigger error event with appropriate message
        //bet with invalid chance: 0%,0.1%;1%,99%,99.99%,100% // should trigger error event with appropriate message
        //bet with 0 amount if supported by site else should trigger error event with appropriate message
        //bet with over site max profit amount (int.max) //should trigger error event with appropriate message
        //bet over user balance, below site max profit) //should trigger error event with appropriate message 
        //valid bet at different chances and currencies: (amounts 0.0000001, chance 5%-95% with 10% steps) // should fire bet finished event with appropriate bet values

    }
}
