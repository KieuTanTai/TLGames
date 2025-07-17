namespace TLGames.Core.Enums
{
    public enum EDeleteStrategy
    {
        Restrict, // Ngăn chặn xóa nếu có con
        Cascade,  // Xóa cả con
        SetNull,  // Đặt FK con thành NULL
        Unlink
    }
}
