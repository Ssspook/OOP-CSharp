using Banks.AccountManagement;

namespace Banks.AccountCreation
{
    public interface IAccountCreator
    {
        IAccount CreateAccount();
    }
}