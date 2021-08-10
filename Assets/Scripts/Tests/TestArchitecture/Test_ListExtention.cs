using UnityEngine.TestTools;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Collections;

public class Test_ListExtention
{

    [Test]
    public void GetOccurenceList()
    {
        List<int> dataList = new List<int> { 1, 2, 3, 1 };
        List<int> expectedResult = new List<int> { 1 };

        List<int> result = ListExtention.GetOccurrenceList<int>(dataList);
        // Use the Assert class to test conditions.
        Assert.AreEqual(expectedResult, result, "RemoveDuplicateSet didn't worked as expected: instead of displaying: " + string.Join(",", expectedResult) + "\nit displayed: " + string.Join(",", result));
    }

    [Test]
    public void RemoveDuplicatesSet()
    {

        List<int> dataList = new List<int> { 1, 2, 3, 1 };
        List<int> expectedResult = new List<int> { 1, 2, 3 };
        List<int> result = ListExtention.RemoveDuplicatesSet<int>(dataList);
        // Use the Assert class to test conditions.
        Assert.AreEqual(expectedResult, result, "RemoveDuplicateSet didn't worked as expected: instead of displaying: " + string.Join(",", expectedResult) + "\nit displayed: " + string.Join(",", result));
    }

}
