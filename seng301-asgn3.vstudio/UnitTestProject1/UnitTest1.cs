using System;
using System.Collections.Generic;
using System.Linq;
using Frontend2;
using Frontend2.Hardware;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckTestWorking()
        {
            Assert.AreEqual(true, true);
        }

        //================
        // T01 Test Method
        //================
        // This method recreates and tests
        // the first good test script
        [TestMethod]
        public void goodInsertAndPressExactChange()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "Coke", "water", "stuff" };
            List<int> popCosts = new List<int> { 250, 250, 205 };
            vm.Configure(popNames, popCosts);
            // load coins
            loadCoins(5, 1, 0, vm);
            loadCoins(10, 1, 1, vm);
            loadCoins(25, 2, 2, vm);
            loadCoins(100, 0, 3, vm);

            // load pops
            loadPops("Coke", 1, 0, vm);
            loadPops("water", 1, 1, vm);
            loadPops("stuff", 1, 2, vm);

            // insert coins
            insertCoins(new int[] { 100, 100, 25, 25 }, vm);
            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            //===============
            // Check Delivery
            //===============
            // now check items
            int expectedItems = 1;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable> { new PopCan("Coke") };
            // check if delivery correct
            Boolean success = checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            //===============
            // Check Teardown
            //===============
            // check if teardown correct
            // get the teardown items
            var storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm.CoinRacks) {
                Console.WriteLine("Unloading...");
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach(var popCanRack in vm.PopCanRacks) {
                Console.WriteLine("Unloading pops...");
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // create expected lists
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(10) },
                new List<Coin> { new Coin(25), new Coin(25), new Coin(25), new Coin(25) },
                new List<Coin> { new Coin(100), new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };
            foreach(var popList in storedContents.PopCansInPopCanRacks)
            {
                Console.WriteLine(popList.Count);
            }
            success = checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T02 Test Method
        //================
        // This method recreates and tests
        // the second good test script
        [TestMethod]
        public void goodInsertAndPressChangeExpected()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "Coke", "water", "stuff" };
            List<int> popCosts = new List<int> { 250, 250, 205 };
            vm.Configure(popNames, popCosts);
            // load coins
            loadCoins(5, 1, 0, vm);
            loadCoins(10, 1, 1, vm);
            loadCoins(25, 2, 2, vm);
            loadCoins(100, 0, 3, vm);

            // load pops
            loadPops("Coke", 1, 0, vm);
            loadPops("water", 1, 1, vm);
            loadPops("stuff", 1, 2, vm);

            // insert coins
            insertCoins(new int[] { 100, 100, 100}, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            Assert.AreEqual(true, true);
        }

        //================
        // HELPER METHODS
        //================

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
                        if (item.GetType() == typeof(PopCan))
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
                        if (item.GetType() == typeof(Coin))
                        {
                            Coin delCoin = (Coin)delivered;
                            int delCoinVal = delCoin.Value;
                            if (delCoinVal.Equals(coinVal))
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
                for(int j=0;i<storedContents.PopCansInPopCanRacks[i].Count;j++)
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
