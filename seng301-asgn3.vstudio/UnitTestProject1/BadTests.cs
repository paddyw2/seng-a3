//==================================//
// UnitTest2 - Good Test Scripts    //
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
    public class BadTests
    {
        private HelperMethods helper = new HelperMethods();

        //================
        // U01 Test Method
        //================
        // This method recreates and tests
        // the first bad test script
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void U01badConfigureBeforeSaleCompletion()
        {
            VendingMachine vm = null;
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
                new List<Coin> { new Coin(25), new Coin(25) },
                new List<Coin>() 
            };
            var expectedPops = new List<List<PopCan>> {
                new List<PopCan> { new PopCan("Coke") },
                new List<PopCan> { new PopCan("water") },
                new List<PopCan> { new PopCan("stuff") }
            };

            Boolean success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);

        }

        //================
        // U02 Test Method
        //================
        // This method recreates and tests
        // the second bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U02badCostsList()
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

            List<int> popCosts = new List<int> { 250, 250, 0 };
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

            Boolean success = helper.checkTeardown(storedContents, expectedCoins, expectedPops, expectedStorageBin);
            Assert.AreEqual(success, true);
        }


        //================
        // U03 Test Method
        //================
        // This method recreates and tests
        // the third bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U03badNamesList()
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
            List<string> popNames = new List<string> { "Coke", "water" };

            List<int> popCosts = new List<int> { 250, 250};
            vm.Configure(popNames, popCosts);

        }

        //================
        // U04 Test Method
        //================
        // This method recreates and tests
        // the fourth bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U04badNonUniqueDenomination()
        {
            // set up create values
            int[] coinKindArray = { 1, 1 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);

        }

        //================
        // U05 Test Method
        //================
        // This method recreates and tests
        // the fifth bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U05badCoinKind()
        {
            // set up create values
            int[] coinKindArray = { 0 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);

        }

        //================
        // U06 Test Method
        //================
        // This method recreates and tests
        // the sixth bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U06badButtonNumber()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100};
            int selectionButtonCount = 3;
            int coinRackCapacity = 0;
            int popRackCapcity = 0;
            int receptacleCapacity = 0;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);

            // press button
            int value = 3;
            vm.SelectionButtons[value].Press();
        }

        //================
        // U07 Test Method
        //================
        // This method recreates and tests
        // the seventh bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U07badButtonNumber2()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100};
            int selectionButtonCount = 3;
            int coinRackCapacity = 0;
            int popRackCapcity = 0;
            int receptacleCapacity = 0;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);

            // press button
            int value = -1;
            vm.SelectionButtons[value].Press();

        }

        //================
        // U08 Test Method
        //================
        // This method recreates and tests
        // the eighth bad test script
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U08badButtonNumber3()
        {
            // set up create values
            int[] coinKindArray = { 5, 10, 25, 100};
            int selectionButtonCount = 3;
            int coinRackCapacity = 0;
            int popRackCapcity = 0;
            int receptacleCapacity = 0;
            // create vending machine, and vending machine logic using
            // these values
            var vm = new VendingMachine(coinKindArray, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            new VendingMachineLogic(vm);

            // press button
            int value = 4;
            vm.SelectionButtons[value].Press();
        }
    }
}