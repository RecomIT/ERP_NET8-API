using AutoMapper;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Domain.AIT;
using Shared.Payroll.Domain.Tax;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.DTO.Variable;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.Domain.Allowance;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.DTO.Configuration;
using Shared.Payroll.ViewModel.Configuration;
using Shared.Payroll.ViewModel.Tax;
using Shared.Employee.ViewModel.Info;

namespace Shared.Payroll
{
    public class PayrollModuleProfile : Profile
    {
        public PayrollModuleProfile()
        {
            CreateMap<DepositAllowancePaymentHistory, PaymentOfDepositAmountByConfigDTO>().ReverseMap();
            CreateMap<SalaryProcessDetail, SalaryProcessDetail>().ReverseMap();
            CreateMap<ConditionalDepositAllowanceConfig, ConditionalDepositAllowanceConfigDTO>().ReverseMap();
            CreateMap<ConditionalDepositAllowanceConfig, ConditionalDepositAllowanceConfigViewModel>().ReverseMap();
            CreateMap<TaxDocumentSubmission, AITSubmissionDTO>().ReverseMap();
            CreateMap<TaxDocumentSubmission, TaxRefundSubmissionDTO>().ReverseMap();
            CreateMap<TaxDocumentSubmission, TaxDocumentSubmissionDTO>().ReverseMap();
            CreateMap<EmployeeProjectedPaymentDTO, EmployeeProjectedPayment>().ReverseMap();
            CreateMap<SpecialTaxSlab, IncomeTaxSlabViewModel>().ReverseMap();
            CreateMap<TaxProcessInfo, SupplementaryPaymentTaxInfo>()
                .ForMember(
                    dest => dest.PaymentMonth,
                    opt => opt.MapFrom(src => src.Month)
                )
                .ForMember(
                    dest => dest.PaymentYear,
                    opt => opt.MapFrom(src => src.Year)
                );
            CreateMap<Shared.Payroll.Process.Tax.TaxDetailInTaxProcess, SupplementaryPaymentTaxDetail>()
                .ForMember(
                    dest => dest.PaymentMonth,
                    opt => opt.MapFrom(src => src.Month)
                )
                .ForMember(
                    dest => dest.PaymentYear,
                    opt => opt.MapFrom(src => src.Year)
                )
                .ForMember(
                    dest => dest.TillDateIncome,
                    opt => opt.MapFrom(src => src.TillAmount)
                )
                .ForMember(
                    dest => dest.ProjectedIncome,
                    opt => opt.MapFrom(src => src.ProjectedAmount)
                )
                .ForMember(
                    dest => dest.CurrentMonthIncome,
                    opt => opt.MapFrom(src => src.CurrentAmount)
                );
            CreateMap<TaxProcessSlab, SupplementaryPaymentTaxSlab>()
                 .ForMember(
                    dest => dest.PaymentMonth,
                    opt => opt.MapFrom(src => src.Month)
                )
                .ForMember(
                    dest => dest.PaymentYear,
                    opt => opt.MapFrom(src => src.Year)
                );


            CreateMap<TaxChallan, TaxChallanDTO>().ReverseMap();
            CreateMap<SalaryAllowanceConfigurationInfo, SalaryAllowanceConfigurationInfoDTO>().ReverseMap();
            CreateMap<SalaryAllowanceConfigurationDetail, SalaryAllowanceConfigurationDetailDTO>().ReverseMap();
            CreateMap<SalaryAllowanceConfigurationInfo, SalaryAllowanceConfigurationInfoViewModel>().ReverseMap();
            CreateMap<SalaryAllowanceConfigurationDetail, SalaryAllowanceConfigurationDetailViewModel>().ReverseMap();
            CreateMap<PeriodicallyVariableAllowanceInfo, PeriodicalAllowanceInfoDTO>().ReverseMap();
            CreateMap<PeriodicallyVariableAllowanceDetail, PeriodicalAllowanceDetailDTO>().ReverseMap();
            CreateMap<PrincipleAmountInfo, PrincipleAmountInfoDTO>().ReverseMap();
            CreateMap<EmployeeServiceDataViewModel, EligibleEmployeeForTaxType>().ReverseMap();
            CreateMap<EmployeeTaxProcess, FinalTaxProcess>().ReverseMap();
            CreateMap<EmployeeTaxProcessDetail, FinalTaxProcessDetail>().ReverseMap();
            CreateMap<EmployeeTaxProcessSlab, FinalTaxProcessSlab>().ReverseMap();
            CreateMap<EmployeeTaxProcessedInfo, FinalTaxProcessedInfo>().ReverseMap();
            CreateMap<EmployeeProjectedAllowanceProcessInfoDTO, EmployeeProjectedAllowanceProcessInfo>().ReverseMap();
            CreateMap<ConditionalProjectedPaymentDTO, ConditionalProjectedPayment>().ReverseMap();

        }
    }
}
