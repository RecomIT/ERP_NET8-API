namespace Shared.Tools.Domain
{
    public class EmployeeTaxSlab
    {
        public int Id { get; set; }

        public decimal CurrentRate { get; set; } = 0; // 0%, 5%, 10%,15%, 20%

        public string Parameter { get; set; } = string.Empty; // On the Next BDT-300000

        public decimal TaxableIncome { get; set; } = 0;  // 300000.00

        public decimal TaxLiability { get; set; } = 0;  // 0
    }
}
