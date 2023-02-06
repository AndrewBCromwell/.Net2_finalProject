using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayerFakes;
using LogicLayer;
using DataObjects;
using System;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class VenueManagerTests
    {
        private VenueManager venueManager = null;

        [TestInitialize]
        public void TestSetup()
        {
            venueManager = new VenueManager(new VenueAccessorFake());
        }

        [TestMethod]
        public void TestRetreiveVenuesReturnsTheCrorrectNumberOfVenues()
        {
            List<VenueVM> venues = null;
            int expectedResult = 2;
            int actualResult;

            venues = venueManager.RetreiveVenues();
            actualResult = venues.Count;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
