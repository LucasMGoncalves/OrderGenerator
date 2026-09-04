namespace OrderGenerator.Domain.Entities
{
    public class Symbol
    {
        public string Code { get; }

        public Symbol(string code)
        {
            Code = code;
        }
    }
}
