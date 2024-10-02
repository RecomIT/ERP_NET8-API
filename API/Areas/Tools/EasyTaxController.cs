using AutoMapper;
using BLL.Tools.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using Shared.OtherModels.Report;
using Shared.Tools.DTO;
using Shared.Tools.ViewModel;


namespace API.Areas.Tools_Module
{

    [ApiController, Area("Tools"), Route("api/[area]/[controller]")]
    public class EasyTaxController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IToolsBusiness _toolsBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EasyTaxController(IMapper mapper, IToolsBusiness toolsBusiness, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _toolsBusiness = toolsBusiness;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost, Route("TaxCard")]
        public async Task<ActionResult> TaxCard([FromBody] EasyTaxDTO model)
        {
            try {
                await Task.Delay(1000);

                var fiscalYear = _toolsBusiness.GetFiscalYear();
                var assismentYear = _toolsBusiness.GetAssismentYear();

                var salaryComponentDetails = _toolsBusiness.GetSalaryComponentDetails(model);

                var totalAnnualIncome = _toolsBusiness.GetTotalAnnualIncome(model);
                var exemptionOfAnnualIncome = _toolsBusiness.GetExemptionOfAnnualIncome(totalAnnualIncome);
                var taxableIncome = _toolsBusiness.GetTaxableIncome(totalAnnualIncome, exemptionOfAnnualIncome);

                var incomeTaxSlabParameters = _toolsBusiness.GetIncomeTaxSlabParameters(model);
                var employeeTaxSlabs = _toolsBusiness.GetEmployeeTaxSlabs(incomeTaxSlabParameters, taxableIncome);
                var investmentRebate = _toolsBusiness.GetInvestmentRebate(model.PF.Amount, model.ActualInvestmentAmount,taxableIncome);

                var taxLiability = employeeTaxSlabs.Sum(x => x.TaxLiability);
                var netTaxPayable = _toolsBusiness.GetNetTaxPayable(taxLiability, investmentRebate.TaxRebateDueToInvestment, model);

                #region Tax Card Report

                var reportData = new ReportTaxCard();

                reportData.EmployeeId = model.EmployeeId;
                reportData.EmployeeName = model.EmployeeName;
                reportData.Designation = model.Designation;
                reportData.Gender = model.Gender;
                reportData.FiscalYear = fiscalYear;
                reportData.AssesmentYear = assismentYear;
                reportData.TIN = model.TIN;


                reportData.TotalAnnualIncomeGross = totalAnnualIncome;
                reportData.TotalExemptionOfAnnualIncome = exemptionOfAnnualIncome;
                reportData.TotalTaxableIncome = taxableIncome;
                reportData.TaxLiability = taxLiability;


                reportData.PFContributionBothParts = investmentRebate.PFContributionsBothParts;
                reportData.OtherInvestmentExceptPF = investmentRebate.OtherInvestmentRecogExceptPF;
                reportData.ActualInvestementTotal = investmentRebate.ActualInvestmentMade;
                reportData.NetRebateAmount = investmentRebate.TaxRebateDueToInvestment;

                reportData.AdvanceIncomeTax = model.AITAmount;
                reportData.RefundAmount = model.RefundAmount;
                reportData.NetTaxPayable = netTaxPayable;

                var _reportFile = new ReportFile();
                
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";
                    
                var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\payroll\\EasyTax.rdlc";

                    
                LocalReport localReport = new LocalReport();

                ReportDataSource reportDataSource1 = new ReportDataSource {
                    Name = "EasyTaxProcessDetails",
                    Value = new List<ReportTaxCard> { reportData } // Needs Collection 
                };

                ReportDataSource reportDataSource2 = new ReportDataSource {
                    Name = "EasyTaxSlabs",
                    Value = employeeTaxSlabs
                };

                ReportDataSource reportDataSource3 = new ReportDataSource {
                    Name = "EasySalaryComponens",
                    Value = salaryComponentDetails
                };


                localReport.DataSources.Clear();

                localReport.DataSources.Add(reportDataSource1);
                localReport.DataSources.Add(reportDataSource2);
                localReport.DataSources.Add(reportDataSource3);
                    
                localReport.Refresh();

                localReport.ReportPath = path;
                var pdf = localReport.Render(renderFormat);

                _reportFile.FileBytes = pdf;
                _reportFile.Extension = extension;
                _reportFile.Mimetype = mimetype;

                _reportFile.FileBytes = pdf;
                _reportFile.Mimetype = mimetype;
                _reportFile.Extension = extension;

                #endregion

                return File(_reportFile.FileBytes, _reportFile.Mimetype, "TaxCard." + _reportFile.Extension);
            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}