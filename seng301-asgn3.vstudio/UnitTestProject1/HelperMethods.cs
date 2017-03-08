// HelperMethods
// Contains helper methods
// for repetitve tasks in
// the good and bad test
// scripts

using System;
using System.Collections.Generic;
using System.Linq;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject1
{
    public class HelperMethods
    {
        public HelperMethods()
        {
        }

        //======================================//
        //            HELPER METHODS            //
        //======================================//

        public void loadCoins(int coinValue, int number, int index, VendingMachine vm)
        {
            List<Coin> coins = new List<Coin>();
            for (int i = 0; i < number; i++)
            {
                coins.Add(new Coin(coinValue));
            }
            vm.CoinRacks[index].LoadCoins(coins);
        }

        public void loadPops(string popName, int number, int index, VendingMachine vm)
        {
            List<PopCan> pops = new List<PopCan>();
            for (int i = 0; i < number; i++)
            {
                pops.Add(new PopCan(popName));
            }
            vm.PopCanRacks[index].LoadPops(pops);
        }

        public void insertCoins(int[] coinValues, VendingMachine vm)
        {
            foreach (int val in coinValues)
            {
                vm.CoinSlot.AddCoin(new Coin(val));
            }
        }

        // takes the expected delivery items and ensures that all
        // expected items are in the actual delivered items
        // returns true if expected items are there, false if not
        public Boolean checkDelivery(List<IDeliverable> expectedList, List<IDeliverable> itemsList)
        {
            foreach (var item in expectedList)
            {
                Boolean itemFound = false;
                if (item.GetType() == typeof(PopCan))
                {
                    PopCan pop = (PopCan)item;
                    string name = pop.Name;
                    foreach (var delivered in itemsList)
                    {
                        if (delivered.GetType() == typeof(PopCan))
                        {
                            PopCan delPop = (PopCan)delivered;
                            string delPopName = delPop.Name;
                            if (name.Equals(delPopName))
                            {
                                itemFound = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Coin coinItem = (Coin)item;
                    int coinVal = coinItem.Value;
                    foreach (var delivered in itemsList)
                    {
                        if (delivered.GetType() == typeof(Coin))
                        {
                            Coin delCoin = (Coin)delivered;
                            int delCoinVal = delCoin.Value;
                            if (delCoinVal == coinVal)
                            {
                                itemFound = true;
                                break;
                            }
                        }
                    }
                }
                if (!itemFound)
                    return false;
            }
            return true;
        }
        
        // checks if teardown items are correct
        public Boolean checkTeardown(VendingMachineStoredContents storedContents, List<List<Coin>> expectedCoins,
            List<List<PopCan>> expectedPops, List<Coin> expectedStorage)
        {
            // loop over each pop rack
            for(int i=0; i<storedContents.PopCansInPopCanRacks.Count;i++)
            {
                // check length
                if (storedContents.PopCansInPopCanRacks[i].Count != expectedPops[i].Count)
                    return false;
                // loop over each pop in each rack
                for(int j=0;j<storedContents.PopCansInPopCanRacks[i].Count;j++)
                {
                    // if any non matches, return false
                    if(!(storedContents.PopCansInPopCanRacks[i][j].Name.Equals(expectedPops[i][j].Name)))
                    {
                        return false;
                    }
                }
            }

            // loop over each coin rack
            for(int i=0; i<storedContents.CoinsInCoinRacks.Count;i++)
            {
                // check length
                if (storedContents.CoinsInCoinRacks[i].Count != expectedCoins[i].Count)
                    return false;
                // loop over each coin in each rack
                for(int j=0;j<storedContents.CoinsInCoinRacks[i].Count;j++)
                {
                    // if any non matches, return false
                    if(!(storedContents.CoinsInCoinRacks[i][j].Value == expectedCoins[i][j].Value))
                    {
                        return false;
                    }
                }
            }
            // loop over each coin in storage bin
            for(int i=0;i<storedContents.PaymentCoinsInStorageBin.Count;i++)
            {
                // if any non matches, return false
                if(!(storedContents.PaymentCoinsInStorageBin[i].Value == expectedStorage[i].Value))
                {
                    return false;
                }
            }

            // if no fails, return true
            return true;
        }
    }
}