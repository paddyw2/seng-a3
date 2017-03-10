//==================================//
// UnitTest2 - Bonus Test Scripts   //
//                                  //
// This class contains unit tests   //
// for all the bonus test scripts   //
// i.e. test scripts which that     //
// are not covered by the provided  //
// test scripts                     //
//==================================//

using System;
using System.Collections.Generic;
using System.Linq;
using Frontend2;
using Frontend2.Hardware;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject1
{
    [TestClass]
    public class BonusTests
    {
        private HelperMethods helper = new HelperMethods();

        //================
        // B01 Test Method
        //================
        // This method checks that the
        // machine cannot be configured
        // with a pop with no name
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void B01invalidPopName()
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
            List<string> popNames = new List<string> { "", "water", "stuff" };
            List<int> popCosts = new List<int> { 250, 250, 205 };
            vm.Configure(popNames, popCosts);
        }

        //================
        // B02 Test Method
        //================
        // This method checks that the
        // machine cannot be configured
        // with an invalid capacity value
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void B02invalidCapacity()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 10;
            int popRackCapcity = 0;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
        }

        //================
        // B03 Test Method
        //================
        // This method checks that if
        // a coin rack is full then the
        // money goes into the storage
        // bin and cannot be used for
        // change
        [TestMethod]
        public void B03invalidCapacity()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 1;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "Diet", "water", "stuff" };
            List<int> popCosts = new List<int> { 5, 250, 205 };
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 1, 0, vm);

            // load pops
            helper.loadPops("Diet", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // insert coins
            helper.insertCoins(new int[] { 5, 5, 5 }, vm);
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
            int expectedItems = 2;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable> { new PopCan("Diet"), new Coin(5) };
            // check if delivery correct
            Boolean success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            //===============
            // Check Teardown
            //===============
            // check if teardown correct
            // get the teardown items
            var storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm.CoinRacks) {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach(var popCanRack in vm.PopCanRacks) {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // create expected lists
            var expectedStorageBin = new List<Coin>
            {
                new Coin(5), new Coin(5), new Coin(5)
            };
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin>(),
                new List<Coin>(),
                new List<Coin>()
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }
    }
}