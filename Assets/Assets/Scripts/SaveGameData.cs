using System;
using System.Collections.Generic;

[Serializable]
public struct SaveGameData
{
    public int deaths;
    public int level;
    public List<int> starsPerLevel;
}
