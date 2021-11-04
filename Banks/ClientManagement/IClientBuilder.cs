namespace Banks.ClientManagement
{
    public interface IClientBuilder
    {
        Client Build();
        IClientBuilder SetFirstname(string firstname);
        IClientBuilder SetSurname(string surname);
        IClientBuilder SetAddress(string address);
        IClientBuilder SetPassportData(string passportData);
    }
}