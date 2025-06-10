namespace TLGames.Models
{
    internal class BankModel
    {
        public int BankId { get; private set; }
        public string BankName { get; private set; }
        public bool Status { get; private set; }

        public BankModel() { }

        public BankModel(int id, string name, bool status)
        {
            BankId = id;
            BankName = name;
            Status = status;
        }

        public void SetBankId(int id)
        {
            BankId = id;
        }

        public void SetBankName(string name)
        {
            BankName = name;
        }

        public void SetStatus(bool status)
        {
            Status = status;
        }
    }
}
