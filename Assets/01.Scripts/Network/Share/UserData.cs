using System;
using Unity.Collections;

[Serializable]
public struct UserData
{
    public ulong clientID;
    public FixedString64Bytes playerName;
    public int characterIndex;
}
