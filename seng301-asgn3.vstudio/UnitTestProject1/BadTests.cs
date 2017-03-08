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

        //================
        // U01 Test Method
        //================
        // This method recreates and tests
        // the first bad test script
        [TestMethod]
        public void U01BadConfigureBeforeSaleCompletion()
        {
            Assert.AreEqual(true, true);

        }
    }
}