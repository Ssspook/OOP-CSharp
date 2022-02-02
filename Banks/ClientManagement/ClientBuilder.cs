namespace Banks.ClientManagement
{
    public class ClientBuilder : IClientBuilder
    {
        private string _firstname;
        private string _surname;
        private string _address;
        private string _passportData;

        public Client Build()
        {
            return new Client(_firstname, _surname, _address, _passportData);
        }

        public IClientBuilder SetFirstname(string firstname)
        {
            _firstname = firstname;
            return this;
        }

        public IClientBuilder SetSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public IClientBuilder SetAddress(string address)
        {
            _address = address;
            return this;
        }

        public IClientBuilder SetPassportData(string passportData)
        {
            _passportData = passportData;
            return this;
        }
    }
}