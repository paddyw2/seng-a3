//==================================//
// UnitTest1 - Good Test Scripts    //
//                                  //
// This class contains unit tests   //
// for all the 'good' test scripts  //
// that is, test scripts which      //
// should not fail                  //
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
    public class GoodTests
    {
        private HelperMethods helper = new HelperMethods();

        //================
        // T01 Test Method
        //================
        // This method recreates and tests
        // the first good test script
        [TestMethod]
        public void T01goodInsertAndPressExactChange()
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
            helper.loadCoins(5, 1, 0, vm);
            helper.loadCoins(10, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(100, 0, 3, vm);

            // load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // insert coins
            helper.insertCoins(new int[] { 100, 100, 25, 25 }, vm);
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

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T02 Test Method
        //================
        // This method recreates and tests
        // the second good test script
        [TestMethod]
        public void T02goodInsertAndPressChangeExpected()
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
            helper.loadCoins(5, 1, 0, vm);
            helper.loadCoins(10, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(100, 0, 3, vm);

            // load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // insert coins
            helper.insertCoins(new int[] { 100, 100, 100}, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 3;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable> {
                new PopCan("Coke"),
                new Coin(25),
                new Coin(25)
            };
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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(10) },
                new List<Coin>(),
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T03 Test Method
        //================
        // This method recreates and tests
        // the third good test script
        [TestMethod]
        public void T03goodTeardownWithoutConfigureOrLoad()
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

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>();

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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin>(),
                new List<Coin>(),
                new List<Coin>()
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan>(),
                new List<PopCan>()
            };
            foreach(var popList in storedContents.PopCansInPopCanRacks)
            {
                //Console.WriteLine(popList.Count);
            }
            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T04 Test Method
        //================
        // This method recreates and tests
        // the fourth good test script
        [TestMethod]
        public void T04goodPressWithoutInsert()
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
            helper.loadCoins(5, 1, 0, vm);
            helper.loadCoins(10, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(100, 0, 3, vm);

            // load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>();

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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(10) },
                new List<Coin> { new Coin(25), new Coin(25)},
                new List<Coin>()
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan> { new PopCan("Coke") },
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T05 Test Method
        //================
        // This method recreates and tests
        // the fifth good test script
        [TestMethod]
        public void T05goodScrambledCoinKinds()
        {
            // set up create values
            int[] coinKindArray = { 100, 5, 25, 10 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 2;
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
            helper.loadCoins(100, 0, 0, vm);
            helper.loadCoins(5, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(10, 1, 3, vm);

            // load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>();

            // check if delivery correct
            Boolean success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            // now insert coins
            helper.insertCoins(new int[] { 100, 100, 100 }, vm);

            // press button
            value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 3;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>
            {
                new Coin(25),
                new Coin(25),
                new PopCan("Coke")
            };

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
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
            var expectedStorageBin = new List<Coin> { new Coin(100)};
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(100), new Coin(100) },
                new List<Coin> { new Coin(5) },
                new List<Coin>(),
                new List<Coin> { new Coin(10) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T06 Test Method
        //================
        // This method recreates and tests
        // the sixth good test script
        [TestMethod]
        public void T06goodExtractBeforeSale()
        {
            // set up create values
            int[] coinKindArray = { 100, 5, 25, 10 };
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
            helper.loadCoins(100, 0, 0, vm);
            helper.loadCoins(5, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(10, 1, 3, vm);

            // load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>();

            // check if delivery correct
            Boolean success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            // now insert coins
            helper.insertCoins(new int[] { 100, 100, 100 }, vm);

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>();

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(25), new Coin(25) },
                new List<Coin> { new Coin(10) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan> { new PopCan("Coke") },
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T07 Test Method
        //================
        // This method recreates and tests
        // the seventh good test script
        [TestMethod]
        public void T07goodChangingConfiguration()
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
            List<string> popNames = new List<string> { "A", "B", "C" };
            List<int> popCosts = new List<int> { 5, 10, 25};
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 1, 0, vm);
            helper.loadCoins(10, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(100, 0, 3, vm);

            // load pops
            helper.loadPops("A", 1, 0, vm);
            helper.loadPops("B", 1, 1, vm);
            helper.loadPops("C", 1, 2, vm);

            // re-configure machine
            popNames = new List<string> { "Coke", "water", "stuff" };
            popCosts = new List<int> { 250, 250, 205};
            vm.Configure(popNames, popCosts);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>();

            // check if delivery correct
            Boolean success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            // now insert coins
            helper.insertCoins(new int[] { 100, 100, 100 }, vm);

            // press button
            value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 3;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>
            {
                new Coin(25),
                new Coin(25),
                new PopCan("A")
            };

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(10) },
                new List<Coin>(),
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan> { new PopCan("B") },
                new List<PopCan> { new PopCan("C") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

         //================
        // T08 Test Method
        //================
        // This method recreates and tests
        // the eighth good test script
        [TestMethod]
        public void T08goodChangeConfiguration()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "stuff" };
            List<int> popCosts = new List<int> { 140 };
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 0, 0, vm);
            helper.loadCoins(10, 5, 1, vm);
            helper.loadCoins(25, 1, 2, vm);
            helper.loadCoins(100, 1, 3, vm);

            // load pops
            helper.loadPops("stuff", 1, 0, vm);

            // now insert coins
            helper.insertCoins(new int[] { 100, 100, 100 }, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 6;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>
            {
                new Coin(100),
                new Coin(10),
                new Coin(10),
                new Coin(10),
                new Coin(25),
                new PopCan("stuff")
            };

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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin> { new Coin(10), new Coin(10) },
                new List<Coin>(),
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T09 Test Method
        //================
        // This method recreates and tests
        // the ninth good test script
        [TestMethod]
        public void T09goodHardForChange()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "stuff" };
            List<int> popCosts = new List<int> { 140 };
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 1, 0, vm);
            helper.loadCoins(10, 6, 1, vm);
            helper.loadCoins(25, 1, 2, vm);
            helper.loadCoins(100, 1, 3, vm);

            // load pops
            helper.loadPops("stuff", 1, 0, vm);

            // now insert coins
            helper.insertCoins(new int[] { 100, 100, 100 }, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 7;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>
            {
                new Coin(100),
                new Coin(10),
                new Coin(10),
                new Coin(10),
                new Coin(25),
                new Coin(5),
                new PopCan("stuff")
            };

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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin> { new Coin(10), new Coin(10), new Coin(10) },
                new List<Coin>(),
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T10 Test Method
        //================
        // This method recreates and tests
        // the tenth good test script
        [TestMethod]
        public void T10goodInvalidCoin()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "stuff" };
            List<int> popCosts = new List<int> { 140 };
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 1, 0, vm);
            helper.loadCoins(10, 6, 1, vm);
            helper.loadCoins(25, 1, 2, vm);
            helper.loadCoins(100, 1, 3, vm);

            // load pops
            helper.loadPops("stuff", 1, 0, vm);

            // now insert coins
            helper.insertCoins(new int[] { 1, 139}, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 2;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>
            {
                new Coin(1),
                new Coin(139)
            };

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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) },
                new List<Coin> { new Coin(25) },
                new List<Coin> { new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T11 Test Method
        //================
        // This method recreates and tests
        // the eleventh good test script
        [TestMethod]
        public void T11goodExtractBeforeSaleComplete()
        {
            // set up create values
            int[] coinKindArray = { 100, 5, 25, 10 };
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
            helper.loadCoins(100, 0, 0, vm);
            helper.loadCoins(5, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(10, 1, 3, vm);

            // load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>();

            // check if delivery correct
            Boolean success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            // insert coins
            helper.insertCoins(new int[] { 100, 100, 100}, vm);

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 0;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>();

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin> { new Coin(5) },
                new List<Coin> { new Coin(25), new Coin(25) },
                new List<Coin> { new Coin(10) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan> { new PopCan("Coke") },
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);

            // re-load coins
            helper.loadCoins(100, 0, 0, vm);
            helper.loadCoins(5, 1, 1, vm);
            helper.loadCoins(25, 2, 2, vm);
            helper.loadCoins(10, 1, 3, vm);

            // re-load pops
            helper.loadPops("Coke", 1, 0, vm);
            helper.loadPops("water", 1, 1, vm);
            helper.loadPops("stuff", 1, 2, vm);

            // press button
            value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 3;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>
            {
                new Coin(25),
                new Coin(25),
                new PopCan("Coke")
            };

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            //===============
            // Check Teardown
            //===============
            // check if teardown correct
            // get the teardown items
            storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm.CoinRacks) {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach(var popCanRack in vm.PopCanRacks) {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // create expected lists
            expectedStorageBin = new List<Coin>();
            expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100) },
                new List<Coin> { new Coin(5) },
                new List<Coin>(),
                new List<Coin> { new Coin(10) }
            };
            expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);

            // set up create values
            int[] coinKindArray1 = { 100, 5, 25, 10 };
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popRackCapcity = 10;
            receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm1 = new VendingMachine(coinKindArray1, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm1);
            // configure machine
            popNames = new List<string> { "Coke", "water", "stuff" };
            popCosts = new List<int> { 250, 250, 205 };
            vm1.Configure(popNames, popCosts);
            popNames = new List<string> { "A", "B", "C" };
            popCosts = new List<int> { 5, 10, 25 };
            vm1.Configure(popNames, popCosts);

            //===============
            // Check Teardown
            //===============
            // check if teardown correct
            // get the teardown items
            storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm1.CoinRacks) {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm1.StorageBin.Unload());
            foreach(var popCanRack in vm1.PopCanRacks) {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // create expected lists
            expectedStorageBin = new List<Coin>();
            expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin>(),
                new List<Coin>(),
                new List<Coin>()
            };
            expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan>(),
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);

            // re-load coins
            helper.loadCoins(100, 0, 0, vm1);
            helper.loadCoins(5, 1, 1, vm1);
            helper.loadCoins(25, 2, 2, vm1);
            helper.loadCoins(10, 1, 3, vm1);

            // re-load pops
            helper.loadPops("A", 1, 0, vm1);
            helper.loadPops("B", 1, 1, vm1);
            helper.loadPops("C", 1, 2, vm1);

            // insert coins
            helper.insertCoins(new int[] { 10, 5, 10 }, vm1);

            // press button
            value = 2;
            vm1.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm1.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 1;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>
            {
                new PopCan("C")
            };

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            //===============
            // Check Teardown
            //===============
            // check if teardown correct
            // get the teardown items
            storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm1.CoinRacks) {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm1.StorageBin.Unload());
            foreach(var popCanRack in vm1.PopCanRacks) {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // create expected lists
            expectedStorageBin = new List<Coin>();
            expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin> { new Coin(5), new Coin(5) },
                new List<Coin> { new Coin(25), new Coin(25) },
                new List<Coin> { new Coin(10), new Coin(10), new Coin(10) }
            };
            expectedPops = new List<List<PopCan>> {
                new List<PopCan> { new PopCan("A") },
                new List<PopCan> { new PopCan("B") },
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }

        //================
        // T12 Test Method
        //================
        // This method recreates and tests
        // the twelfth good test script
        [TestMethod]
        public void T12goodApproximateChangeWithCredit()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "stuff" };
            List<int> popCosts = new List<int> { 140 };
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 0, 0, vm);
            helper.loadCoins(10, 5, 1, vm);
            helper.loadCoins(25, 1, 2, vm);
            helper.loadCoins(100, 1, 3, vm);

            // load pops
            helper.loadPops("stuff", 1, 0, vm);

            // now insert coins
            helper.insertCoins(new int[] { 100, 100, 100}, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 6;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>
            {
                new Coin(100),
                new Coin(25),
                new Coin(10),
                new Coin(10),
                new Coin(10),
                new PopCan("stuff")
            };

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
            var expectedStorageBin = new List<Coin>();
            var expectedCoins = new List<List<Coin>> {
                new List<Coin>(),
                new List<Coin> { new Coin(10), new Coin(10) },
                new List<Coin>(),
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100) }
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);

            // re-load coins
            helper.loadCoins(5, 10, 0, vm);
            helper.loadCoins(10, 10, 1, vm);
            helper.loadCoins(25, 10, 2, vm);
            helper.loadCoins(100, 10, 3, vm);

            // re-load pops
            helper.loadPops("stuff", 1, 0, vm);

            // insert coins
            helper.insertCoins(new int[] { 25, 100, 10 }, vm);

            // press button
            value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            items = vm.DeliveryChute.RemoveItems();
            itemsList = new List<IDeliverable>(items);

            // now check items
            expectedItems = 1;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            expectedList = new List<IDeliverable>
            {
                new PopCan("stuff")
            };

            // check if delivery correct
            success = helper.checkDelivery(expectedList, itemsList);
            Assert.AreEqual(success, true);

            //===============
            // Check Teardown
            //===============
            // check if teardown correct
            // get the teardown items
            storedContents = new VendingMachineStoredContents();
            foreach(var coinRack in vm.CoinRacks) {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach(var popCanRack in vm.PopCanRacks) {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // create expected lists
            expectedStorageBin = new List<Coin>
            {
                new Coin(25),
                new Coin(100),
                new Coin(10)
            };
            expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5) },
                new List<Coin> { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) },
                new List<Coin> { new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25) },
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100) },
            };
            expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan>(),
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        } 

        //================
        // T13 Test Method
        //================
        // This method recreates and tests
        // the thirteenth good test script
        [TestMethod]
        public void T13goodNeedToStorePayment()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);
            // configure machine
            List<string> popNames = new List<string> { "stuff" };
            List<int> popCosts = new List<int> { 135 };
            vm.Configure(popNames, popCosts);
            // load coins
            helper.loadCoins(5, 10, 0, vm);
            helper.loadCoins(10, 10, 1, vm);
            helper.loadCoins(25, 10, 2, vm);
            helper.loadCoins(100, 10, 3, vm);

            // load pops
            helper.loadPops("stuff", 1, 0, vm);

            // now insert coins
            helper.insertCoins(new int[] { 25, 100, 10}, vm);

            // press button
            int value = 0;
            vm.SelectionButtons[value].Press();

            //===============
            // Check Delivery
            //===============
            // extract
            var items = vm.DeliveryChute.RemoveItems();
            var itemsList = new List<IDeliverable>(items);

            // now check items
            int expectedItems = 1;
            if (itemsList.Count != expectedItems)
                Assert.Fail("Different number of items: " + itemsList.Count);

            List<IDeliverable> expectedList = new List<IDeliverable>
            {
                new PopCan("stuff") 
            };

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
                new Coin(25),
                new Coin(100),
                new Coin(10)
            };
            var expectedCoins = new List<List<Coin>> {
                new List<Coin> { new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5) },
                new List<Coin> { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) },
                new List<Coin> { new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25) },
                new List<Coin> { new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100) },
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan>(),
                new List<PopCan>(),
                new List<PopCan>()
            };

            success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }


    }

}
