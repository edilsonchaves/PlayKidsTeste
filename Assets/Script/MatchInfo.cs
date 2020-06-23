using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MatchInfo
{
    public List<GameObject> matchItens;
    public int startXMatch, endingXMatch;
    public int startYMatch, endingYMatch;

    public bool getValidMatch()
    {
        if (matchItens != null)
        {
            return true;
        }
        return false;
    }
}
