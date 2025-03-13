<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HRDemoReportUI._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <hr />
    <main>
        <div class="d-flex align-items-end" style="gap: 6px;">
            <div class="form-group">
                <label class="form-label" for="MainContent_employeeSearch">Search Employee</label>
                <asp:TextBox ID="employeeSearch" runat="server" OnTextChanged="SearchText_Change" AutoPostBack="true" CssClass="form-control"/>
            </div>
            <div class="form-group">
                <label class="form-label" for="MainContent_employeeSelectList">Select Employee</label>
                <asp:DropDownList ID="employeeSelectList" runat="server" CssClass="form-control" style="min-width: max-content"/>
            </div>
            <div class="form-group">
                <label class="form-label" for="MainContent_reportYearSelectList">Year</label>
                <asp:DropDownList ID="reportYearSelectList" runat="server" CssClass="form-control" style="min-width: max-content" />
            </div>
            <div class="form-group">
                <label class="form-label" for="MainContent_reportMonthSelectList">Month</label>
                <asp:DropDownList ID="reportMonthSelectList" runat="server" CssClass="form-control" style="min-width: max-content" />
            </div>
        </div>
        <hr />
        <div class="d-flex justify-content-between align-items-end" style="gap: 6px;">
            <asp:Button UseSubmitBehavior="false" Text="Generate Employee's Report" ID="generateEmployeeReportBtn" runat="server" CssClass="btn btn-primary" OnClick="GenerateEmployeeReportBtn_Click"/>
            <asp:Button UseSubmitBehavior="false" Text="Generate My Report" ID="generateMyReportBtn" runat="server" CssClass="btn btn-primary" OnClick="GenerateMyReportBtn_Click"/>
        </div>
        <asp:HiddenField ID="timezoneOffset" runat="server" />
        <hr />
        <div class="m-2 p-4 px-5 border border-1" id="report-preview" style="max-width: 800px;" data-preview-available="<%:ReportData == null ? "false" : "true" %>">
            <%if (ReportData == null)
            {%>
                <div>Please generate a report to see its preview here.</div>
            <%} %>

            <% if (ReportData?.EmployeeReport != null) { %>
                <div class="d-flex align-items-start justify-content-between" style="gap: 6px;">
                    <div>
                        <h3>Monthly Employee Report</h3>
                        <h4><%: ReportData.EmployeeReport.FirstName %> <%: ReportData.EmployeeReport.LastName %> (<%: ReportData.EmployeeReport.State %>, <%: ReportData.EmployeeReport.Country %>)</h4>
                        <h6><%: reportYearSelectList.SelectedValue %> <%: reportMonthSelectList.SelectedItem.Text%></h6>
                    </div>
                    <%if (IsMyReport) {%>
                      <asp:Button UseSubmitBehavior="false" ID="getPDFReportBtn_Self" runat="server" Text="Download PDF" CssClass="btn btn-primary" OnClick="GetPDFReportBtn_Click"/>
                    <%} else {%>
                      <asp:Button UseSubmitBehavior="false" ID="getPDFReportBtn_Employee" runat="server" Text="Download PDF" CssClass="btn btn-primary" OnClick="GetPDFReportBtn_Click"/>
                    <%} %>
                </div>
                <hr />

                <h5>Salary Calculation</h5>
                <table class="table table-bordered table-two-cols">
                    <tbody>
                        <tr>
                            <td><strong>Working Days</strong></td>
                            <td><%: ReportData.EmployeeReport.WorkingDaysInMonth %> (out of <%:ReportData.EmployeeReport.WorkingDaysInYear %>)</td>
                        </tr>
                        <tr>
                            <td><strong>Annual Salary</strong></td>
                            <td>$<%: ReportData.EmployeeReport.AnnualSalary %></td>
                        </tr>
                        <tr>
                            <td><strong>Gross Salary</strong></td>
                            <td>$<%: ReportData.EmployeeReport.MonthSalary %></td>
                        </tr>
                        <tr>
                            <td><strong>Daily Wage</strong></td>
                            <td>$<%: Math.Round(ReportData.EmployeeReport.MonthSalary / ReportData.EmployeeReport.WorkingDaysInMonth, 2) %></td>
                        </tr>
                    </tbody>
                </table>
                <hr />
                <h5>Attendance Summary</h5>
                <table class="table table-bordered table-two-cols">
                    <tbody>
                        <tr>
                            <td><strong>Present Days</strong></td>
                            <td><%: ReportData.EmployeeReport.PresentDays %></td>
                        </tr>
                        <tr>
                            <td><strong>Late Days</strong></td>
                            <td><%: ReportData.EmployeeReport.LateDays %></td>
                        </tr>
                        <tr>
                            <td><strong>Leave Days</strong></td>
                            <td><%: ReportData.EmployeeReport.LeaveDays %></td>
                        </tr>
                        <tr>
                            <td><strong>Absent Days</strong></td>
                            <td>
                                <%: ReportData.EmployeeReport.AbsentDays %>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <hr />
                <h5>Leave Summary</h5>
                <table class="table table-bordered table-two-cols">
                    <tbody>
                        <tr>
                            <td><strong>Sick Leave</strong></td>
                            <td><%: ReportData.EmployeeReport.SickLeave %></td>
                        </tr>
                        <tr>
                            <td><strong>Casual Leave</strong></td>
                            <td><%: ReportData.EmployeeReport.CasualLeave %></td>
                        </tr>
                        <tr>
                            <td><strong>Parental Leave</strong></td>
                            <td><%: ReportData.EmployeeReport.ParentalLeave %></td>
                        </tr>
                        <tr>
                            <td><strong>Bereavement Leave</strong></td>
                            <td><%: ReportData.EmployeeReport.BereavementLeave %></td>
                        </tr>
                        <tr>
                            <td><strong>Unpaid Leave</strong></td>
                            <td><%: ReportData.EmployeeReport.UnpaidLeave %></td>
                        </tr>
                    </tbody>
                </table>
            <%} %>
 
            <% if (ReportData?.PayrollReports != null && ReportData.PayrollReports.Length > 0)
                { %>
                <hr />
                <h5>Payrolls</h5>
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <td class="fw-bold">Gross Amount</td>
                            <td class="fw-bold">Pre Tax Deduction</td>
                            <td class="fw-bold">Tax Deduction</td>
                            <td class="fw-bold">Post Tax Deduction</td>
                            <td class="fw-bold">Net Amount</td>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (var payroll in ReportData.PayrollReports)
                            { %>
                            <tr>
                                <td>$<%: payroll.GrossAmount %></td>
                                <td>$<%: payroll.PreTaxDeduction %></td>
                                <td>$<%: payroll.TaxDeduction %></td>
                                <td>$<%: payroll.PostTaxDeduction %></td>
                                <td>$<%: payroll.NetAmount %></td>
                            </tr>
                        <% } %>
                        <tr>
                            <td><strong>$<%: ReportData.PayrollReports.Sum(p => p.GrossAmount) %></strong></td>
                            <td><strong>$<%: ReportData.PayrollReports.Sum(p => p.PreTaxDeduction) %></strong></td>
                            <td><strong>$<%: ReportData.PayrollReports.Sum(p => p.TaxDeduction) %></strong></td>
                            <td><strong>$<%: ReportData.PayrollReports.Sum(p => p.PostTaxDeduction) %></strong></td>
                            <td><strong>$<%: ReportData.PayrollReports.Sum(p => p.NetAmount) %></strong></td>
                        </tr>
                    </tbody>
                </table>
                <hr />
                <div class="d-flex justify-content-between align-items-end report-footer" style="gap: 6px;">
                    <span class="small"><%:DateTime.Now.ToString("MM/dd/yyyy") %></span>
                    <span class="small">Page <span class="currentPageNumber">1</span> of <span class="totalPages" >1</span></span>
                </div>
            <%} %>
        </div>
    </main>

    <style>
        table td
        {
            width: auto;
        }

        table.table-two-cols td
        {
            width: 50%;
        }
    </style>

    <script>
        $(document).ready(function () {
            var buttonId = '#MainContent_generateEmployeeReportBtn';
            var listId = '#MainContent_employeeSelectList';
            // Initially disable the button if no value is selected
            $(buttonId).prop('disabled', $(listId).val().trim() === '0');

            // Enable or disable the button based on the input field value
            $(listId).on('change', function () {
                $(buttonId).prop('disabled', $(this).val().trim() === '0');
            });

            // set hidden field value for timezone offset
            $('#MainContent_timezoneOffset').val(-1 * new Date().getTimezoneOffset());
        });
    </script>

</asp:Content>
