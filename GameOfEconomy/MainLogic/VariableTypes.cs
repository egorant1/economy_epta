namespace GameOfEconomy.MainLogic
{
    public class EVariable
    {
        public string Name { get; set; }
        public string LongName { get; set; }
    }

    public class EInstrVar : EVariable
    {
        public string Value { get; set; }
    }

    public class EInterVar : EVariable
    {

    }

    public class EEndVar : EVariable
    {

    }

    public class EConstVar : EVariable
    {

    }
}
