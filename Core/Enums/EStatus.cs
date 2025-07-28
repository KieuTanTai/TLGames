namespace TLGames.Core.Enums
{
    public enum EUserStatus
    {
        ACTIVE,
        LOCK,
        INACTIVE
    }

    public enum EUserRelationshipStatus
    {
        PENDING,
        ACCEPTED,
        REJECTED
    }

    public enum EProductGameMode
    {
        SINGLE_PLAYER,
        MULTi_PLAYER,
    }

    public enum ETypeItemCart
    {
        BUY,
        GIFT
    }

    public enum ETypePaymentMethod
    {
        VISA_OR_MASTERCARD,
        BANKING,
        MOMO
    }

    public enum EDiscountType
    {
        PERCENT,
        AMOUNT
    }

    public enum EInvoiceStatus
    {
        RETURN,
        SUCCESS,
        CANCEL
    }

    public enum ETransactionType
    {
        RETURN,
        PURCHASE,
        SELL
    }

    public enum ETransactionStatus
    {
        RETURN,
        SUCCESS,
        PENDING
    }

    public enum EConversationStatus
    {
        ACTIVE,
        ARCHIVED
    }

    public enum EMessageType
    {
        TEXT,
        IMAGE,
        VIDEO,
        FILE
    }

    public enum EActiveStatus
    {
        ACTIVE,
        INACTIVE
    }
}
