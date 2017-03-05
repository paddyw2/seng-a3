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
            // 1
            int coinKindIndex = 0;
            List<Coin> coins = new List<Coin> { new Coin(5)};
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);
            // 2
            coinKindIndex = 1;
            coins.Clear();
            coins = new List<Coin> { new Coin(10)};
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);
            // 3
            coinKindIndex = 2;
            coins.Clear();
            coins = new List<Coin> { new Coin(25), new Coin(25)};
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);
            // 4
            coinKindIndex = 3;
            coins.Clear();
            coins = new List<Coin> { new Coin(100)};
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);
            // load pops
            // 1
            int popKindIndex = 0;
            List<PopCan> pops = new List<PopCan> { new PopCan("Coke") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);
            // 2
            popKindIndex = 1;
            pops = new List<PopCan> { new PopCan("water") };
            // 3
            popKindIndex = 2;
            pops = new List<PopCan> { new PopCan("stuff") };

            // insert coins
            Coin coin = new Coin(100);
            vm.CoinSlot.AddCoin(coin);
            coin = new Coin(100);
            vm.CoinSlot.AddCoin(coin);
            coin = new Coin(25);
            vm.CoinSlot.AddCoin(coin);
            coin = new Coin(25);
            vm.CoinSlot.AddCoin(coin);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            //=================
            // TEST ASSERT ZONE
            //=================
            // now check items
            int expectedItems = 1;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable> { new PopCan("Coke") };
            // check if delivery correct
            CollectionAssert.AreEqual(expectedList, itemsList);

            var storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm.CoinRacks) {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach(var popCanRack in vm.PopCanRacks) {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }
            // check if teardown correct
        }

        //================
        // HELPER METHOD
        //================
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
    }
}
