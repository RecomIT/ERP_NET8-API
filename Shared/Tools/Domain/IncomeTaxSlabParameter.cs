namespace Shared.Tools.Domain
{
    public class IncomeTaxSlabParameter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public decimal Amount { get; set; }

        public decimal Percentage { get; set; }

    }
}
