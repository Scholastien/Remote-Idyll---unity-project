
using System.Collections.Generic;
using System;

public class ResourceItemException : Exception
{
    public List<string> listOfOccurrence;

    public ResourceItemException(List<string> occ, string message) : base(message)
    {
        listOfOccurrence = occ;
    }
}