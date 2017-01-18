using System;
// using System.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Windows.Automation;

namespace ClassLibrary1
{
    [TestClass]
    public class iTunesTest
    {

        [TestMethod]
       public void launchItunes()
        {
            iTunes = new iTunesInstance();
            iTunes.searchBarText = "Sidewalks";
            iTunes.isSelected("play targeted, Sidewalks (feat. Kendrick Lamar), Time 3:51, The Weeknd, Album Starboy, Genre R&B/Soul");
        }


        iTunesInstance iTunes;
    }
}
