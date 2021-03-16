namespace passwordking
{
    public class Entry
    {
        public string Name;
        public string Password;
        public Entry(string _name, string _password)
        {
            Name = _name;
            Password = _password;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
