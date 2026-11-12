namespace Loveverse.Romance
{
    public enum InteractionType
    {
        Wave = 0,
        HighFive = 1,
        Handshake = 2,
        HoldHands = 3,
        Hug = 4,
        Dance = 5,
        SitTogether = 6,
        Stargaze = 7,
        GiveGift = 8
    }

    public enum InteractionState
    {
        Idle,
        RequestSent,
        RequestReceived,
        Aligning,
        Active,
        Canceled
    }
}
