using System.Collections.Generic;

namespace Shared.Overtime.DTO
{
    public class CreateOvertimeDTO
    {
        public List<OvertimeUnit> Units { get; set; }
        public List<OvertimeAmountType> AmountTypes { get; set; }

        public CreateOvertimeDTO()
        {
            Units = getUnits();
            AmountTypes = getAmountTypes();
        }

        private List<OvertimeUnit> getUnits()
        {
            var units = new List<OvertimeUnit>() {
                new OvertimeUnit(){ Id=1, Name="Hourly"},
                 new OvertimeUnit(){ Id=2, Name="Daily"},
                  new OvertimeUnit(){ Id=3, Name="Times"}
            };


            return units;
        }
        private List<OvertimeAmountType> getAmountTypes()
        {
            var types = new List<OvertimeAmountType>() {
                new OvertimeAmountType(){ Id=1, Name="Flat"},
                 new OvertimeAmountType(){ Id=2, Name="Percentage of Basic"},

            };
            return types;
        }


    }

    public class OvertimeUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class OvertimeAmountType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
