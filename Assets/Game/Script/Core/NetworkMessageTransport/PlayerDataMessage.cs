using Mirror;

public struct PlayerDataMessage : NetworkMessage
{
    public string SkinName;
    public int AvatarInt;
    public string Nickname;
}