using Blazored.Toast.Services;
using McHealthCare.Application.Dtos.Others;
using McHealthCare.Application.Services;
using Microsoft.JSInterop;

namespace McHealthCare.Web.Extentions.CS
{
    public static class Helper
    {
        public static List<string> URLS =
        [
            // Clinic Services
            "clinic-service/general-consultation-services",
            "clinic-service/medical-checkups",
            "clinic-service/procedure-rooms",
            "clinic-service/acccidents",
            "clinic-service/reporting-and-analytics",

            // Queues
            "queue/queue-counters",
            "queue/queue-displays",
            "queue/kiosk-departments",
            "queue/kiosk-configurations",

            // BPJS 
            "bpjs/bpjs-classifications",
            "bpjs/system-parameters",

            // BPJS Configurations
            "bpjs-integration/physicians",
            "bpjs-integration/services",
            "bpjs-integration/medical-procedures",
            "bpjs-integration/awareness",
            "bpjs-integration/diagnoses",
            "bpjs-integration/providers",
            "bpjs-integration/allergies",
            "bpjs-integration/drugs",

            // Patiens
            "patient/patients",
            "patient/family-relations",
            "patient/insurance-policies",

            // Pharmacies
            "pharmacy/presciptions",
            "pharmacy/medicament-groups",
            "pharmacy/signas",
            "pharmacy/medicine-dosages",
            "pharmacy/active-components",
            "pharmacy/drug-forms",
            "pharmacy/drug-routes",
            "pharmacy/reporting-pharmacy",

            // Inventories
            "inventory/products",
            "inventory/product-categories",
            "inventory/inventory-adjusments",
            "inventory/internal-transfers",
            "inventory/goods-receipts",
            "inventory/uom-categories",
            "inventory/uoms",
            "inventory/locations",
            "inventory/reporting-inventories",

            // Employees
            "employee/employees",
            "employee/sick-leave-managements",
            "employee/claim-managements",
            "employee/departments",
            "employee/job-positions",

            // Medicals
            "medical/practitioners",
            "medical/doctor-schedules",
            "medical/doctor-schedule-slots",
            "medical/insurances",
            "medical/specialities",
            "medical/diagnoses",
            "medical/procedures",
            "medical/disease-categories",
            "medical/chronic-diagnoses",
            "medical/health-centers",
            "medical/building-and-locations",
            "medical/services",
            "medical/nursing-diagnoses",
            "medical/lab-tests",
            "medical/sample-types",
            "medical/uom-labs",

            // Configurations
            "configuration/users",
            "configuration/groups",
            "configuration/menus",
            "configuration/email-templates",
            "configuration/email-settings",
            "configuration/companies",
            "configuration/countries",
            "configuration/provinces",
            "configuration/cities",
            "configuration/districts",
            "configuration/sub-districts",
            "configuration/religions",
            "configuration/occupationals",
        ];

        public static async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        public static void ShowErrorImport(this IToastService ToastService, int row, int col, string val)
        {
            ToastService.ShowInfo($"Data \"{val}\" in row {row} and column {col} is invalid");
        }
    }
}
